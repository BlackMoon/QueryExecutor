using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable NonReadonlyMemberInGetHashCode

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

        public override int GetHashCode()
        {
            return base.GetHashCode() + Value.GetHashCode();
        }
    }
}
