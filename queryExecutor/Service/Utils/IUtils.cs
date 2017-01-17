using System.Collections.Generic;
using System.ServiceModel;
using queryExecutor.Domain.DscQColumn;
using queryExecutor.Domain.DscQColumn.Query;
using queryExecutor.Domain.DscQueryData;
using queryExecutor.Domain.DscQueryData.Query;
using queryExecutor.Domain.DscQueryParameter;
using queryExecutor.Domain.DscQueryParameter.Query;

namespace queryExecutor.Service.Utils
{
    [ServiceContract(Namespace = "http://web.aquilon.ru")]
    public interface IUtils
    {
        [OperationContract]
        IEnumerable<DscQColumn> GetColumns(DscQColumnQuery query);

        [OperationContract]
        IEnumerable<DscQParameter> GetParameters(DscQParameterQuery query);

        [OperationContract]
        IEnumerable<DscQData> GetResults(DscQDataQuery query);
    }
}
