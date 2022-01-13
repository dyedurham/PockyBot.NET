namespace PockyBot.NET.Models
{
    internal class LinkedKeyword
    {
        internal LinkedKeyword(string linkedKeywordConfig)
        {
            var splitString = linkedKeywordConfig.Split(':');
            Keyword = splitString[0];
            LinkedWord = splitString[1];
        }
        public string Keyword { get; set; }
        public string LinkedWord { get; set; }
    }
}
