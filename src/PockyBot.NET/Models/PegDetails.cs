using System.Collections.Generic;

namespace PockyBot.NET.Models
{
    internal class PegDetails
    {
        public string SenderName { get; set; }
        public int Weight { get; set; }
        public string Comment { get; set; }
        public List<string> Keywords { get; set; }
        public string SenderLocation { get; set; }
    }
}
