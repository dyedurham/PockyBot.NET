using System.Collections.Generic;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using PockyBot.NET.Configuration;

namespace PockyBot.NET.Tests.TestData.Pegs
{
    public static class PegRequestValidatorTestData
    {
        public static IEnumerable<object[]> ValidTestData()
        {
            yield return new object[]
            {
                GetBasicSettings(),
                1, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                1, // requireValues
                new Message
                {
                    Text = "TestBot peg Test User 2 for doing some keyword2 things",
                    MessageParts = new[]
                    {
                        new MessagePart
                        {
                            Text = "TestBot",
                            MessageType = MessageType.PersonMention,
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            Text = " peg ",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Test User 2",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser2Id"
                        },
                        new MessagePart
                        {
                            Text = " for doing some keyword2 things",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User",
                        Type = PersonType.Person
                    }
                }
            };

            // no comments required
            yield return new object[]
            {
                GetBasicSettings(),
                0, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                1, // requireValues
                new Message
                {
                    Text = "TestBot peg Test User 2 keyword2",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "TestBot",
                            MessageType = MessageType.PersonMention,
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            Text = " peg ",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Test User 2",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser2Id"
                        },
                        new MessagePart
                        {
                            Text = " keyword2",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User",
                        Type = PersonType.Person
                    }
                }
            };

            // no keywords required
            yield return new object[]
            {
                GetBasicSettings(),
                1, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                0, // requireValues
                new Message
                {
                    Text = "TestBot peg Test User 2 for doing some things",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "TestBot",
                            MessageType = MessageType.PersonMention,
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            Text = " peg ",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Test User 2",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser2Id"
                        },
                        new MessagePart
                        {
                            Text = " for doing some things",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User",
                        Type = PersonType.Person
                    }
                }
            };

            // no comments or keywords required
            yield return new object[]
            {
                GetBasicSettings(),
                0, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                0, // requireValues
                new Message
                {
                    Text = "TestBot peg Test User 2",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "TestBot",
                            MessageType = MessageType.PersonMention,
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            Text = " peg ",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Test User 2",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser2Id"
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User",
                        Type = PersonType.Person
                    }
                }
            };

            // penalty keyword
            yield return new object[]
            {
                GetBasicSettings(),
                1, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                1, // requireValues
                new Message
                {
                    Text = "TestBot peg Test User 2 for doing some penaltyKeyword things",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "TestBot",
                            MessageType = MessageType.PersonMention,
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            Text = " peg ",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Test User 2",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser2Id"
                        },
                        new MessagePart
                        {
                            Text = " for doing some keyword2 things",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User",
                        Type = PersonType.Person
                    }
                }
            };

            // trailing mention
            yield return new object[]
            {
                GetBasicSettings(),
                1, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                1, // requireValues
                new Message
                {
                    Text = "TestBot peg Test User 2 for doing some keyword2 things Test User 3",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "TestBot",
                            MessageType = MessageType.PersonMention,
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            Text = " peg ",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Test User 2",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser2Id"
                        },
                        new MessagePart
                        {
                            Text = " for doing some keyword2 things",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Test User 3",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser3Id"
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User",
                        Type = PersonType.Person
                    }
                }
            };
        }

        public static IEnumerable<object[]> InvalidTestData()
        {
            // not enough parts
            yield return new object[]
            {
                GetBasicSettings(),
                1, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                1, // requireValues
                new Message
                {
                    Text = "TestBot peg ",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "NotTestBot",
                            MessageType = MessageType.PersonMention,
                            UserId = "notTestBotId"
                        },
                        new MessagePart
                        {
                            Text = " peg ",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User",
                        Type = PersonType.Person
                    }
                },
                "I'm sorry, I couldn't understand your peg request. Please use the following format: `@TestBot peg @Person this is the reason for giving you a peg`."
            };

            // second part is a mention
            yield return new object[]
            {
                GetBasicSettings(),
                1, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                1, // requireValues
                new Message
                {
                    Text = "TestBot peg Test User 2 for doing some keyword2 things",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "TestBot",
                            MessageType = MessageType.PersonMention,
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            Text = " peg ",
                            MessageType = MessageType.PersonMention,
                            UserId = "testPegUser"
                        },
                        new MessagePart
                        {
                            Text = "Test User 2",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser2Id"
                        },
                        new MessagePart
                        {
                            Text = " for doing some keyword2 things",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User",
                        Type = PersonType.Person
                    }
                },
                "I'm sorry, I couldn't understand your peg request. Please use the following format: `@TestBot peg @Person this is the reason for giving you a peg`."
            };

            // wrong command
            yield return new object[]
            {
                GetBasicSettings(),
                1, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                1, // requireValues
                new Message
                {
                    Text = "TestBot pegg Test User 2 for doing some keyword2 things",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "TestBot",
                            MessageType = MessageType.PersonMention,
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            Text = " pegg ",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Test User 2",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser2Id"
                        },
                        new MessagePart
                        {
                            Text = " for doing some keyword2 things",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User",
                        Type = PersonType.Person
                    }
                },
                "I'm sorry, I couldn't understand your peg request. Please use the following format: `@TestBot peg @Person this is the reason for giving you a peg`."
            };

            // third part not a mention
            yield return new object[]
            {
                GetBasicSettings(),
                1, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                1, // requireValues
                new Message
                {
                    Text = "TestBot peg Test User 2 for doing some keyword2 things",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "TestBot",
                            MessageType = MessageType.PersonMention,
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            Text = " peg ",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Test User 2",
                            MessageType = MessageType.Text,
                            UserId = "testUser2Id"
                        },
                        new MessagePart
                        {
                            Text = " for doing some keyword2 things",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User",
                        Type = PersonType.Person
                    }
                },
                "I'm sorry, I couldn't understand your peg request. Please use the following format: `@TestBot peg @Person this is the reason for giving you a peg`."
            };

            // comment required, not provided
            yield return new object[]
            {
                GetBasicSettings(),
                1, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                1, // requireValues
                new Message
                {
                    Text = "TestBot peg Test User 2",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "TestBot",
                            MessageType = MessageType.PersonMention,
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            Text = " peg ",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Test User 2",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser2Id"
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User",
                        Type = PersonType.Person
                    }
                },
                "Please provide a comment with your peg."
            };

            // keyword required, not provided
            yield return new object[]
            {
                GetBasicSettings(),
                1, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                1, // requireValues
                new Message
                {
                    Text = "TestBot peg Test User 2 for doing some things",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "TestBot",
                            MessageType = MessageType.PersonMention,
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            Text = " peg ",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Test User 2",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser2Id"
                        },
                        new MessagePart
                        {
                            Text = " for doing some things",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User",
                        Type = PersonType.Person
                    }
                },
                "I'm sorry, you have to include a keyword in your comment. Please include one of the below keywords in your comment:\n\nkeyword1, keyword2, keyword3"
            };
        }

        private static PockyBotSettings GetBasicSettings()
        {
            return new PockyBotSettings
            {
                BotId = "testBotId",
                BotName = "TestBot"
            };
        }
    }
}
