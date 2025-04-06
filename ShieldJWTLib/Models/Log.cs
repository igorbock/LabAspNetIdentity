using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShieldJWTLib.Models
{
    [Table("log")]
    public class Log
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey(nameof(Company))]
        public Guid IdCompany { get; set; }

        [StringLength(5)]
        public string Method { get; set; }

        [StringLength(100)]
        public string Endpoint { get; set; }

        public string ReturnType { get; set; }

        public Company Company { get; set; }
    }
}
