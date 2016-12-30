using queryExecutor.CQRS.Query;

namespace queryExecutor.Domain.DscQColumn.Query
{
    public class DscQColumnQuery : IQuery
    {
        public string DataSource { get; set; }

        public string Password { get; set; }

        public string UserId { get; set; }

        public string Path { get; set; }
    }
}