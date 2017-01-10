using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.OData;
using queryExecutor.Domain.DscQueryData;
using queryExecutor.CQRS.Query;
using queryExecutor.Domain.DscQueryData.Query;
using queryExecutor.Domain.DscQueryParameter;
using queryExecutor.Domain.DscQueryParameter.Query;
using queryExecutor.Identity;

namespace queryExecutor.Controllers
{
    [BasicAuthentication]
    public class ResultsController : ODataController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ResultsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [EnableQuery]
        // GET: odata/Data
        public IEnumerable<DscQData> Get([FromODataUri] string datasource, [FromODataUri] string path)
        {
            ClaimsPrincipal cp = (ClaimsPrincipal)User;

            string user = cp.FindFirst(ClaimTypes.Name)?.Value;
            string pswd = cp.FindFirst(BasicClaimTypes.Password)?.Value;

            // DscQParameters (из кеша)
            DscQParameterQuery parameterQuery = new DscQParameterQuery()
            {
                Path = path.Replace(DscQRouteHandler.RandomWord, "\\"),
                DataSource = datasource,
                UserId = user,
                Password = pswd
            };

            DscQParameterQueryResult parameterResult = _queryDispatcher.Dispatch<DscQParameterQuery, DscQParameterQueryResult>(parameterQuery);

            DscQDataQuery dataQuery = new DscQDataQuery()
            {
                Path = path.Replace(DscQRouteHandler.RandomWord, "\\"),
                DataSource = datasource,
                UserId = user,
                Password = pswd,
                Parameters = Request.GetQueryNameValuePairs()
                    .Where(p => !p.Key.StartsWith("$"))
                    .Select(p => new DscQParameter()
                    {
                        FieldCode = p.Key,
                        Value = p.Value,
                        // valueType из списка DscQParameter's
                        ValueType = parameterResult
                            .Items
                            .FirstOrDefault(i => i.FieldCode.Equals(p.Key, StringComparison.OrdinalIgnoreCase))
                            ?.ValueType
                    })
                    .ToList()
            };

            DscQDataQueryResult result = _queryDispatcher.Dispatch<DscQDataQuery, DscQDataQueryResult>(dataQuery);
            return result.Items;
        }

        [EnableQuery]
        // GET: odata/Data(5)
        public SingleResult<DscQData> Get([FromODataUri] string datasource, [FromODataUri] string path, [FromODataUri] long key)
        {
            ClaimsPrincipal cp = (ClaimsPrincipal)User;

            DscQDataQuery dataQuery = new DscQDataQuery()
            {
                Path = path.Replace(DscQRouteHandler.RandomWord, "\\"),
                DataSource = datasource,
                UserId = cp.FindFirst(ClaimTypes.Name)?.Value,
                Password = cp.FindFirst(BasicClaimTypes.Password)?.Value,
                Parameters = Request.GetQueryNameValuePairs()
                    .Where(p => !p.Key.StartsWith("$"))
                    .Select(p => new DscQParameter() { FieldCode = p.Key, Value = p.Value })
                    .ToList()
            };

            DscQDataQueryResult result = _queryDispatcher.Dispatch<DscQDataQuery, DscQDataQueryResult>(dataQuery);
            return SingleResult.Create(result.Items);
        }
    }
}
