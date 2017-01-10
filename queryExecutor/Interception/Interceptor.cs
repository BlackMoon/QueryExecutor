using Castle.DynamicProxy;

namespace queryExecutor.Interception
{
    public abstract class Interceptor : IInterceptor
    {
        public virtual void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}