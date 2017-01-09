using System;
using System.Linq;
using queryExecutor.CQRS.Query;
using queryExecutor.DbManager;

namespace queryExecutor.Domain.DscQueryParameter.Query
{
    public class DscQParameterQueryHandler : IQueryHandler<DscQParameterQuery, DscQParameterQueryResult>
    {
        private readonly IDbManager _dbManager;
        public DscQParameterQueryHandler(IDbManager dbManager)
        {
            _dbManager = dbManager;
        }

        public DscQParameterQueryResult Execute(DscQParameterQuery query)
        {
            IQueryable<DscQParameter> dscQParameters = null;
            try
            {
                string sql = @"SELECT p.no, p.name, p.field_Code fieldCode, ff.value_type_no valueType FROM DSC$QUERY_PARAMETERS p
                               JOIN TDF$FLEX_FIELDS ff ON ff.no = p.field_no 
                               WHERE p.query_no = dsc$utils.query_find(:p0)";

                _dbManager.Open($"Data Source={query.DataSource};User Id={query.UserId};Password={query.Password}");

                dscQParameters = _dbManager
                    .DbContext
                    .Set<DscQParameter>()
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

            return new DscQParameterQueryResult() { Items = dscQParameters };
        }
    }
}
