using System.Collections.Generic;
using System.Linq;
using queryExecutor.CQRS.Query;
using queryExecutor.Domain.DscQueryParameter;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace queryExecutor.Domain.DscQueryData.Query
{
    public class DscQDataQuery : IQuery
    {
        public string DataSource { get; set; }

        public string Password { get; set; }

        public string UserId { get; set; }

        public string Path { get; set; }

        public List<DscQParameter> Parameters { get; set; }

        public override int GetHashCode()
        {
            return $"{DataSource}{UserId}{Password}{Path}".GetHashCode() + Parameters.Sum(p => p.GetHashCode());
        }
    }
}