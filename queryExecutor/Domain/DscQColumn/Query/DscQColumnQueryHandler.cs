using System;
using System.Linq;
using queryExecutor.CQRS.Query;
using queryExecutor.DbManager;

namespace queryExecutor.Domain.DscQColumn.Query
{
    public class DscQColumnQueryHandler : IQueryHandler<DscQColumnQuery, DscQColumnQueryResult>
    {
        private readonly IDbManager _dbManager;

        public DscQColumnQueryHandler(IDbManager dbManager)
        {
            _dbManager = dbManager;
        }

        public DscQColumnQueryResult Execute(DscQColumnQuery query)
        {
            IQueryable<DscQColumn> dscQColumns = null;
            try
            {
                string sql = @"SELECT c.no, c.name, c.field_Code fieldCode, c.value_type_no valueType FROM DSC$QUERY_COLUMNS c
                               WHERE c.query_no = dsc$utils.query_find(:p0)";

                _dbManager.Open($"Data Source={query.DataSource};User Id={query.UserId};Password={query.Password}");

                dscQColumns = _dbManager
                    .DbContext
                    .Set<DscQColumn>()
                    .SqlQuery(sql, query.Path)
                    .AsQueryable();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Join("\n", ex.Messages()));
            }
            finally
            {
                _dbManager.Close();
            }

            return new DscQColumnQueryResult() { Items = dscQColumns };
        }
    }
}