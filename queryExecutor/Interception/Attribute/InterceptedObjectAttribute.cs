using System;

namespace queryExecutor.Interception.Attribute
{
    /// <summary>
    /// Аттрибут-метка для перехватываемого объекта
    /// <para>[InterceptedObject(InterceptorType = typeof(CacheInterceptor))]</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class InterceptedObjectAttribute : System.Attribute
    {
        /// <summary>
        /// Тип объекта-перехватчика
        /// </summary>
        public Type InterceptorType { get; set; }

        /// <summary>
        /// Тип интерфейса перехватываемого объекта. Если не задан - берется 1й имплементируемый объектом интерфейс
        /// </summary>
        public Type ServiceInterfaceType { get; set; }
    }
}