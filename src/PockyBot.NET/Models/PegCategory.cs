using System.Collections.Generic;

namespace PockyBot.NET.Models
{
    internal class PegCategory
    {
        public string Name { get; set; }
        public List<PegRecipient> Recipients { get; set; }
    }
}
