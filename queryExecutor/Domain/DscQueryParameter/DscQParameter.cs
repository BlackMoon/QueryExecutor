using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace queryExecutor.Domain.DscQueryParameter
{
    /// <summary>
    /// dsc$query_parameter
    /// </summary>
    [Table("DSC$QUERY_PARAMETERS")]
    public class DscQParameter
    {
        [Key]
        [Column("NO")]
        public long No { get; set; }

        [Column("NAME")]
        public string Name { get; set; }

        [Column("FIELD_CODE")]
        public string FieldCode { get; set; }

        [NotMapped]
        public string Value { get; set; }
    }
}
