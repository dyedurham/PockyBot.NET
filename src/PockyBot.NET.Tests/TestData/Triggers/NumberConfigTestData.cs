using System.Collections.Generic;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Tests.TestData.Triggers
{
    public static class NumberConfigTestData
    {
        public static IEnumerable<object[]> GetNumberConfigTestData()
        {
            yield return new object[]
            {
                new List<GeneralConfig>(),
                new List<string> { "No number configs have been set." }
            };

            yield return new object[]
            {
                new List<GeneralConfig>
                {
                    new GeneralConfig { Name = "raccoon", Value = 3 },
                    new GeneralConfig { Name = "cat", Value = 2 },
                    new GeneralConfig { Name = "owl", Value = 2 }
                },
                new List<string>
                {
                    "Here is the current config:",
                    "* raccoon: 3",
                    "* cat: 2",
                    "* owl: 2"
                }
            };
        }
    }
}
