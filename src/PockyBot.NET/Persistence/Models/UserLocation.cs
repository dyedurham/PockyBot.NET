using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PockyBot.NET.Persistence.Models
{
    [Table("user_locations")]
    internal class UserLocation
    {
        [Key]
        [Column("userid")]
        public string UserId { get; set; }
        [Column("location")]
        public string Location { get; set; }

        public PockyUser User { get; set; }
    }
}
