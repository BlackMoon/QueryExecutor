using System;
using System.Data;
using System.Linq;
using queryExecutor.CQRS.Query;
using queryExecutor.DbManager;

namespace queryExecutor.Domain.DscQueryData.Query
{
    public class DscQDataQueryHandler : IQueryHandler<DscQDataQuery, DscQDataQueryResult>
    {
        private readonly IDbManager _dbManager;

        public DscQDataQueryHandler(IDbManager dbManager)
        {
            _dbManager = dbManager;
        }

        public DscQDataQueryResult Execute(DscQDataQuery query)
        {
            IQueryable<DscQData> dscQDatas = null;

            try
            {
                _dbManager.Open($"Data Source={query.DataSource};User Id={query.UserId};Password={query.Password}");

                query.Parameters.ForEach(p =>
                {
                    _dbManager.AddParameter(p.FieldCode, p.Value);
                });

                IDbDataParameter pResult = _dbManager.AddParameter("result", null, ParameterDirection.ReturnValue, short.MaxValue);
                _dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "DSC$QUERY_UTILS.query_run");

                var v = pResult.Value;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Join("\n", ex.Messages()));
            }
            finally
            {
                _dbManager.Close();
            }

            return new DscQDataQueryResult() { Items = dscQDatas };
        }
    }
}