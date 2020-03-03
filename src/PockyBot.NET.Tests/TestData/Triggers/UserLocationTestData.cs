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
                CreateSingleMentionMessage("userlocation set here me"),
                new[] { "here", "me" },
                true
            };

            yield return new object[]
            {
                CreateSingleMentionMessage("userlocation delete me"),
                CreateSimpleTextMessage("delete me"),
                true
            };

            yield return new object[]
            {
                CreateSingleMentionMessage("userlocation get all"),
                CreateSimpleTextMessage("get all"),
                true
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

        private static Message CreateSimpleTextMessage(string message)
        {
            return new Message
            {
                Text = message
            };
        }

        private static Message CreateMultipleMentionMessage(string message, string[] userids)
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

            var userMentions = userids.Select(userid => new MessagePart
            {
                Text = userid,
                MessageType = MessageType.PersonMention,
                UserId = userid
            }).ToList();

            var messageParts = staticDefinedMessageParts.Concat(userMentions);

            return new Message
            {
                Text = $"TestBot {message} {string.Join(' ', userids)}",
                MessageParts = messageParts.ToArray()
            };
        }
    }
}
