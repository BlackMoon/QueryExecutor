using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using queryExecutor.Domain.DscQueryData;
using queryExecutor.CQRS.Query;
using queryExecutor.Domain.DscQueryData.Query;
using queryExecutor.Domain.DscQueryParameter;

namespace queryExecutor.Controllers
{
    public class ResultsController : ODataController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ResultsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [EnableQuery]
        // GET: odata/Data
        public IEnumerable<DscQData> Get([FromODataUri] string datasource, [FromODataUri] string path, [FromODataUri] string parameters = null)
        {
            DscQDataQuery dataQuery = new DscQDataQuery()
            {
                Path = path.Replace(DscQRouteHandler.RandomWord, "\\"),
                DataSource = datasource,
                UserId = "ECO",
                Password = "ECO",
                Parameters = Request.GetQueryNameValuePairs()
                    .Where(p => !p.Key.StartsWith("$"))
                    .Select(p => new DscQParameter() { FieldCode = p.Key, Value = p.Value })
                    .ToList()
            };

            DscQDataQueryResult result = _queryDispatcher.Dispatch<DscQDataQuery, DscQDataQueryResult>(dataQuery);
            return result.Items;
        }

        [EnableQuery]
        // GET: odata/Data(5)
        public SingleResult<DscQData> Get([FromODataUri] string datasource, [FromODataUri] string path, [FromODataUri] long key)
        {
            DscQDataQuery dataQuery = new DscQDataQuery()
            {
                Path = path.Replace(DscQRouteHandler.RandomWord, "\\"),
                DataSource = datasource,
                UserId = "ECO",
                Password = "ECO"
            };

            DscQDataQueryResult result = _queryDispatcher.Dispatch<DscQDataQuery, DscQDataQueryResult>(dataQuery);
            return SingleResult.Create(result.Items);
        }
    }
}
