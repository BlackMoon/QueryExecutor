using System.Linq;
using queryExecutor.CQRS.Query;

namespace queryExecutor.Domain.DscQueryData.Query
{
    public class DscQDataQueryResult : IQueryResult
    {
        public IQueryable<DscQData> Items { get; set; }
    }
}