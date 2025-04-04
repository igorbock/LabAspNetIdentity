using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShieldJWTLib.Models
{
    [Table("changed_password")]
    public class ChangedPassword
    {
        [Key]
        public int Id { get; set; }

        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(500)]
        public string NewHash { get; set; }

        [StringLength(20)]
        public string ConfimationCode { get; set; }

        public bool Confirmed { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey(nameof(Company))]
        public Guid IdCompany { get; set; }

        public virtual Company Company { get; set; }
    }
}
