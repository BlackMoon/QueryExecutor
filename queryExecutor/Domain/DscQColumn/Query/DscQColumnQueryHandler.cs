using System.Linq;
using queryExecutor.CQRS.Query;
using queryExecutor.DbManager;

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
                string sql = @"SELECT c.no, c.name, c.field_Code fieldCode, c.value_type_no valueType FROM DSC$QUERY_COLUMNS c
                               WHERE c.query_no = dsc$utils.query_find(:p0)
                               ORDER BY c.order_no";

                _dbManager.Open($"Data Source={query.DataSource};User Id={query.UserId};Password={query.Password}");

                dscQColumns = _dbManager
                    .DbContext
                    .Set<DscQColumn>()
                    .SqlQuery(sql, query.Path)
                    .AsQueryable();
            }
            finally
            {
                _dbManager.Dispose();
            }

            return new DscQColumnQueryResult() { Items = dscQColumns };
        }
    }
}