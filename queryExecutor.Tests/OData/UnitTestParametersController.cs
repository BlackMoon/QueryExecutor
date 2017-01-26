using System.Linq;
using System.Net.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using Microsoft.OData.Edm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using queryExecutor.Controllers;
using queryExecutor.CQRS.Query;
using queryExecutor.Domain;
using queryExecutor.Domain.DscQColumn;
using queryExecutor.Domain.DscQColumn.Query;
using queryExecutor.Domain.DscQueryParameter;
using queryExecutor.Domain.DscQueryParameter.Query;

namespace queryExecutor.Tests.OData
{
    [TestClass]
    public class UnitTestParametersController : UnitTestBase
    {
        private readonly Mock<IQueryDispatcher> _mock = new Mock<IQueryDispatcher>(MockBehavior.Strict);

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            // setup the mock
            _mock
                .Setup(m => m.Dispatch<DscQParameterQuery, DscQParameterQueryResult>(It.IsAny<DscQParameterQuery>()))
                .Returns((DscQParameterQuery q) =>
                {

                    if (q.DataSource.Equals(DataSource) && q.Path.Equals(Path) && q.UserId.Equals(UserId) && q.Password.Equals(Password))
                    {
                        return
                            new DscQParameterQueryResult()
                            {
                                Items = new[]
                                {
                                    new DscQParameter()
                                    {
                                        No = 12673108,
                                        Name = "Дата начала",
                                        FieldCode = "PDATE_BEGIN",
                                        FormatMask = "dd.mm.yyyy",
                                        Precision = 0,
                                        Scale = 0,
                                        ValueType = EValueType.DATE
                                    },
                                    new DscQParameter()
                                    {
                                        No = 12673109,
                                        Name = "Дата окончания",
                                        FieldCode = "PDATE_END",
                                        FormatMask = "dd.mm.yyyy",
                                        Precision = 0,
                                        Scale = 0,
                                        ValueType = EValueType.DATE
                                    }
                                }
                                .AsQueryable()
                            };
                    }
                    return null;
                });
        }

        [TestMethod]
        public void Test_Get()
        {
            //arrange
            HttpRequestMessage request = GetRequest("aql.eco/Test/all_objects/odata/Parameters");

            ParametersController controller = new ParametersController(_mock.Object)
            {
                Request = request,
                User = GetPrincipal()
            };

            ODataQueryOptions opts = new ODataQueryOptions<DscQParameter>(new ODataQueryContext(GetEdmModel(), typeof(DscQParameter), new ODataPath()), request);

            //act
            IQueryable<DscQParameter> result = controller.Get(DataSource, Path);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 2);

            _mock.Verify(d => d.Dispatch<DscQParameterQuery, DscQParameterQueryResult>(It.IsAny<DscQParameterQuery>()), Times.Once());
        }
        

        protected override IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<DscQParameter>("Parameters");

            return builder.GetEdmModel();
        }
    }
}
