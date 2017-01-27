using System;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace queryExecutor.Domain.DscQColumn
{
    /// <summary>
    /// dsc$query_columns
    /// </summary>
    [Table("DSC$QUERY_COLUMNS")]
    public class DscQColumn : KeyObject
    {
        [Column("QUERY_NO")]
        public long QueryNo { get; set; }

        [Column("ORDER_NO")]
        public int OrderNo { get; set; }

        [Column("NAME")]
        public string Name { get; set; }

        [Column("FIELD_CODE")]
        public string FieldCode { get; set; }

        [Column("PRECISION")]
        public int? Precision { get; set; }

        [Column("SCALE")]
        public int? Scale { get; set; }

        [Column("VALUE_TYPE_NO")]
        public EValueType? ValueType { get; set; }

        
        public static long QueryFind(string path)
        {
            throw new NotImplementedException();
        }
    }
}