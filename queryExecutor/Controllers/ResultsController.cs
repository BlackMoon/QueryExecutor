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
        public IQueryable<DscQData> Get([FromODataUri] string datasource, [FromODataUri] string path)
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
                    .Select((p, i) => new DscQParameter()
                    {
                        // заполнение ключа [No] для вычисления хеша
                        No = i + 1,
                        FieldCode = p.Key,
                        Value = p.Value,
                        // valueType из списка DscQParameter's
                        ValueType = parameterResult
                            .Items
                            .FirstOrDefault(item => item.FieldCode.Equals(p.Key, StringComparison.OrdinalIgnoreCase))
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
            IQueryable<DscQData> items = Get(datasource, path);
            return SingleResult.Create(items.Where(i => i.No == key));
        }
    }
}
