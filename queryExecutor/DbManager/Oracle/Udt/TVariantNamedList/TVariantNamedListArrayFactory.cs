using System;
using System.Diagnostics.CodeAnalysis;
using Oracle.DataAccess.Types;

namespace queryExecutor.DbManager.Oracle.Udt.TVariantNamedList
{
    /// <summary>
    /// Summary description for TVariantNamedListArrayFactory
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class TVariantNamedListArrayFactory : IOracleArrayTypeFactory
    {

        public Array CreateArray(int numElems)
        {
            return new queryExecutor.DbManager.Oracle.Udt.TVariantNamedList.TVariantNamedList[numElems];
        }

        public Array CreateStatusArray(int numElems)
        {
            return null;
        }
    }
}