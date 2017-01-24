using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace queryExecutor.OData
{
    /// <summary>
    /// MessageHandler для DscQRoute
    /// <para>Заменяет / в сегменте {path} в odata-url вида {datasource}/{path}/odata. </para>
    /// </summary>
    public class DscQRouteHandler : DelegatingHandler
    {
        private const int WordLength = 3;
        private const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789~!";

        private static string _randomWord;

        public static string RandomWord
        {
            get
            {
                if (string.IsNullOrEmpty(_randomWord))
                {
                    Random rnd = new Random();
                    _randomWord = new string(Enumerable.Repeat(Chars, WordLength).Select(s => s[rnd.Next(s.Length)]).ToArray());
                }

                return _randomWord;
            }
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string uri = request.RequestUri.LocalPath;

            Regex rgx = new Regex("^/[\\w.-]+/(.+)/odata");
            Match matches = rgx.Match(uri);

            if (matches.Length > 0)
            {
                foreach (Match m in rgx.Matches(uri))
                {
                    if (m.Groups.Count > 1)
                    {
                        string oldValue = m.Groups[1].Value,
                            newValue = oldValue.Replace("/", RandomWord);

                        uri = uri.Replace(oldValue, newValue);
                    }
                }

                request.RequestUri = new Uri($"{request.RequestUri.Scheme}://{request.RequestUri.Host}:{request.RequestUri.Port}{uri}{request.RequestUri.Query}");
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}