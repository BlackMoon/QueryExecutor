using queryExecutor.CQRS.Query;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace queryExecutor.Domain.DscQueryParameter.Query
{
    public class DscQParameterQuery : IQuery
    {
        public string DataSource { get; set; }

        public string Password { get; set; }

        public string UserId { get; set; }

        public string Path { get; set; }

        public override int GetHashCode()
        {
            return $"{DataSource}{UserId}{Password}{Path}".GetHashCode();
        }
    }
}
