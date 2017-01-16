using System.Linq;
using System.ServiceModel;
using queryExecutor.Domain.DscQColumn;

namespace queryExecutor.Service.Utils
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUtils" in both code and config file together.
    [ServiceContract]
    public interface IUtils
    {
        [OperationContract]
        IQueryable<DscQColumn> GetColumns();

        [OperationContract]
        void GetParameters();

        [OperationContract]
        void GetResults();
    }
}
