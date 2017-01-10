using System.Linq;
using CacheManager.Core;
using Castle.DynamicProxy;

namespace queryExecutor.Interception
{
    /// <summary>
    /// Interceptor для кеширования
    /// </summary>
    public class CacheInterceptor : Interceptor
    {
        private readonly ICacheManager<object> _cacheManager; 
        public CacheInterceptor(ICacheManager<object> cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public override void Intercept(IInvocation invocation)
        {
            string key = invocation.Method.DeclaringType + invocation.Method.Name + invocation.Arguments.Sum(a => a.GetHashCode());

            invocation.ReturnValue = _cacheManager.Get(key);
            if (invocation.ReturnValue == null)
            {
                base.Intercept(invocation);
                _cacheManager.Add(key, invocation.ReturnValue);
            }
        }
    }
}