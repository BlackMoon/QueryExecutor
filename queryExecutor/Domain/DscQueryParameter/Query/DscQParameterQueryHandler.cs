using System;
using System.Collections.Generic;
using System.Linq;
using queryExecutor.CQRS.Query;
using queryExecutor.DbManager;
using queryExecutor.Interception;
using queryExecutor.Interception.Attribute;

namespace queryExecutor.Domain.DscQueryParameter.Query
{
    [InterceptedObject(InterceptorType = typeof(CacheInterceptor), ServiceInterfaceType = typeof(IQueryHandler<DscQParameterQuery, DscQParameterQueryResult>))]
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
                string sql = @"SELECT p.no, p.name, p.field_Code fieldCode, ff.value_type_no valueType FROM DSC$QUERY_PARAMETERS p
                               JOIN TDF$FLEX_FIELDS ff ON ff.no = p.field_no 
                               WHERE p.query_no = dsc$utils.query_find(:p0) AND p.is_hidden = 'F'";

                _dbManager.Open($"Data Source={query.DataSource};User Id={query.UserId};Password={query.Password}");

                dscQParameters = _dbManager
                    .DbContext
                    .Set<DscQParameter>()
                    .SqlQuery(sql, query.Path)
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
