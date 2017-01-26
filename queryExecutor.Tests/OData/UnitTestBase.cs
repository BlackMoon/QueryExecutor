using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Moq;
using queryExecutor.CQRS.Query;
using queryExecutor.Identity;

namespace queryExecutor.Tests.OData
{
    public abstract class UnitTestBase
    {
        protected const string DataSource = "aql.eco";
        protected const string Path = "Test/all_objects";
        protected const string Password = "ECO";
        protected const string UserId = "ECO";

        private readonly HttpConfiguration _config = new HttpConfiguration { IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always };
        protected readonly Mock<IQueryDispatcher> Mock = new Mock<IQueryDispatcher>(MockBehavior.Strict);

        protected abstract IEdmModel GetEdmModel();

        public virtual void Initialize()
        {
            _config.MapODataServiceRoute(
                routeName: "DscQuery",
                routePrefix: "{datasource}/{path}/odata",
                model: GetEdmModel());

            _config.EnsureInitialized();
        }

        protected IPrincipal GetPrincipal()
        {
            Claim nameClaim = new Claim(ClaimTypes.Name, UserId),
                  pswdClaim = new Claim(BasicClaimTypes.Password, Password);

            ClaimsIdentity identity = new ClaimsIdentity(new[] { nameClaim, pswdClaim }, AuthenticationTypes.Basic);
            return new ClaimsPrincipal(identity);
        }

        protected HttpRequestMessage GetRequest(string uri)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost/{uri}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{UserId}:{Password}")));
            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, _config);
            return request;
        }

    }
}
