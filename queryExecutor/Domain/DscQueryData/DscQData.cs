using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace queryExecutor.Domain.DscQueryData
{
    public class DscQData
    {
        [Key]
        public long No { get; set; }

        public IDictionary<string, object> DynamicProperties { get; set; }
    }
}