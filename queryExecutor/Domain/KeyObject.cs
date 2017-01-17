using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace queryExecutor.Domain
{
    public class KeyObject
    {
        [Key]
        [Column("NO")]
        public long No { get; set; }

        public override int GetHashCode()
        {
            return (int)No;
        }
    }
}