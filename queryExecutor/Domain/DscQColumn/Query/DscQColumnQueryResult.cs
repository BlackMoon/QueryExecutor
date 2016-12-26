using System.Linq;
using queryExecutor.CQRS.Query;

namespace queryExecutor.Domain.DscQColumn.Query
{
    public class DscQColumnQueryResult : IQueryResult
    {
        public IQueryable<DscQColumn> Items { get; set; }
    }
}