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
        public string Sender { get; set; }
        [Column("receiver")]
        public string Receiver { get; set; }
        [Column("comment")]
        public string Comment { get; set; }

        [ForeignKey("sender")]
        public PockyUser SenderUser { get; set; }
        [ForeignKey("receiver")]
        public PockyUser ReceiverUser { get; set; }
    }
}
