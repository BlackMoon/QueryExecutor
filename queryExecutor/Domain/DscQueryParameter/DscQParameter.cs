using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace queryExecutor.Domain.DscQueryParameter
{
    /// <summary>
    /// dsc$query_parameter
    /// </summary>
    [Table("DSC$QUERY_PARAMETERS")]
    public class DscQParameter : KeyObject
    {
        [Column("FIELD_NO")]
        public long FieldNo { get; set; }

        [Column("QUERY_NO")]
        public decimal QueryNo { get; set; }

        [Column("IS_HIDDEN")]
        public string IsHidden { get; set; }

        [Column("FIELD_CODE")]
        public string FieldCode { get; set; }

        [Column("NAME")]
        public string Name { get; set; }

       
        [ForeignKey("FieldNo")]
        public TdfFlexField.TdfFlexField FlexField { get; set; }
       
        [NotMapped]
        public object Value { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode() + Value.GetHashCode();
        }
    }
}
