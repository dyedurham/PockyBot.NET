using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
        [InverseProperty("Sender")]
        public List<Peg> PegsGiven { get; set; }
        [InverseProperty("Receiver")]
        public List<Peg> PegsReceived { get; set; }

        public bool HasRole(string role) =>
            Roles.Any(x => string.Equals(role, x.UserRole, StringComparison.OrdinalIgnoreCase));
    }
}
