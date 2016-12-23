using System;
using System.Collections.Generic;
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
            IList<DscQParameter> dscQParameters = new List<DscQParameter>();
            try
            {
                _dbManager.Open($"Data Source={query.DataSource};User Id={query.UserId};Password={query.Password}");

               
                
            }
            catch (Exception ex)
            {
                // ignored
            }
            finally
            {
                _dbManager.Close();
            }

            return new DscQParameterQueryResult() { Items = dscQParameters.AsQueryable() };
        }
    }
}
