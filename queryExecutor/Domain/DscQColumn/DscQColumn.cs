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
    }
}