using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.OData;
using queryExecutor.CQRS.Query;
using queryExecutor.Domain.DscQueryParameter;
using queryExecutor.Domain.DscQueryParameter.Query;
using queryExecutor.Identity;
using queryExecutor.OData;

namespace queryExecutor.Controllers
{
    [BasicAuthentication]
    public class ParametersController : ODataController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ParametersController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        // GET: odata/Parameters
        [EnableQuery]
        public IQueryable<DscQParameter> Get(string datasource, string path)
        {
            ClaimsPrincipal cp = (ClaimsPrincipal)User;

            DscQParameterQuery parameterQuery = new DscQParameterQuery()
            {
                Path = path.Replace(DscQRouteHandler.RandomWord, "\\"),
                DataSource = datasource,
                UserId = cp.FindFirst(ClaimTypes.Name)?.Value,
                Password = cp.FindFirst(BasicClaimTypes.Password)?.Value
            };

            DscQParameterQueryResult result = _queryDispatcher.Dispatch<DscQParameterQuery, DscQParameterQueryResult>(parameterQuery);
            return result.Items.AsQueryable();
        }

        // GET: odata/Parameters(5)
        [EnableQuery]
        public SingleResult<DscQParameter> Get(string datasource, string path, long key)
        {
            IQueryable<DscQParameter> items = Get(datasource, path);
            return SingleResult.Create(items.Where(i => i.No == key));
        }
    }
}
