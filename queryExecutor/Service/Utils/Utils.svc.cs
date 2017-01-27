using System;
using System.IdentityModel.Tokens;
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
        private readonly UserNameSecurityToken _securityToken;

        public Utils(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;

            if (OperationContext.Current.IncomingMessageProperties.Security.IncomingSupportingTokens.Count > 0)
                _securityToken = OperationContext.Current.IncomingMessageProperties.Security.IncomingSupportingTokens[0].SecurityToken as UserNameSecurityToken;
        }

        public DscQColumn[] GetColumns(DscQColumnQuery query)
        {
            query.UserId = _securityToken?.UserName;
            query.Password = _securityToken?.Password;

            DscQColumnQueryResult result = _queryDispatcher.Dispatch<DscQColumnQuery, DscQColumnQueryResult>(query);
            return result.Items.ToArray();
        }

        public DscQParameter[] GetParameters(DscQParameterQuery query)
        {
            query.UserId = _securityToken?.UserName;
            query.Password = _securityToken?.Password;

            DscQParameterQueryResult result = _queryDispatcher.Dispatch<DscQParameterQuery, DscQParameterQueryResult>(query);
            return result.Items.ToArray();
        }

        public DscQData[] GetResults(DscQDataQuery query)
        {
            // DscQParameters (из кеша)
            DscQParameterQuery parameterQuery = new DscQParameterQuery()
            {
                Path = query.Path,
                DataSource = query.DataSource,
                UserId = _securityToken.UserName,
                Password = _securityToken.Password
            };

            DscQParameterQueryResult parameterResult = _queryDispatcher.Dispatch<DscQParameterQuery, DscQParameterQueryResult>(parameterQuery);

            query.UserId = _securityToken.UserName;
            query.Password = _securityToken.Password;

            if (query.DynamicParameters != null)
            {
                query.Parameters = query.DynamicParameters
                    .Select((p, i) => new DscQParameter()
                    {
                        // заполнение ключа [No] для вычисления хеша
                        No = i + 1,
                        FieldCode = p.Key,
                        Value = p.Value,
                        // FlexField из списка DscQParameter's
                        FlexField = parameterResult
                            .Items
                            .FirstOrDefault(item => item.FieldCode.Equals(p.Key, StringComparison.OrdinalIgnoreCase))
                            ?.FlexField
                    })
                    .ToList();
            }

            DscQDataQueryResult result = _queryDispatcher.Dispatch<DscQDataQuery, DscQDataQueryResult>(query);
            return result.Items.ToArray();
        }
    }
}
