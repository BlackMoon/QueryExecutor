using System.Collections.Generic;

namespace queryExecutor.Domain.DscQueryData
{
    public class DscQData : KeyObject
    {
        public IDictionary<string, object> DynamicProperties { get; }

        public DscQData()
        {
            DynamicProperties = new Dictionary<string, object>();
        }
    }
}