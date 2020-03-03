using System.Collections.Generic;
using System.Linq;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;

namespace PockyBot.NET.Tests.TestData.Triggers
{
    public class UserLocationTestData
    {
        public static IEnumerable<object[]> GetUserTestData()
        {
            yield return new object[]
            {
                CreateSingleMentionMessage("userlocation get me"),
                false,
                new[] { "me" }

            };

            yield return new object[]
            {
                CreateSingleMentionMessage("userlocation get unset"),
                false,
                new[] { "unset" }
            };

            yield return new object[]
            {
                CreateSingleMentionMessage("userlocation get all"),
                true,
                new[] { "all" }
            };
        }

        public static IEnumerable<object[]> SetUserTestData() {

            yield return new object[]
            {
                CreateMultipleMentionMessage("userlocation set here me"),
                true,
                new[] { "here", "me" }
            };

            yield return new object[]
            {
                CreateMultipleMentionMessage("userlocation set here", "Alice"),
                true,
                new[] { "here", "Alice" }
            };

            yield return new object[]
            {
                CreateMultipleMentionMessage("userlocation set here", "Alice", "Bob"),
                false,
                new[] { "here", "Alice", "Bob" }
            };
        }

        public static IEnumerable<object[]> DeleteUserTestData() {

            yield return new object[]
            {
                CreateMultipleMentionMessage("userlocation set here me"),
                true,
                new[] { "here", "me" }
            };

            yield return new object[]
            {
                CreateMultipleMentionMessage("userlocation set here", "Alice"),
                true,
                new[] { "here", "Alice" }
            };

            yield return new object[]
            {
                CreateMultipleMentionMessage("userlocation set here", "Alice", "Bob"),
                false,
                new[] { "here", "Alice", "Bob" }
            };
        }

        public static IEnumerable<object[]> BadCommandTestData()
        {
            yield return new object[]
            {
                CreateSingleMentionMessage("userlocation"),
                "Please specify a command. Possible values are get, set, and delete."
            };

            yield return new object[]
            {
                CreateSingleMentionMessage("userlocation hello"),
                "Unknown command. Possible values are get, set, and delete."
            };
        }

        private static Message CreateSingleMentionMessage(string message)
        {
            return new Message
            {
                Text = $"TestBot {message}",
                MessageParts = new[]
                {
                    new MessagePart
                    {
                        Text = "TestBot",
                        MessageType = MessageType.PersonMention,
                        UserId = "testbotid"
                    },
                    new MessagePart
                    {
                        Text = $" {message}",
                        MessageType = MessageType.Text
                    }
                },
                Sender = new Person
                {
                    UserId = "senderid"
                }
            };
        }

        private static Message CreateMultipleMentionMessage(string message, params string[] userIds)
        {
            var staticDefinedMessageParts = new List<MessagePart>
            {
                new MessagePart
                {
                    Text = "TestBot",
                    MessageType = MessageType.PersonMention,
                    UserId = "testbotid"
                },
                new MessagePart
                {
                    Text = $" {message} ",
                    MessageType = MessageType.Text
                }
            };

            var userMentions = userIds.SelectMany(userid => new MessagePart[]
            {
                new MessagePart
                {
                    Text = userid,
                    MessageType = MessageType.PersonMention,
                    UserId = userid
                },
                new MessagePart
                {
                    Text = " ",
                    MessageType = MessageType.Text
                }
            }).ToList();

            var messageParts = staticDefinedMessageParts.Concat(userMentions);

            return new Message
            {
                Text = $"TestBot {message} {string.Join(' ', userIds)}",
                MessageParts = messageParts.ToArray(),
                Sender = new Person
                {
                    UserId = "senderid"
                }
            };
        }
    }
}
