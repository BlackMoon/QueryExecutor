// ReSharper disable InconsistentNaming

using System.Configuration;

namespace queryExecutor.DbManager.Oracle
{
    public interface IOracleEnvironmentConfiguration
    {
        string Oracle_Home { get; }

        string Nls_Lang { get; }

        string Path { get; }

        string Tns_Admin { get; }
    }

    /// <summary>
    /// Oracle. Настройки среды
    /// </summary>
    public class OracleEnvironmentConfiguration : ConfigurationSection, IOracleEnvironmentConfiguration
    {
        [ConfigurationProperty("oracleHome")]
        public string Oracle_Home => (string) this["oracleHome"];

        [ConfigurationProperty("nlsLang" )]
        public string Nls_Lang => (string)this["nlsLang"];

        [ConfigurationProperty("path")]
        public string Path => (string)this["path"];

        [ConfigurationProperty("tnsAdmin")]
        public string Tns_Admin => (string)this["tnsAdmin"];
    }
}
