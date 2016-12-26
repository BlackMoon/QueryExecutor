using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using queryExecutor.CQRS.Command;
using queryExecutor.DbManager;

namespace queryExecutor.Domain.DscQueryData.Command
{
    public class DscQDataCommandHandler : ICommandHandlerWithResult<DscQDataCommand, DscQDataCommandResult>
    {
        private readonly IDbManager _dbManager;

        public DscQDataCommandHandler(IDbManager dbManager)
        {
            _dbManager = dbManager;
        }

        public DscQDataCommandResult Execute(DscQDataCommand command)
        {
            IQueryable<DscQData> dscQDatas = null;

            try
            {
                _dbManager.Open($"Data Source={command.DataSource};User Id={command.UserId};Password={command.Password}");
                _dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "DSC$QUERY_UTILS.query_run");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Join("\n", ex.Messages()));
            }
            finally
            {
                _dbManager.Close();
            }

            return new DscQDataCommandResult() { Items = dscQDatas };
        }
    }
}