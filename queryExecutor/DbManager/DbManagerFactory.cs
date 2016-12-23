using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace queryExecutor.DbManager
{
    /// <summary>
    /// Фабрика типов DbManager
    /// </summary>
    public static class DbManagerFactory
    {
        /// <summary>
        /// Словарь соответвий [наименование провайдера - тип DbManager'a] 
        /// </summary>
        private static readonly IDictionary<string, Type> Managers;

        static DbManagerFactory()
        {
            Func<Type, bool> pre = t => t.GetInterfaces().Contains(typeof(IDbManager));

            Assembly assembly = Assembly.GetExecutingAssembly();
            Managers = new Dictionary<string, Type>();
            foreach (Type t in assembly.GetTypes().Where(pre))
            {
                // Наименование --> из аттрибута
                ProviderNameAttribute attr = (ProviderNameAttribute)t.GetCustomAttribute(typeof(ProviderNameAttribute));
                if (attr != null)
                    Managers[attr.ProviderName] = t;
            }
        }

        public static IDbManager CreateDbManager(string providerName, string connectionString = null)
        {
            IDbManager dbManager = null;

            Type t = null;

            if (Managers != null)
                t = Managers.ContainsKey(providerName) ? Managers[providerName] : Managers.Values.FirstOrDefault();

            if (t != null)
            {
                dbManager = (IDbManager) Activator.CreateInstance(t);
                dbManager.ConnectionString = connectionString;
            }

            return dbManager;
        }
    }
}
