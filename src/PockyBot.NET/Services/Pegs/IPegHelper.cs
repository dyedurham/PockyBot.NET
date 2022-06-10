namespace PockyBot.NET.Services.Pegs
{
    internal interface IPegHelper
    {
        bool IsPegValid(string comment, int? requireKeywords, string[] keywords, string[] penaltyKeywords);
        bool IsPenaltyPeg(string comment, string[] penaltyKeywords, string[] validKeywords);
        int GetPegWeighting(string senderLocation, string receiverLocation);
    }
}
