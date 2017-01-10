using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace queryExecutor.Identity
{
    public class BasicAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authHeader = request.Headers.Authorization;

            if (authHeader == null || !authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase))
                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);

            else
            {
                string authToken = authHeader.Parameter;
                string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

                int pos = decodedToken.IndexOf(":", StringComparison.Ordinal);
                string username = decodedToken.Substring(0, pos++);
                string password = decodedToken.Substring(pos);

                // имя и пароль для ConnectionString
                Claim nameClaim = new Claim(ClaimTypes.Name, username),
                      pswdClaim = new Claim(BasicClaimTypes.Password, password);

                ClaimsIdentity identity = new ClaimsIdentity(new [] { nameClaim, pswdClaim }, AuthenticationTypes.Basic);
                context.Principal = new ClaimsPrincipal(identity);
            }

            return Task.FromResult<object>(null);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}