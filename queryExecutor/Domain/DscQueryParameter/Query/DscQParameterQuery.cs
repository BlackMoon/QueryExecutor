using queryExecutor.CQRS.Query;

namespace queryExecutor.Domain.DscQueryParameter.Query
{
    public class DscQParameterQuery : IQuery
    {
        public string DataSource { get; set; }

        public string Password { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
    }
}
