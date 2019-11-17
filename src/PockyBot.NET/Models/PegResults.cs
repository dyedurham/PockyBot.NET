using System;
using System.Collections.Generic;

namespace PockyBot.NET.Models
{
    internal class PegResults
    {
        public DateTime Date { get; set; }
        public List<PegRecipient> Winners { get; set; }
        public List<PegRecipient> PegRecipients { get; set; }
        public List<PegCategory> Categories { get; set; }
        public List<PegRecipient> Penalties { get; set; }
    }
}
