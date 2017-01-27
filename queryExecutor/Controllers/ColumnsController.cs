using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.OData;
using queryExecutor.CQRS.Query;
using queryExecutor.Domain.DscQColumn;
using queryExecutor.Domain.DscQColumn.Query;
using queryExecutor.Identity;
using queryExecutor.OData;

namespace queryExecutor.Controllers
{
    [BasicAuthentication]
    public class ColumnsController : ODataController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ColumnsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        // GET: odata/Columns
        [EnableQuery]
        public IQueryable<DscQColumn> Get(string datasource, string path)
        {
            ClaimsPrincipal cp = (ClaimsPrincipal)User;

            DscQColumnQuery columnQuery = new DscQColumnQuery()
            {
                Path = path.Replace(DscQRouteHandler.RandomWord, "\\"),
                DataSource = datasource,
                UserId = cp.FindFirst(ClaimTypes.Name)?.Value,
                Password = cp.FindFirst(BasicClaimTypes.Password)?.Value
            };

            DscQColumnQueryResult result = _queryDispatcher.Dispatch<DscQColumnQuery, DscQColumnQueryResult>(columnQuery);
            return result.Items.AsQueryable();
        }

        // GET: odata/Columns(5)
        [EnableQuery]
        public SingleResult<DscQColumn> Get(string datasource, string path, long key)
        {
            IQueryable<DscQColumn> items = Get(datasource, path);
            return SingleResult.Create(items.Where(i => i.No == key));
        }
    }
}