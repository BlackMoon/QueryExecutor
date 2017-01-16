using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using queryExecutor.CQRS.Query;
using queryExecutor.DbManager;
using queryExecutor.DbManager.Oracle.Udt.TVariantNamed;
using queryExecutor.DbManager.Oracle.Udt.TVariantNamedList;
using queryExecutor.Interception;
using queryExecutor.Interception.Attribute;

namespace queryExecutor.Domain.DscQueryData.Query
{
    //[InterceptedObject(InterceptorType = typeof(CacheInterceptor), ServiceInterfaceType = typeof(IQueryHandler<DscQDataQuery, DscQDataQueryResult>))]
    public class DscQDataQueryHandler : IQueryHandler<DscQDataQuery, DscQDataQueryResult>
    {
        private const string KeyField = "NO";

        private readonly IDbManager _dbManager;

        public DscQDataQueryHandler(IDbManager dbManager)
        {
            _dbManager = dbManager;
        }
        
        public DscQDataQueryResult Execute(DscQDataQuery query)
        {
            IEnumerable<DscQData> dscQDatas = Enumerable.Empty<DscQData>();

            try
            {
                _dbManager.Open($"Data Source={query.DataSource};User Id={query.UserId};Password={query.Password}");

                IEnumerable<TVariantNamed> variantNameds = query.Parameters.Select(p =>
                {
                    TVariantNamed variantNamed = new TVariantNamed() {Name = p.FieldCode};

                    switch (p.ValueType)
                    {
                        case EValueType.DATE:

                            variantNamed.ValueDate = Convert.ToDateTime(p.Value);
                            break;

                        case EValueType.NUMBER:

                            variantNamed.ValueNumber = Convert.ToDecimal(p.Value);
                            break;

                        case EValueType.OBJECT:

                            variantNamed.ValueObject = Convert.ToInt64(p.Value);
                            break;

                        default:

                            variantNamed.ValueVarchar2 = Convert.ToString(p.Value);
                            break;
                    }
                    return variantNamed;
                });

                TVariantNamedList variantNamedList = TVariantNamedList.Create((OracleConnection) _dbManager.DbConnection, variantNameds.ToArray());

                OracleParameter pParams = new OracleParameter("pParams", OracleDbType.Object, ParameterDirection.Input)
                {
                    UdtTypeName = "PUBLIC.T_VARIANT_NAMED_LIST",
                    Value = variantNamedList
                };
                _dbManager.AddParameter("pQuery_Path", query.Path, ParameterDirection.Input);
                _dbManager.AddParameter(pParams);

                OracleParameter pCursor = (OracleParameter) _dbManager.AddParameter("pCursor", null, ParameterDirection.Output);
                pCursor.OracleDbType = OracleDbType.RefCursor;

                IDbDataParameter pResult = _dbManager.AddParameter("result", null, ParameterDirection.ReturnValue, short.MaxValue);
                _dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "DSC$UTILS.query_run");

                long res = Convert.ToInt64(pResult.Value);
                if (res == 0)
                {
                    DataTable dt = new DataTable();
                    OracleDataAdapter adapter = new OracleDataAdapter();
                    adapter.Fill(dt, (OracleRefCursor) pCursor.Value);

                    // проверка на наличие ключевого поля
                    bool keyFieldExists = dt.Columns.Contains(KeyField);

                    IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>()
                        .Select(c => c.ColumnName)
                        .Where(c => !c.Equals(KeyField, StringComparison.OrdinalIgnoreCase));

                    dscQDatas = dt.AsEnumerable()
                        .Select((r, i) =>
                        {
                            object v;
                            DscQData dscQData = new DscQData() {No = i + 1};

                            if (keyFieldExists)
                            {
                                v = r[KeyField];
                                if (v != DBNull.Value)
                                    dscQData.No = Convert.ToInt64(v);
                            }

                            foreach (string column in columnNames)
                            {
                                v = r[column];
                                dscQData.DynamicProperties[column] = (v != DBNull.Value) ? v : null;
                            }

                            return dscQData;
                        });
                }
            }
            finally
            {
                _dbManager.Dispose();
            }

            return new DscQDataQueryResult() { Items = dscQDatas.AsQueryable() };
        }
    }
}