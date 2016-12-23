using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace queryExecutor.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }

        public DateTimeOffset BirthTime { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}