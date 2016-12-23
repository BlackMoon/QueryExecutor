using System.Data;
using queryExecutor.CQRS.Job;

namespace queryExecutor.DbManager.Oracle.Job
{
    /// <summary>
    /// Задача - (пере)регистрация в системе [Oracle Data Provider for .NET]
    /// </summary>
    public class AddOracleDataProvider : IStartupJob
    {
        public void Run()
        {
            DataSet ds = (DataSet)System.Configuration.ConfigurationManager.GetSection("system.data");
            if (ds?.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                bool invariantNameExists = dt.Columns.Contains("InvariantName");
                if (invariantNameExists)
                {
                    DataRow[] rows = dt.Select("InvariantName = 'Oracle.DataAccess.Client'");

                    foreach (DataRow row in rows)
                    {
                        dt.Rows.Remove(row);
                    }
                }

                dt.Rows.Add("Oracle Data Provider", "Oracle Data Provider for .NET", "Oracle.DataAccess.Client",
                    typeof(global::Oracle.DataAccess.Client.OracleClientFactory).AssemblyQualifiedName);
            }
        }
    }
}
