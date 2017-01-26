using System.Linq;
using System.Net.Http;
using System.Security.Claims;
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
using queryExecutor.Identity;

namespace queryExecutor.Tests.OData
{
    [TestClass]
    public class UnitTestColumnsController : UnitTestBase
    {
        private readonly Mock<IQueryDispatcher> _mock = new Mock<IQueryDispatcher>(MockBehavior.Strict);

        [TestInitialize]
        public override void Initialize()
        {
           base.Initialize();

            // setup the mock
            _mock
                .Setup(m => m.Dispatch<DscQColumnQuery, DscQColumnQueryResult>(It.IsAny<DscQColumnQuery>()))
                .Returns((DscQColumnQuery q) =>
                {

                    if (q.DataSource.Equals(DataSource) && q.Path.Equals(Path) && q.UserId.Equals(UserId) && q.Password.Equals(Password))
                    {
                        return
                            new DscQColumnQueryResult
                            {
                                Items = new[]
                                {
                                    new DscQColumn()
                                    {
                                        No = 12673110,
                                        Name = "OBJ_NO",
                                        FieldCode = "OBJ_NO",
                                        Precision = 2,
                                        Scale = 15,
                                        ValueType = EValueType.NUMBER
                                    },
                                    new DscQColumn()
                                    {
                                        No = 12673111,
                                        Name = "Наименование скважины",
                                        FieldCode = "OBJ_NAME",
                                        Precision = 0,
                                        Scale = 0,
                                        ValueType = EValueType.VARCHAR
                                    },
                                    new DscQColumn()
                                    {
                                        No = 12673112,
                                        Name = "Наименование организации",
                                        FieldCode = "ORG_NAME",
                                        Precision = 0,
                                        Scale = 0,
                                        ValueType = EValueType.VARCHAR
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
            HttpRequestMessage request = GetRequest("aql.eco/Test/all_objects/odata/Columns");

            ColumnsController controller = new ColumnsController(_mock.Object)
            {
                Request = request,
                User = GetPrincipal()
            };
            
            ODataQueryOptions opts = new ODataQueryOptions<DscQColumn>(new ODataQueryContext(GetEdmModel(), typeof(DscQColumn), new ODataPath()), request);

            //act
            IQueryable<DscQColumn> result = controller.Get(DataSource, Path);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 3);
            
            _mock.Verify(d => d.Dispatch<DscQColumnQuery, DscQColumnQueryResult>(It.IsAny<DscQColumnQuery>()), Times.Once());
        }

        protected override IEdmModel GetEdmModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<DscQColumn>("Columns");

            return builder.GetEdmModel();
        }
    }
}
