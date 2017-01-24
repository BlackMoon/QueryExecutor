using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using Microsoft.OData.Edm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using queryExecutor.Domain.DscQueryParameter;

namespace queryExecutor.Tests
{
    [TestClass]
    public class ParametersControllerTest
    {
        private const string AcceptJson = "application/json";
        private const string Path = "Аварийные скважины/Анализ скважин за период";

        private const string DataSource = "aql.ECO";
        private const string Password = "ECO";
        private const string UserId = "ECO";

        private readonly HttpClient _client;

        public ParametersControllerTest()
        {
            var configuration = new HttpConfiguration
            {
                IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always
            };

            configuration.MapODataServiceRoute(
                 routeName: "DscQuery",
                 routePrefix: "{datasource}/{path}/odata",
                 model: GetQueryEdmModel());

            HttpServer server = new HttpServer(configuration);
            _client = new HttpClient(server);
        }


        [TestMethod]
        public void ParametersControllerShouldReturnData()
        {
            // Arrange
            string uri = $"{DataSource}/{Path}/odata/Parameters";

            // Act
            HttpResponseMessage response = GetResponse(uri, AcceptJson);

            // Assert
            Assert.IsNotNull(response);
            Assert.Equals(HttpStatusCode.OK, response.StatusCode);
        }

        private IEdmModel GetQueryEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<DscQParameter>("Parameters");

            return builder.GetEdmModel();
        }

        private HttpResponseMessage GetResponse(string uri, string acceptHeader)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost/{uri}");
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(acceptHeader));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{UserId}:{Password}")));
            return _client.SendAsync(request).Result;
        }
    }
}
