using System.Linq;
using System.Web.Http;
using System.Web.OData;
using queryExecutor.CQRS.Query;
using queryExecutor.Domain.DscQColumn;
using queryExecutor.Domain.DscQColumn.Query;

namespace queryExecutor.Controllers
{
   
    public class ColumnsController : ODataController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ColumnsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        // GET: odata/Columns
        [EnableQuery]
        public IQueryable<DscQColumn> Get([FromODataUri] string datasource, [FromODataUri] string path)
        {
            DscQColumnQuery columnQuery = new DscQColumnQuery()
            {
                Path = path.Replace(DscQRouteHandler.RandomWord, "\\"),
                DataSource = datasource,
                UserId = "ECO",
                Password = "ECO"
            };

            DscQColumnQueryResult result = _queryDispatcher.Dispatch<DscQColumnQuery, DscQColumnQueryResult>(columnQuery);
            return result.Items;
        }

        // GET: odata/Columns(5)
        public SingleResult<DscQColumn> Get([FromODataUri] string datasource, [FromODataUri] string path, [FromODataUri] long key)
        {
            DscQColumnQuery columnQuery = new DscQColumnQuery()
            {
                Path = path.Replace(DscQRouteHandler.RandomWord, "\\"),
                DataSource = datasource,
                UserId = "ECO",
                Password = "ECO"
            };

            DscQColumnQueryResult result = _queryDispatcher.Dispatch<DscQColumnQuery, DscQColumnQueryResult>(columnQuery);
            return SingleResult.Create(result.Items.Where(c => c.No == key));
        }
    }
}