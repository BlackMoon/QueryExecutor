using System.ComponentModel.DataAnnotations;

namespace queryExecutor.Models
{
    public class DscQuery
    {
        [Key]
        public long No { get; set; }

        public string Name { get; set; } = "Query1";
    }
}
