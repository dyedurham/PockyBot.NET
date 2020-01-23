using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PockyBot.NET.Persistence.Models
{
    [Table("pockyusers")]
    internal class PockyUser
    {
        [Key]
        [Column("userid")]
        public string UserId { get; set; }
        [Column("username")]
        public string Username { get; set; }

        public List<Role> Roles { get; set; }
        [InverseProperty("sender")]
        public List<Peg> PegsGiven { get; set; }
        [InverseProperty("receiver")]
        public List<Peg> PegsReceived { get; set; }
        public UserLocation Location { get; set; }

        public bool HasRole(string role) =>
            Roles?.Any(x => string.Equals(role, x.UserRole, StringComparison.OrdinalIgnoreCase)) ?? false;
    }
}
