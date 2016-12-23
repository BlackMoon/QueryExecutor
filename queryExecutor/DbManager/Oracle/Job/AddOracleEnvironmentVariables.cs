using System;
using queryExecutor.CQRS.Job;

namespace queryExecutor.DbManager.Oracle.Job
{
    /// <summary>
    /// Задача - (пере)регистрация в системе [Oracle Environment Variales]
    /// </summary>
    public class AddOracleEnvironmentVariables : IStartupJob
    {
        private readonly OracleEnvironmentConfiguration _config;
        
        public AddOracleEnvironmentVariables(OracleEnvironmentConfiguration config)
        {
            _config = config;
        }

        public void Run()
        {
            if (!string.IsNullOrEmpty(_config.Nls_Lang))
                Environment.SetEnvironmentVariable("NLS_LANG", _config.Nls_Lang);

            if (!string.IsNullOrEmpty(_config.Oracle_Home))
                Environment.SetEnvironmentVariable("ORACLE_HOME", _config.Oracle_Home);

            if (!string.IsNullOrEmpty(_config.Path))
                Environment.SetEnvironmentVariable("PATH", _config.Path + ";" + Environment.GetEnvironmentVariable("PATH"));

            if (!string.IsNullOrEmpty(_config.Tns_Admin))
                Environment.SetEnvironmentVariable("TNS_ADMIN", _config.Tns_Admin);
        }
    }
}
