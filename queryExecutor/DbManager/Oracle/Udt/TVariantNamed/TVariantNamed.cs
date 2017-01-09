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
        public decimal ValueNumber { get; set; }

        [OracleObjectMapping("VALUE_OBJECT_NO")]
        public long ValueObject { get; set; }

        [OracleObjectMapping("VALUE_VARCHAR2")]
        public string ValueVarchar2 { get; set; }

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, "NAME", Name);
            OracleUdt.SetValue(con, pUdt, "VALUE_DATE", ValueDate);
            OracleUdt.SetValue(con, pUdt, "VALUE_NUMBER", ValueNumber);
            OracleUdt.SetValue(con, pUdt, "VALUE_OBJECT_NO", ValueObject);
            OracleUdt.SetValue(con, pUdt, "VALUE_VARCHAR2", ValueVarchar2);
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            Name = (string)OracleUdt.GetValue(con, pUdt, "NAME");
            ValueDate = (DateTime)OracleUdt.GetValue(con, pUdt, "VALUE_DATE");
            ValueNumber = (decimal)OracleUdt.GetValue(con, pUdt, "VALUE_NUMBER");
            ValueObject = (long)OracleUdt.GetValue(con, pUdt, "VALUE_OBJECT_NO");
            ValueVarchar2 = (string)OracleUdt.GetValue(con, pUdt, "VALUE_VARCHAR2");
        }

        public bool IsNull => _isNull;

        // TVariantNamed.Null is used to return a NULL TVariantNamed object
        public static TVariantNamed Null => new TVariantNamed {_isNull = true };
    }
}