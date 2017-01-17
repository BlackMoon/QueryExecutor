using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using queryExecutor.CQRS.Query;
using queryExecutor.Domain.DscQColumn;
using queryExecutor.Domain.DscQColumn.Query;
using queryExecutor.Domain.DscQueryData;
using queryExecutor.Domain.DscQueryData.Query;
using queryExecutor.Domain.DscQueryParameter;
using queryExecutor.Domain.DscQueryParameter.Query;

namespace queryExecutor.Service.Utils
{
    [ServiceBehavior(Namespace = "http://web.aquilon.ru")]
    public class Utils : IUtils
    {
        private readonly IQueryDispatcher _queryDispatcher;
        public Utils(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        public IEnumerable<DscQColumn> GetColumns(DscQColumnQuery query)
        {
            query.UserId = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            DscQColumnQueryResult result = _queryDispatcher.Dispatch<DscQColumnQuery, DscQColumnQueryResult>(query);
            return result.Items;
        }

        public IEnumerable<DscQParameter> GetParameters(DscQParameterQuery query)
        {
            DscQParameterQueryResult result = _queryDispatcher.Dispatch<DscQParameterQuery, DscQParameterQueryResult>(query);
            return result.Items;
        }

        public IEnumerable<DscQData> GetResults(DscQDataQuery query)
        {
            // DscQParameters (из кеша)
            DscQParameterQuery parameterQuery = new DscQParameterQuery()
            {
                Path = query.Path,
                DataSource = query.DataSource,
                //UserId = user,
                //Password = pswd
            };

            DscQParameterQueryResult parameterResult = _queryDispatcher.Dispatch<DscQParameterQuery, DscQParameterQueryResult>(parameterQuery);

                //UserId = user,
                //Password = pswd,
            query.Parameters = query.Parameters2
                .Select((p, i) => new DscQParameter()
                {
                    // заполнение ключа [No] для вычисления хеша
                    No = i + 1,
                    FieldCode = p.FieldCode,
                    Value = p.Value,
                    // valueType из списка DscQParameter's
                    ValueType = parameterResult
                        .Items
                        .FirstOrDefault(item => item.FieldCode.Equals(p.FieldCode, StringComparison.OrdinalIgnoreCase))
                        ?.ValueType
                }).ToList();
                    

            DscQDataQueryResult result = _queryDispatcher.Dispatch<DscQDataQuery, DscQDataQueryResult>(query);
            return result.Items;
        }
    }
}
