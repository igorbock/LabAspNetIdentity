using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShieldJWTLib.Models
{
    [Table("log_delete_iteration")]
    public class LogDeleteIteration
    {
        [Key]
        public int Id { get; set; }

        public DateTime LastIterationDate { get; set; }
        public int LogCount { get; set; }
    }
}
