using System;
using System.Diagnostics.CodeAnalysis;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace queryExecutor.DbManager.Oracle.Udt.TVariantNamed
{
    /// <summary>
    /// Summary description for TVariantNamed
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class TVariantNamed : INullable, IOracleCustomType
    {
        private bool _isNull;

        [OracleObjectMapping("NAME")]
        public string Name { get; set; }

        [OracleObjectMapping("VALUE_DATE")]
        public DateTime ValueDate { get; set; }

        [OracleObjectMapping("VALUE_NUMBER")]
        public long ValueNumber { get; set; }

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, "NAME", Name);
            OracleUdt.SetValue(con, pUdt, "VALUE_DATE", ValueDate);
            OracleUdt.SetValue(con, pUdt, "VALUE_NUMBER", ValueNumber);
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            Name = (string)OracleUdt.GetValue(con, pUdt, "NAME");
            ValueDate = (DateTime)OracleUdt.GetValue(con, pUdt, "VALUE_DATE");
            ValueNumber = (long)OracleUdt.GetValue(con, pUdt, "VALUE_NUMBER");
        }

        public bool IsNull => _isNull;

        // TVariantNamed.Null is used to return a NULL TVariantNamed object
        public static TVariantNamed Null => new TVariantNamed {_isNull = true };
    }
}