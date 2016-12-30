using System.Collections.Generic;
using queryExecutor.CQRS.Query;
using queryExecutor.Domain.DscQueryParameter;

namespace queryExecutor.Domain.DscQueryData.Query
{
    public class DscQDataQuery : IQuery
    {
        public string DataSource { get; set; }

        public string Password { get; set; }

        public string UserId { get; set; }

        public string Code { get; set; }

        public string Path { get; set; }

        public List<DscQParameter> Parameters { get; set; }
    }
}