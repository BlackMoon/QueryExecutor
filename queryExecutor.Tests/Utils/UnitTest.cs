using System.Linq;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using queryExecutor.Domain;
using queryExecutor.Domain.DscQColumn;
using queryExecutor.Domain.DscQColumn.Query;
using queryExecutor.Domain.DscQueryData;
using queryExecutor.Domain.DscQueryData.Query;
using queryExecutor.Domain.DscQueryParameter;
using queryExecutor.Domain.DscQueryParameter.Query;

namespace queryExecutor.Tests.Utils
{
    [TestClass]
    public class UnitTest
    {
        private readonly Mock<IUtilsChannel> _channelMock = new Mock<IUtilsChannel>(MockBehavior.Strict);

        private ChannelFactory<IUtils> _factory;
        private IUtils _client;

        [TestInitialize]
        public void Initialize()
        {
            // setup the mock
            _channelMock
                .Setup(c => c.GetColumns(It.IsAny<DscQColumnQuery>()))
                .Returns(() => new []
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
                });

            _channelMock
                .Setup(c => c.GetParameters(It.IsAny<DscQParameterQuery>()))
                .Returns(() => new []
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
                });

            _channelMock.Setup(c => c.GetResults(It.IsAny<DscQDataQuery>()))
                .Returns(() => new []
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
                });

            // setup the channel
            _factory = new ChannelFactory<IUtils>("WSHttpBinding_IUtils");
            if (_factory.Credentials != null)
            {
                _factory.Credentials.UserName.Password = "ECO";
                _factory.Credentials.UserName.UserName = "ECO";
            }
            _client = _factory.CreateChannel();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _factory.Close();                
        }

        [TestMethod]
        public void Test_GetColumns()
        {
            // arrange
            DscQColumnQuery query = new DscQColumnQuery()
            {
                DataSource = "aql.eco",
                Path = "Аварийные скважины/Скважины на которых были аварии"
            };

            // act
            DscQColumn[] result = _client.GetColumns(query);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Length, 7);
        }

        [TestMethod]
        public void Test_GetColumnsMock()
        {
            // arrange
            DscQColumnQuery query = new DscQColumnQuery();

            // act  
            DscQColumn[] result = _channelMock.Object.GetColumns(query);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Length, 3);

            _channelMock.Verify(c => c.GetColumns(query), Times.Once());
        }

        [TestMethod]
        public void Test_GetParameters()
        {
            // arrange
            DscQParameterQuery query = new DscQParameterQuery()
            {
                DataSource = "aql.eco",
                Path = "Аварийные скважины/Скважины на которых были аварии"
            };

            // act
            DscQParameter[] result = _client.GetParameters(query);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Length, 2);
        }

        [TestMethod]
        public void Test_GetParametersMock()
        {
            // arrange
            DscQParameterQuery query = new DscQParameterQuery();

            // act  
            DscQParameter[] result = _channelMock.Object.GetParameters(query);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Length, 2);

            _channelMock.Verify(c => c.GetParameters(query), Times.Once());
        }

        [TestMethod]
        public void Test_GetResults()
        {
            // arrange
            DscQDataQuery query = new DscQDataQuery()
            {
                DataSource = "aql.eco",
                Path = "Аварийные скважины/Скважины на которых были аварии"
            };
           
            DscQColumnQuery columnQuery = new DscQColumnQuery()
            {
                DataSource = "aql.eco",
                Path = "Аварийные скважины/Скважины на которых были аварии"
            };

            // act  
            DscQData[] result = _client.GetResults(query);
            DscQColumn[] columns = _client.GetColumns(columnQuery);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Length, 1);
            Assert.AreEqual(result.FirstOrDefault()?.DynamicProperties?.Count, columns.Length);
        }

        [TestMethod]
        public void Test_GetResultsMock()
        {
            // arrange
            DscQDataQuery query = new DscQDataQuery();
            DscQColumnQuery columnQuery = new DscQColumnQuery();

            // act  
            DscQData[] result = _channelMock.Object.GetResults(query);
            DscQColumn[] columns = _channelMock.Object.GetColumns(columnQuery);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Length, 1);
            Assert.AreEqual(result.FirstOrDefault()?.DynamicProperties?.Count, columns.Length);

            _channelMock.Verify(c => c.GetResults(query), Times.Once());
        }
    }
}
