using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShieldJWTLib.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Username { get; set; }

        [StringLength(500)]
        public string Hash { get; set; }

        public bool EmailConfirmed { get; set; }

        [ForeignKey(nameof(Company))]
        public Guid IdCompany { get; set; }

        public virtual Company Company { get; set; }
    }
}
