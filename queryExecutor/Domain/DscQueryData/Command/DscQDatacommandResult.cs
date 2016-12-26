using System.Linq;
using queryExecutor.CQRS.Command;

namespace queryExecutor.Domain.DscQueryData.Command
{
    public class DscQDataCommandResult : ICommandResult
    {
        public IQueryable<DscQData> Items { get; set; }
    }
}