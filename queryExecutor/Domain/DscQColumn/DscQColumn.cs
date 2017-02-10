using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace queryExecutor.Domain.DscQColumn
{
    /// <summary>
    /// dsc$query_columns
    /// </summary>
    [DataContract]
    [Table("DSC$QUERY_COLUMNS")]
    public class DscQColumn : KeyObject
    {
        [Column("QUERY_NO")]
        public long QueryNo { get; set; }

        [Column("ORDER_NO")]
        public int OrderNo { get; set; }

        [Column("NAME")]
        [DataMember]
        public string Name { get; set; }

        [Column("FIELD_CODE")]
        [DataMember]
        public string FieldCode { get; set; }

        [Column("PRECISION")]
        [DataMember]
        public int? Precision { get; set; }

        [Column("SCALE")]
        [DataMember]
        public int? Scale { get; set; }

        [Column("VALUE_TYPE_NO")]
        [DataMember]
        public EValueType? ValueType { get; set; }
       
    }
}