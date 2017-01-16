using System;
using System.Diagnostics.CodeAnalysis;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using queryExecutor.Domain;

namespace queryExecutor.DbManager.Oracle.Udt.TVariantNamed
{
    /// <summary>
    /// Summary description for TVariantNamed
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class TVariantNamed : INullable, IOracleCustomType
    {
        private bool        _isNull;
        private DateTime?   _valueDate;
        private decimal?    _valueNumber;
        private long?       _valueObject;
        private string      _valueVarchar2;

        public bool IsNull => _isNull;

        public EValueType ValueType => (EValueType) ValueTypeNo;

        [OracleObjectMapping("VALUE_TYPE_NO")]
        public long ValueTypeNo { get; private set; }

        [OracleObjectMapping("NAME")]
        public string Name { get; set; }

        [OracleObjectMapping("VALUE_DATE")]
        public DateTime? ValueDate
        {
            get
            {
                return _valueDate;     
            }
            set
            {
                _valueDate = value;
                ValueTypeNo = (long)EValueType.DATE;
            }
        }
        
        [OracleObjectMapping("VALUE_NUMBER")]
        public decimal? ValueNumber
        {
            get
            {
                return _valueNumber;
            }
            set
            {
                _valueNumber = value;
                ValueTypeNo = (long)EValueType.NUMBER;
            }
        }

        [OracleObjectMapping("VALUE_OBJECT_NO")]
        public long? ValueObject {
            get
            {
                return _valueObject;
            }
            set
            {
                _valueObject = value;
                ValueTypeNo = (long)EValueType.OBJECT;
            }
        }

        [OracleObjectMapping("VALUE_VARCHAR2")]
        public string ValueVarchar2
        {
            get
            {
                return _valueVarchar2;
            }
            set
            {
                _valueVarchar2 = value;
                ValueTypeNo = (long)EValueType.VARCHAR;
            }

        }

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, "NAME", Name);
            OracleUdt.SetValue(con, pUdt, "VALUE_TYPE_NO", ValueTypeNo);

            switch ((EValueType)ValueTypeNo)
            {
                case EValueType.DATE:
                    OracleUdt.SetValue(con, pUdt, "VALUE_DATE", ValueDate);
                    break;
                
                case EValueType.NUMBER:
                    OracleUdt.SetValue(con, pUdt, "VALUE_NUMBER", ValueNumber);
                    break;
                
                case EValueType.OBJECT:
                    OracleUdt.SetValue(con, pUdt, "VALUE_OBJECT_NO", ValueObject);
                    break;

                case EValueType.VARCHAR:
                    OracleUdt.SetValue(con, pUdt, "VALUE_VARCHAR2", ValueVarchar2);
                    break;
            }
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            Name = (string)OracleUdt.GetValue(con, pUdt, "NAME");
            ValueTypeNo = (long)OracleUdt.GetValue(con, pUdt, "VALUE_TYPE_NO");

            switch ((EValueType)ValueTypeNo)
            {
                case EValueType.DATE:

                    if (!OracleUdt.IsDBNull(con, pUdt, "VALUE_DATE"))
                        ValueDate = (DateTime)OracleUdt.GetValue(con, pUdt, "VALUE_DATE");

                    break;
                
                case EValueType.NUMBER:

                    if (!OracleUdt.IsDBNull(con, pUdt, "VALUE_NUMBER"))
                        ValueNumber = (decimal)OracleUdt.GetValue(con, pUdt, "VALUE_NUMBER");

                    break;

                case EValueType.OBJECT:

                    if (!OracleUdt.IsDBNull(con, pUdt, "VALUE_OBJECT_NO"))
                        ValueObject = (long)OracleUdt.GetValue(con, pUdt, "VALUE_OBJECT_NO");

                    break;

                case EValueType.VARCHAR:

                    if (!OracleUdt.IsDBNull(con, pUdt, "VALUE_VARCHAR2"))
                        ValueVarchar2 = (string)OracleUdt.GetValue(con, pUdt, "VALUE_VARCHAR2");

                    break;
            }
        }

        // TVariantNamed.Null is used to return a NULL TVariantNamed object
        public static TVariantNamed Null => new TVariantNamed {_isNull = true };
    }
}