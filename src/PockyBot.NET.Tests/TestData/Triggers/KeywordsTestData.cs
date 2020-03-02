using System.Collections.Generic;
using GlobalX.ChatBots.Core.Messages;

namespace PockyBot.NET.Tests.TestData.Triggers
{
    public static class KeywordsTestData
    {
        public static IEnumerable<object[]> RespondTestData()
        {
            yield return new object[]
            {
                new List<string>(),
                new List<string>(),
                new Message
                {
                    Text = "No keywords set.\n\nNo penalty keywords set."
                }
            };

            yield return new object[]
            {
                new List<string> { "brave", "real" },
                new List<string>(),
                new Message
                {
                    Text = "## Here is the list of possible keywords to include in your message\n\n* brave\n* real\n\nNo penalty keywords set."
                }
            };

            yield return new object[]
            {
                new List<string>(),
                new List<string> { "shame", "superShame" },
                new Message
                {
                    Text = "No keywords set.\n\n## Here is the list of keywords that can be used to apply a penalty to the sender\n\nPenalty keywords do not count against the peg limit, and are *not* applied to messages that also include standard keywords.\n\n* shame\n* superShame"
                }
            };

            yield return new object[]
            {
                new List<string> { "brave", "real" },
                new List<string> { "shame", "superShame" },
                new Message
                {
                    Text = "## Here is the list of possible keywords to include in your message\n\n* brave\n* real\n\n## Here is the list of keywords that can be used to apply a penalty to the sender\n\nPenalty keywords do not count against the peg limit, and are *not* applied to messages that also include standard keywords.\n\n* shame\n* superShame"
                }
            };
        }
    }
}
