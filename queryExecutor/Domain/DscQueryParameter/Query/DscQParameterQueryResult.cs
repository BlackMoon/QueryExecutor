using System.Linq;
using queryExecutor.CQRS.Query;

namespace queryExecutor.Domain.DscQueryParameter.Query
{
    public class DscQParameterQueryResult : IQueryResult
    {
        public IQueryable<DscQParameter> Items { get; set; }
    }
}
