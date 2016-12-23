using System.Threading;
using System.Threading.Tasks;

namespace queryExecutor.DbManager
{
    public interface IDbManagerAsync
    {
        Task OpenAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}