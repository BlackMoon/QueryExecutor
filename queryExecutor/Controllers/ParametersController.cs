using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.OData;
using queryExecutor.CQRS.Query;
using queryExecutor.Domain.DscQueryParameter;
using queryExecutor.Domain.DscQueryParameter.Query;
using queryExecutor.Identity;

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
        public IQueryable<DscQParameter> Get([FromODataUri] string datasource, [FromODataUri] string path)
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
            return result.Items;
        }

        // GET: odata/Parameters(5)
        public SingleResult<DscQParameter> Get([FromODataUri] string datasource, [FromODataUri] string path, [FromODataUri] long key)
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
            return SingleResult.Create(result.Items.Where(p => p.No == key));
        }
    }
}
