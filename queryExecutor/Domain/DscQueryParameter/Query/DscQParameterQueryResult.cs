using System.Collections.Generic;
using System.Linq;
using queryExecutor.CQRS.Query;

namespace queryExecutor.Domain.DscQueryParameter.Query
{
    public class DscQParameterQueryResult : IQueryResult
    {
        public IEnumerable<DscQParameter> Items { get; set; }
    }
}
