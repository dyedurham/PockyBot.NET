using System.Collections.Generic;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Tests.TestData.Pegs
{
    public static class PegHelperTestData
    {
        public static IEnumerable<object[]> IsPegValidTestData()
        {
            var keywords = new[] { "keyword1", "keyword2", "keyword3" };
            var penaltyKeywords = new[] { "penaltyKeyword" };

            yield return new object[]
            {
                "This is a comment",
                0, // requireKeywords
                keywords,
                penaltyKeywords,
                true
            };

            yield return new object[]
            {
                "This is a comment",
                1, // requireKeywords
                keywords,
                penaltyKeywords,
                false
            };

            yield return new object[]
            {
                "This is a comment keyword1",
                1, // requireKeywords
                keywords,
                penaltyKeywords,
                true
            };

            yield return new object[]
            {
                "This is a comment penaltyKeyword",
                0, // requireKeywords
                keywords,
                penaltyKeywords,
                false
            };

            yield return new object[]
            {
                "This is a comment penaltyKeyword",
                1, // requireKeywords
                keywords,
                penaltyKeywords,
                false
            };

            yield return new object[]
            {
                "This is a comment KeyWord1",
                1, // requireKeywords
                keywords,
                penaltyKeywords,
                true
            };

            yield return new object[]
            {
                "This is a comment keyword2 penaltyKeyword",
                1, // requireKeywords
                keywords,
                penaltyKeywords,
                true
            };
        }

        public static IEnumerable<object[]> GetPegWeightingTestData()
        {
            yield return new object[]
            {
                "Location1",
                "Location1",
                new List<GeneralConfig>(),
                null,
                1
            };

            yield return new object[]
            {
                "Location1",
                "Location2",
                new List<GeneralConfig>(),
                null,
                2
            };

            yield return new object[]
            {
                "Location1",
                "Location2",
                new List<GeneralConfig>(),
                3,
                3
            };

            yield return new object[]
            {
                "Location1",
                "Location2",
                new List<GeneralConfig>
                {
                    new GeneralConfig
                    {
                        Name = "locationWeightLocation1ToLocation2",
                        Value = 3
                    }
                },
                null,
                3
            };

            yield return new object[]
            {
                "Location1",
                "Location2",
                new List<GeneralConfig>
                {
                    new GeneralConfig
                    {
                        Name = "locationWeightLocation2ToLocation1",
                        Value = 3
                    }
                },
                null,
                3
            };

            yield return new object[]
            {
                "Location1",
                null,
                new List<GeneralConfig>(),
                null,
                1
            };

            yield return new object[]
            {
                null,
                "Location1",
                new List<GeneralConfig>(),
                null,
                1
            };
        }
    }
}
