using System.Runtime.Serialization;
using queryExecutor.CQRS.Query;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace queryExecutor.Domain.DscQueryParameter.Query
{
    [DataContract]
    public class DscQParameterQuery : IQuery
    {
        [DataMember]
        public string DataSource { get; set; }

        public string Password { get; set; }

        public string UserId { get; set; }

        [DataMember]
        public string Path { get; set; }

        public override int GetHashCode()
        {
            return $"{DataSource}{UserId}{Password}{Path}".GetHashCode();
        }
    }
}
