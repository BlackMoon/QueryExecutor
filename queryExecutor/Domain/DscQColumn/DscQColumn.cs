using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace queryExecutor.Domain.DscQColumn
{
    /// <summary>
    /// dsc$query_columns
    /// </summary>
    [Table("DSC$QUERY_COLUMNS")]
    public class DscQColumn
    {
        [Key]
        [Column("NO")]
        public long No { get; set; }

        [Column("NAME")]
        public string Name { get; set; }

        [Column("FIELD_CODE")]
        public string FieldCode { get; set; }
        
        [Column("VALUE_TYPE_NO")]
        public EValueType? ValueType { get; set; }

        public override int GetHashCode()
        {
            return (int)No;
        }
    }
}