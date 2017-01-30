using System.Collections.Generic;
using System.Runtime.Serialization;
using queryExecutor.CQRS.Query;
using queryExecutor.Domain.DscQueryParameter;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace queryExecutor.Domain.DscQueryData.Query
{
    [DataContract]
    public class DscQDataQuery : IQuery
    {
        [DataMember]
        public string DataSource { get; set; }

        public string Password { get; set; }

        public string UserId { get; set; }

        [DataMember]
        public string Path { get; set; }
        
        public List<DscQParameter> Parameters { get; set; }

        [DataMember]
        public Dictionary<string, object> DynamicParameters { get; set; }

        public override int GetHashCode()
        {
            int sum = 0;

            if (Parameters != null)
            {
                unchecked
                {
                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (DscQParameter p in Parameters)
                    {
                        sum += p.GetHashCode();
                    }
                }
            }

            return $"{DataSource}{UserId}{Password}{Path}".GetHashCode() + sum;
        }
    }
}