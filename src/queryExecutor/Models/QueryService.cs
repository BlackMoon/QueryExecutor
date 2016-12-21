using System.Collections.Generic;

namespace queryExecutor.Models
{
    public class QueryService
    {
        public IEnumerable<DscQuery> Queries => new [] { new DscQuery() };
    }
}
