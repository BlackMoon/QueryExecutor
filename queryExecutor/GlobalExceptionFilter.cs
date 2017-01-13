using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace queryExecutor
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public bool AllowMultiple => false;

        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            Serilog.Log.Error(actionExecutedContext.Exception, string.Empty);
            return Task.FromResult<object>(null);
        }
    }
}