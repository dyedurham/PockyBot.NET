using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PockyBot.NET.Persistence.Models
{
    [Table("generalconfig")]
    public class GeneralConfig
    {
        [Key]
        [Column("name")]
        public string Name { get; set; }
        [Column("value")]
        public int Value { get; set; }
    }
}
