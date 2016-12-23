using System.Data.Entity;
using Oracle.DataAccess.EntityFramework;

namespace queryExecutor.DbManager.Oracle
{
    /// <summary>
    /// Summary description for OracleDbConfiguration
    /// </summary>
    public class OracleDbConfiguration : DbConfiguration
    {
        public OracleDbConfiguration()
        {
            SetDefaultConnectionFactory(new OracleConnectionFactory());
            SetProviderServices("Oracle.DataAccess.Client", EFOracleProviderServices.Instance);
            SetProviderFactory("Oracle.DataAccess.Client", new global::Oracle.DataAccess.Client.OracleClientFactory());
        }
    }
}