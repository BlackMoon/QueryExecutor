using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using queryExecutor.Domain.DscQueryParameter;

namespace queryExecutor.Controllers
{
    public class ParametersController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        private static List<DscQParameter> ParamList = new List<DscQParameter>
        {
            new DscQParameter() {
                No = 11, FieldCode = "Lowest", ValueTypeNo = 1

            },
            new DscQParameter() {
                No = 33, FieldCode = "Highest", ValueTypeNo = 2

            },
            new DscQParameter() { No = 22, FieldCode = "Middle", ValueTypeNo = 3},
            new DscQParameter() { No = 3, FieldCode = "NewLow", ValueTypeNo = 4},
        };

        // GET: odata/Parameters
        [EnableQuery]
        public IQueryable<DscQParameter> Get([FromODataUri] string datasource, [FromODataUri] string path, [FromODataUri] string name)
        {
            return ParamList.AsQueryable();
        }

        // GET: odata/Parameters(5)
        public SingleResult<DscQParameter> Get([FromODataUri] string datasource, [FromODataUri] string path, [FromODataUri] string name, [FromODataUri] long key)
        {
            return SingleResult.Create(ParamList.AsQueryable().Where(p => p.No == key));
        }
    }
}
