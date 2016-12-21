using System.Collections.Generic;

namespace queryExecutor.Models
{
    interface IQueryService
    {
        IEnumerable<DscQuery> Queries { get; }
    }
}
