using System.ComponentModel.DataAnnotations.Schema;

namespace PockyBot.NET.Persistence.Models
{
    [Table("roles")]
    internal class UserRole
    {
        [Column("userid")]
        public string UserId { get; set; }
        [Column("role")]
        public Role Role { get; set; }

        public PockyUser User { get; set; }
    }
}
