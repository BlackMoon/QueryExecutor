using System;

namespace queryExecutor.DbManager
{
    /// <summary>
    /// Аттрибут - наименование провайдера
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ProviderNameAttribute : Attribute
    {
        public ProviderNameAttribute(string provideName)
        {
            ProviderName = provideName;
        }
        
        public virtual string ProviderName { get; }
    }
}
