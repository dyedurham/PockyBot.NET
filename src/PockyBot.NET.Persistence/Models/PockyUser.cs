using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PockyBot.NET.Persistence.Models
{
    [Table("pockyusers")]
    public class PockyUser
    {
        [Key]
        [Column("userid")]
        public string UserId { get; set; }
        [Column("username")]
        public string Username { get; set; }

        public List<Role> Roles { get; set; }
    }
}
