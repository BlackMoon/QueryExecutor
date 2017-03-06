using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace queryExecutor.Domain.DscQueryParameter
{
    /// <summary>
    /// dsc$query_parameter
    /// </summary>
    [DataContract]
    [Table("DSC$QUERY_PARAMETERS")]
    public class DscQParameter : KeyObject
    {
        [Column("FIELD_NO")]
        [DataMember]
        public long FieldNo { get; set; }

        [Column("QUERY_NO")]
        [DataMember]
        public decimal QueryNo { get; set; }

        [Column("IS_HIDDEN")]
        public string IsHidden { get; set; }

        [Column("FIELD_CODE")]
        [DataMember]
        public string FieldCode { get; set; }

        [Column("NAME")]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        [ForeignKey("FieldNo")]
        public TdfFlexField.TdfFlexField FlexField { get; set; }

        [DataMember]
        [NotMapped]
        public object Value { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode() + Value.GetHashCode();
        }
    }
}
