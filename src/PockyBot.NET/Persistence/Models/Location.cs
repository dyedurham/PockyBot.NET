using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PockyBot.NET.Persistence.Models
{
    [Table("locations")]
    internal class Location
    {
        [Key]
        [Column("name")]
        public string Name { get; set; }
    }
}
