using System.Collections.Generic;
using queryExecutor.CQRS.Query;

namespace queryExecutor.Domain.DscQColumn.Query
{
    public class DscQColumnQueryResult : IQueryResult
    {
        public IEnumerable<DscQColumn> Items { get; set; }
    }
}