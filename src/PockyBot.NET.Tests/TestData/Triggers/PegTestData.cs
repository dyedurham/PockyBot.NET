using System.Collections.Generic;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Tests.TestData.Triggers
{
    public static class PegTestData
    {
        public static IEnumerable<object[]> RespondTestData()
        {
            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot peg TestUser1 for reasons",
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
                            Text = "TestUser1",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser1Id"
                        },
                        new MessagePart
                        {
                            Text = " for reasons",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUser2Id",
                        Username = "Test User 2",
                        Type = PersonType.Person
                    }
                },
                null, // error message
                new PockyUser // sender
                {
                    UserId = "testUser2Id",
                    Username = "Test User 2",
                    PegsGiven = new List<Peg>(),
                    Roles = new List<UserRole>()
                },
                new PockyUser // receiver
                {
                    UserId = "testUser1Id",
                    Username = "Test User 1"
                },
                5, // limit
                "for reasons", // comment
                true, // is peg valid
                new Person // receiver
                {
                    UserId = "testUser1Id",
                    Username = "Test User 1"
                },
                null, // response
                true //give peg
            };

            // all pegs used, unmetered user
            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot peg TestUser1 for reasons",
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
                            Text = "TestUser1",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser1Id"
                        },
                        new MessagePart
                        {
                            Text = " for reasons",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUser2Id",
                        Username = "Test User 2",
                        Type = PersonType.Person
                    }
                },
                null, // error message
                new PockyUser // sender
                {
                    UserId = "testUser2Id",
                    Username = "Test User 2",
                    PegsGiven = new List<Peg>
                    {
                        new Peg { Comment = "this is a comment" }
                    },
                    Roles = new List<UserRole>
                    {
                        new UserRole {
                            Role = Role.Unmetered
                        }
                    }
                },
                new PockyUser // receiver
                {
                    UserId = "testUser1Id",
                    Username = "Test User 1"
                },
                1, // limit
                "for reasons", // comment
                true, // is peg valid
                new Person // receiver
                {
                    UserId = "testUser1Id",
                    Username = "Test User 1"
                },
                null, // response
                true //give peg
            };

            // all pegs used, penalty peg
            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot peg TestUser1 for shameful reasons",
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
                            Text = "TestUser1",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser1Id"
                        },
                        new MessagePart
                        {
                            Text = " for reasons",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUser2Id",
                        Username = "Test User 2",
                        Type = PersonType.Person
                    }
                },
                null, // error message
                new PockyUser // sender
                {
                    UserId = "testUser2Id",
                    Username = "Test User 2",
                    PegsGiven = new List<Peg>
                    {
                        new Peg { Comment = "this is a comment" }
                    },
                    Roles = new List<UserRole>()
                },
                new PockyUser // receiver
                {
                    UserId = "testUser1Id",
                    Username = "Test User 1"
                },
                1, // limit
                "for shameful reasons", // comment
                false, // is peg valid
                new Person // receiver
                {
                    UserId = "testUser1Id",
                    Username = "Test User 1"
                },
                null, // response
                true //give peg
            };

            // invalid peg message
            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot peg TestUser1 for shameful reasons",
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
                            Text = "TestUser1",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser1Id"
                        },
                        new MessagePart
                        {
                            Text = " for reasons",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUser2Id",
                        Username = "Test User 2",
                        Type = PersonType.Person
                    }
                },
                "this is an error", // error message
                new PockyUser // sender
                {
                    UserId = "testUser2Id",
                    Username = "Test User 2",
                    PegsGiven = new List<Peg>
                    {
                        new Peg { Comment = "this is a comment" }
                    },
                    Roles = new List<UserRole>()
                },
                new PockyUser // receiver
                {
                    UserId = "testUser1Id",
                    Username = "Test User 1"
                },
                1, // limit
                "for shameful reasons", // comment
                false, // is peg valid
                new Person // receiver
                {
                    UserId = "testUser1Id",
                    Username = "Test User 1"
                },
                new Message
                {
                    Text = "this is an error"
                },
                false //give peg
            };

            // all pegs spent
            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot peg TestUser1 for reasons",
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
                            Text = "TestUser1",
                            MessageType = MessageType.PersonMention,
                            UserId = "testUser1Id"
                        },
                        new MessagePart
                        {
                            Text = " for reasons",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUser2Id",
                        Username = "Test User 2",
                        Type = PersonType.Person
                    }
                },
                null, // error message
                new PockyUser // sender
                {
                    UserId = "testUser2Id",
                    Username = "Test User 2",
                    PegsGiven = new List<Peg>
                    {
                        new Peg { Comment = "this is a comment" }
                    },
                    Roles = new List<UserRole>()
                },
                new PockyUser // receiver
                {
                    UserId = "testUser1Id",
                    Username = "Test User 1"
                },
                1, // limit
                "for reasons", // comment
                true, // is peg valid
                new Person // receiver
                {
                    UserId = "testUser1Id",
                    Username = "Test User 1"
                },
                new Message
                {
                    Text = "Sorry, but you have already spent all of your pegs for this fortnight."
                },
                false //give peg
            };
        }
    }
}
