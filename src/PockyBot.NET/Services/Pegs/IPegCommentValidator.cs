namespace PockyBot.NET.Services.Pegs
{
    internal interface IPegCommentValidator
    {
        bool IsPegValid(string comment, int? requireKeywords, string[] keywords, string[] penaltyKeywords);
    }
}
