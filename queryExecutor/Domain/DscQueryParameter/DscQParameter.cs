using System.ComponentModel.DataAnnotations;

namespace queryExecutor.Domain.DscQueryParameter
{
    /// <summary>
    /// dsc$query_parameter
    /// </summary>
    public class DscQParameter
    {
        [Key]
        public long No { get; set; }
        
        public string FieldCode { get; set; }

        public long ValueTypeNo { get; set; }
    }
}
