using System.Collections.Generic;

namespace queryExecutor.Domain.DscQueryData
{
    public class DscQData : KeyObject
    {
        public Dictionary<string, object> DynamicProperties { get; }

        public DscQData()
        {
            DynamicProperties = new Dictionary<string, object>();
        }
    }
}