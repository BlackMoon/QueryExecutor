using System.Diagnostics.CodeAnalysis;
using Oracle.DataAccess.Types;

namespace queryExecutor.DbManager.Oracle.Udt.TVariantNamedList
{
    /// <summary>
    /// Summary description for TVariantNamedListFactory
    /// </summary>
    [OracleCustomTypeMapping("T_VARIANT_NAMED_LIST")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class TVariantNamedListFactory : IOracleCustomTypeFactory
    {
        public IOracleCustomType CreateObject()
        {
            return TVariantNamedList.Create();
        }
    }
}