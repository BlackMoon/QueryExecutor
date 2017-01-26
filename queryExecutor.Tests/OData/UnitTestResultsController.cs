using System.Linq;
using System.Net.Http;
using System.Security.Principal;
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
using queryExecutor.Domain.DscQueryData;
using queryExecutor.Domain.DscQueryData.Query;
using queryExecutor.Domain.DscQueryParameter;
using queryExecutor.Domain.DscQueryParameter.Query;

namespace queryExecutor.Tests.OData
{
    [TestClass]
    public class UnitTestResultsController : UnitTestBase
    {
        private readonly Mock<IQueryDispatcher> _mock = new Mock<IQueryDispatcher>(MockBehavior.Strict);

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            // setup the mock
            _mock
                .Setup(m => m.Dispatch<DscQDataQuery, DscQDataQueryResult>(It.IsAny<DscQDataQuery>()))
                .Returns((DscQDataQuery q) =>
                {
                    if (q.DataSource.Equals(DataSource) && q.Path.Equals(Path) && q.UserId.Equals(UserId) && q.Password.Equals(Password))
                    {
                        return
                            new DscQDataQueryResult
                            {
                                Items = new[]
                                {
                                    new DscQData()
                                    {
                                        No = 1,
                                        DynamicProperties =
                                        {
                                            { "OBJ_NO", 11872992 },
                                            { "OBJ_NAME", "20628" } ,
                                            { "ORG_NAME", "ЦДНГ №1, НГДУ \"Альметьевнефть\", ПАО \"Татнефть\"" }
                                        }
                                    }
                                }
                                .AsQueryable()
                            };
                    }
                    return null;
                });

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
            HttpRequestMessage request = GetRequest("aql.eco/Test/all_objects/odata/Results");
            IPrincipal principal = GetPrincipal();

            ResultsController controller = new ResultsController(_mock.Object)
            {
                Request = request,
                User = principal
            };

            ODataQueryOptions opts = new ODataQueryOptions<DscQData>(new ODataQueryContext(GetEdmModel(), typeof(DscQData), new ODataPath()), request);


            HttpRequestMessage request1 = GetRequest("aql.eco/Test/all_objects/odata/Results");

            ColumnsController controller1 = new ColumnsController(_mock.Object)
            {
                Request = request1,
                User = principal
            };

            ODataQueryOptions opts1 = new ODataQueryOptions<DscQColumn>(new ODataQueryContext(GetEdmModel(), typeof(DscQColumn), new ODataPath()), request);

            //act
            IQueryable<DscQData> result = controller.Get(DataSource, Path);
            IQueryable<DscQColumn> columns = controller1.Get(DataSource, Path);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(result.FirstOrDefault()?.DynamicProperties?.Count, columns.Count());

            _mock.Verify(d => d.Dispatch<DscQDataQuery, DscQDataQueryResult>(It.IsAny<DscQDataQuery>()), Times.Once());
        }

        protected override IEdmModel GetEdmModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<DscQColumn>("Columns");
            builder.EntitySet<DscQData>("Results");

            return builder.GetEdmModel();
        }
    }
}
