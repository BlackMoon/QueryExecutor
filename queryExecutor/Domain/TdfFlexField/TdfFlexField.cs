using System.ComponentModel.DataAnnotations.Schema;

namespace queryExecutor.Domain.TdfFlexField
{
    /// <summary>
    /// tdf$flex_fields
    /// </summary>
    [Table("TDF$FLEX_FIELDS")]
    public class TdfFlexField : KeyObject
    {
        [Column("FORMAT_MASK")]
        public string FormatMask { get; set; }

        [Column("PRECISION")]
        public int? Precision { get; set; }

        [Column("SCALE")]
        public int? Scale { get; set; }

        [Column("VALUE_TYPE_NO")]
        public EValueType ValueType { get; set; }
    }
}