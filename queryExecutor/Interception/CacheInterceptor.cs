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

        protected override void Proceed(IInvocation invocation)
        {
            string key = invocation.Method.Name + invocation.Arguments.Sum(a => a.GetHashCode());
            string region = invocation.Method.DeclaringType?.FullName;

            invocation.ReturnValue = _cacheManager.Get(key, region);
            //if (invocation.ReturnValue == null)
            {
                base.Proceed(invocation);
                _cacheManager.Add(key, invocation.ReturnValue, region);
            }
        }
    }
}