// ReSharper disable InconsistentNaming

using System.Configuration;

namespace queryExecutor.DbManager.Oracle
{
    /// <summary>
    /// Oracle. Настройки среды
    /// </summary>
    public class OracleEnvironmentConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("oracleHome")]
        public string Oracle_Home => (string)this["oracleHome"];

        [ConfigurationProperty("nlsLang" )]
        public string Nls_Lang => (string)this["nlsLang"];

        [ConfigurationProperty("path")]
        public string Path => (string)this["path"];

        [ConfigurationProperty("tnsAdmin")]
        public string Tns_Admin => (string)this["tnsAdmin"];
    }
}
