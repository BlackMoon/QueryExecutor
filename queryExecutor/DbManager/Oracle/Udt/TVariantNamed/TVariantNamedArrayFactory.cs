using System;
using System.Diagnostics.CodeAnalysis;
using Oracle.DataAccess.Types;

namespace queryExecutor.DbManager.Oracle.Udt.TVariantNamed
{
    /// <summary>
    /// Summary description for TVariantNamedTable
    /// </summary>
    [OracleCustomTypeMapping("T_VARIANT_NAMED_TABLE")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class VariantNamedArrayFactory : IOracleArrayTypeFactory
    {	
        public Array CreateArray(int numElems)
        {
            return new queryExecutor.DbManager.Oracle.Udt.TVariantNamed.TVariantNamed[numElems];
        }

        public Array CreateStatusArray(int numElems)
        {
            return null;
        }
    }
}