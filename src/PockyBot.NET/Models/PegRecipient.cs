using System.Collections.Generic;

namespace PockyBot.NET.Models
{
    internal class PegRecipient
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Location { get; set; }
        public int TotalPoints { get; set; }
        public int PegCount { get; set; }
        public int PenaltyCount { get; set; }
        public int PegsGivenCount { get; set; }
        public List<PegDetails> Pegs { get; set; }
        public List<PegDetails> Penalties { get; set; }
    }
}
