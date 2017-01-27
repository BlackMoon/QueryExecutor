using System.Linq;
using queryExecutor.CQRS.Query;
using queryExecutor.DbManager;
using queryExecutor.DbManager.Oracle;

namespace queryExecutor.Domain.DscQColumn.Query
{
#if !DEBUG
    [Interception.Attribute.InterceptedObject(InterceptorType = typeof(Interception.CacheInterceptor), ServiceInterfaceType = typeof(IQueryHandler<DscQColumnQuery, DscQColumnQueryResult>))]
#endif
    public class DscQColumnQueryHandler : IQueryHandler<DscQColumnQuery, DscQColumnQueryResult>
    {
        private readonly IDbManager _dbManager;

        public DscQColumnQueryHandler(IDbManager dbManager)
        {
            _dbManager = dbManager;
        }

        public DscQColumnQueryResult Execute(DscQColumnQuery query)
        {
            IQueryable<DscQColumn> dscQColumns;
            try
            {
                _dbManager.Open($"Data Source={query.DataSource};User Id={query.UserId};Password={query.Password}");

                OracleDbContext ctx = _dbManager.DbContext.Cast<OracleDbContext>();

                dscQColumns = ctx.DscQColumns
                    .Where(c => c.QueryNo == ctx.DscUtils_QueryFind(query.Path))
                    .OrderBy(c => c.OrderNo);
            }
            finally
            {
                _dbManager.Dispose();
            }

            return new DscQColumnQueryResult() { Items = dscQColumns };
        }
    }
}