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
                new List<string>(),
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
                new List<string>(),
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
                new List<string>(),
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
                new List<string>(),
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
                new List<string>(),
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
                new List<string>(),
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

            //including linked keywords
            yield return new object[]
            {
                GetBasicSettings(),
                1, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                new List<string> { "keyword1:link1", "keyword1:link2" },
                1, // requireValues
                new Message
                {
                    Text = "TestBot peg Test User 2 for doing some link1 things",
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
                            Text = " for doing some link1 things",
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
        }

        public static IEnumerable<object[]> ValidFormatTestData()
        {
            yield return new object[]
            {
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

            // extra mentions in text
            yield return new object[]
            {
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
                            Text = " for doing some keyword2 things with ",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "User3",
                            MessageType = MessageType.PersonMention
                        },
                        new MessagePart
                        {
                            Text = " and I to fix things",
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
        }

        public static IEnumerable<object[]> InvalidFormatTestData()
        {
            // not enough parts
            yield return new object[]
            {
                GetBasicSettings(),
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
        }

        public static IEnumerable<object[]> InvalidTestData()
        {
            // comment required, not provided
            yield return new object[]
            {
                GetBasicSettings(),
                1, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                new List<string>(),
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
                new List<string>(),
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
            // attempts to peg self
            yield return new object[]
            {
                GetBasicSettings(),
                1, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                new List<string>(),
                1, // requireValues
                new Message
                {
                    Text = "TestBot peg testUserId for doing some keyword2 things",
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
                            Text = "Test User",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUserId"
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
                "You can't peg yourself."
            };

            // keyword required, invalid linked keyword supplied
            yield return new object[]
            {
                GetBasicSettings(),
                1, // commentsRequired
                new List<string> { "keyword1", "keyword2", "keyword3" },
                new List<string> { "penaltyKeyword" },
                new List<string> { "keyword4:link1" },
                1, // requireValues
                new Message
                {
                    Text = "TestBot peg Test User 2 for doing some link1 things",
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
                            Text = " for doing some link1 things",
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
