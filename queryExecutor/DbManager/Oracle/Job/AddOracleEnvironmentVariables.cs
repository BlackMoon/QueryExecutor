using System;
using queryExecutor.CQRS.Job;

namespace queryExecutor.DbManager.Oracle.Job
{
    /// <summary>
    /// Задача - (пере)регистрация в системе [Oracle Environment Variales]
    /// </summary>
    public class AddOracleEnvironmentVariables : IStartupJob
    {
        private readonly IOracleEnvironmentConfiguration _options;

        public AddOracleEnvironmentVariables(IOracleEnvironmentConfiguration options)
        {
            _options = options;
        }

        public void Run()
        {
            if (_options != null)
            {
                if (!string.IsNullOrEmpty(_options.Nls_Lang))
                    Environment.SetEnvironmentVariable("NLS_LANG", _options.Nls_Lang);

                if (!string.IsNullOrEmpty(_options.Oracle_Home))
                    Environment.SetEnvironmentVariable("ORACLE_HOME", _options.Oracle_Home);

                if (!string.IsNullOrEmpty(_options.Path))
                    Environment.SetEnvironmentVariable("PATH",
                        _options.Path + ";" + Environment.GetEnvironmentVariable("PATH"));

                if (!string.IsNullOrEmpty(_options.Tns_Admin))
                    Environment.SetEnvironmentVariable("TNS_ADMIN", _options.Tns_Admin);
            }
        }
    }
}