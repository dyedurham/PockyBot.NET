namespace PockyBot.NET.Services.Pegs
{
    internal interface IPegHelper
    {
        bool IsPegValid(string comment, int? requireKeywords, string[] keywords, string[] penaltyKeywords);
        int GetPegWeighting(string senderLocation, string receiverLocation);
    }
}
