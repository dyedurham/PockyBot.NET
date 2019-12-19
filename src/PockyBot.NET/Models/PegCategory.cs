using System.Collections.Generic;

namespace PockyBot.NET.Models
{
    internal class PegCategory
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public List<PegRecipient> Recipients { get; set; }
    }
}
