using System.ComponentModel.DataAnnotations.Schema;

namespace PockyBot.NET.Persistence.Models
{
    [Table("stringconfig")]
    internal class StringConfig
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("value")]
        public string Value { get; set; }
    }
}
