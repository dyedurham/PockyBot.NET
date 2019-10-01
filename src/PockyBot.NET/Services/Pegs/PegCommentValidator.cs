using System;
using System.Linq;

namespace PockyBot.NET.Services.Pegs
{
    internal class PegCommentValidator : IPegCommentValidator
    {
        public bool IsPegValid(string comment, int? requireKeywords, string[] keywords, string[] penaltyKeywords)
        {
            if (requireKeywords == 1)
            {
                return keywords.Any(x => comment.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            return !penaltyKeywords.Any(x => comment.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}
