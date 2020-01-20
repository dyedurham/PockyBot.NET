using System.ComponentModel.DataAnnotations.Schema;

namespace PockyBot.NET.Persistence.Models
{
    [Table("roles")]
    internal class Role
    {
        [Column("userid")]
        public string UserId { get; set; }
        [Column("role")]
        public string UserRole { get; set; }

        public PockyUser User { get; set; }
    }
}
