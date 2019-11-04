using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PockyBot.NET.Persistence.Models
{
    [Table("pegs")]
    public class Peg
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("sender")]
        [ForeignKey("sender")]
        public string SenderId { get; set; }
        [Column("receiver")]
        [ForeignKey("receiver")]
        public string ReceiverId { get; set; }
        [Column("comment")]
        public string Comment { get; set; }

        public PockyUser Sender { get; set; }
        public PockyUser Receiver { get; set; }
    }
}
