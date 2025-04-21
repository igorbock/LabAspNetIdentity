using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShieldJWTLib.Models
{
    [Table("claim")]
    public class ShieldClaim
    {
        [Key]
        public long Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public string Value { get; set; }

        [ForeignKey(nameof(User))]
        public int IdUser { get; set; }

        public User User { get; set; }
    }
}
