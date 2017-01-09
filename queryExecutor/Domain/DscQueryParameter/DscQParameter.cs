using System.ComponentModel.DataAnnotations.Schema;

namespace queryExecutor.Domain.DscQueryParameter
{
    /// <summary>
    /// dsc$query_parameter
    /// </summary>
    [Table("DSC$QUERY_PARAMETERS")]
    public class DscQParameter : DscQColumn.DscQColumn
    {
        [NotMapped]
        public string Value { get; set; }
    }
}
