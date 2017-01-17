using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace queryExecutor.Domain.DscQueryParameter
{
    /// <summary>
    /// dsc$query_parameter
    /// </summary>
    [Table("DSC$QUERY_PARAMETERS")]
    public class DscQParameter : KeyObject
    {
        [Column("NAME")]
        public string Name { get; set; }

        [Column("FIELD_CODE")]
        public string FieldCode { get; set; }

        [NotMapped]
        public string Value { get; set; }

        [Column("VALUE_TYPE_NO")]
        public EValueType? ValueType { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode() + Value.GetHashCode();
        }
    }
}
