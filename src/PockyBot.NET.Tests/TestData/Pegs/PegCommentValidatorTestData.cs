using System.Collections.Generic;

namespace PockyBot.NET.Tests.TestData.Pegs
{
    public static class PegCommentValidatorTestData
    {
        public static IEnumerable<object[]> IsPegValid()
        {
            yield return new object[]
            {
                "for doing a cool keyword2 thing",
                1, // require keywords
                new[] { "keyword1", "keyword2", "keyword3" },
                new[] { "penaltyKeyword" },
                true
            };

            // no keywords required
            yield return new object[]
            {
                "for doing a cool thing",
                0, // require keywords
                new[] { "keyword1", "keyword2", "keyword3" },
                new[] { "penaltyKeyword" },
                true
            };

            // both penalty and non penalty keyword
            yield return new object[]
            {
                "for doing a cool keyword2 thing penaltyKeyword",
                1, // require keywords
                new[] { "keyword1", "keyword2", "keyword3" },
                new[] { "penaltyKeyword" },
                true
            };

            // penalty
            yield return new object[]
            {
                "for doing a cool penaltyKeyword thing",
                1, // require keywords
                new[] { "keyword1", "keyword2", "keyword3" },
                new[] { "penaltyKeyword" },
                false
            };

            // penalty, keywords not required
            yield return new object[]
            {
                "for doing a cool penaltyKeyword thing",
                0, // require keywords
                new[] { "keyword1", "keyword2", "keyword3" },
                new[] { "penaltyKeyword" },
                false
            };
        }
    }
}
