using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using queryExecutor.Interception.Attribute;

namespace queryExecutor.Interception
{
    public abstract class Interceptor : IInterceptor
    {
        public virtual void Intercept(IInvocation invocation)
        {
            if (ShouldInterceptMethod(invocation.TargetType, invocation.Method))
                Proceed(invocation);
        }

        /// <summary>
        /// Перехватывать метод?
        /// <para>Перехватывается единственный декларированный метод или, если он помечен атрибутом [InterceptedMethodAttribute]</para>
        /// </summary>
        /// <param name="type">Тип перехватываемого объекта</param>
        /// <param name="methodInfo">Метод перехватывамого интерфейса</param>
        /// <returns></returns>
        protected virtual bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            bool shouldIntercept = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Length == 1;
            if (!shouldIntercept)
            {
                // поиск метода у объекта!
                MethodInfo mi = type.GetMethod(methodInfo.Name);
                shouldIntercept = mi.GetCustomAttributes(typeof (InterceptedMethodAttribute)).Any();
            }

            return shouldIntercept;
        }

        protected virtual void Proceed(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}