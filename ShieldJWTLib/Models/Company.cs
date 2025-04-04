using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShieldJWTLib.Models
{
    [Table("company")]
    public class Company
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(80)]
        public string CompanyName { get; set; }

        [StringLength(14)]
        public string CNPJ { get; set; }
    }
}
