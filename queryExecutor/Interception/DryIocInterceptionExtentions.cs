using System;
using System.Linq;
using Castle.DynamicProxy;
using DryIoc;

namespace queryExecutor.Interception
{
    public static class DryIocInterceptionExtentions
    {
        private static DefaultProxyBuilder _proxyBuilder;

        private static DefaultProxyBuilder ProxyBuilder => _proxyBuilder ?? (_proxyBuilder = new DefaultProxyBuilder());

        public static void Intercept(this IRegistrator registrator, Type serviceType, Type interceptorType, object serviceKey = null)
        {
            Type proxyType;
            if (serviceType.IsInterface())
                proxyType = ProxyBuilder.CreateInterfaceProxyTypeWithTargetInterface(serviceType, ArrayTools.Empty<Type>(), ProxyGenerationOptions.Default);
            else if (serviceType.IsClass())
                proxyType = ProxyBuilder.CreateClassProxyType(serviceType, ArrayTools.Empty<Type>(), ProxyGenerationOptions.Default);
            else
                throw new ArgumentException($"Intercepted service type {serviceType} is not a supported: it is nor class nor interface");

            Setup decoratorSetup = serviceKey == null
                ? Setup.DecoratorWith(useDecorateeReuse: true)
                : Setup.DecoratorWith(r => serviceKey.Equals(r.ServiceKey), useDecorateeReuse: true);

            registrator.Register(serviceType, proxyType,
                made: Made.Of(type => type.GetPublicInstanceConstructors().SingleOrDefault(c => c.GetParameters().Length != 0), Parameters.Of.Type<IInterceptor[]>(interceptorType.MakeArrayType())),
                setup: decoratorSetup);
        }

        public static void Intercept<TService, TInterceptor>(this IRegistrator registrator, object serviceKey = null) where TInterceptor : class, IInterceptor
        {
            registrator.Intercept(typeof(TService), typeof(TInterceptor[]), serviceKey);
        }
    }
}