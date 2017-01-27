using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using queryExecutor.CQRS.Query;
using queryExecutor.DbManager;
using queryExecutor.DbManager.Oracle;

namespace queryExecutor.Domain.DscQueryParameter.Query
{
#if !DEBUG
    [Interception.Attribute.InterceptedObject(InterceptorType = typeof(Interception.CacheInterceptor), ServiceInterfaceType = typeof(IQueryHandler<DscQParameterQuery, DscQParameterQueryResult>))]
#endif
    public class DscQParameterQueryHandler : IQueryHandler<DscQParameterQuery, DscQParameterQueryResult>
    {
        private readonly IDbManager _dbManager;
        public DscQParameterQueryHandler(IDbManager dbManager)
        {
            _dbManager = dbManager;
        }

        /// <summary>
        /// Результат выборки (кешируется)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DscQParameterQueryResult Execute(DscQParameterQuery query)
        {
            IEnumerable<DscQParameter> dscQParameters = Enumerable.Empty<DscQParameter>();
            try
            {
                _dbManager.Open($"Data Source={query.DataSource};User Id={query.UserId};Password={query.Password}");

                OracleDbContext ctx = _dbManager.DbContext.Cast<OracleDbContext>();
               
                dscQParameters = ctx.DscQParameters
                    .Where(p => p.QueryNo == ctx.DscUtils_QueryFind(query.Path))
                    .Include(p => p.FlexField)
                    .ToList();
            }
            finally
            {
                _dbManager.Dispose();
            }

            return new DscQParameterQueryResult() { Items = dscQParameters };
        }
    }
}
