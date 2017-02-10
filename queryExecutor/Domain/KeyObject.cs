using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace queryExecutor.Domain
{
    [DataContract]
    public abstract class KeyObject
    {
        [Column("NO")]
        [DataMember]
        [Key]
        public long No { get; set; }

        public override int GetHashCode()
        {
            return (int)No;
        }
    }
}