using System.Collections.Generic;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;

namespace PockyBot.NET.Tests.TestData.Triggers
{
    public static class UnpegTestData
    {
        public static IEnumerable<object[]> SingleUnpegMessageTestData()
        {
            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot unpeg TestUser1 for reasons",
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
                            Text = " unpeg ",
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
                1,
                new Message
                {
                    MessageParts = new[]
                    {
                        new MessagePart {Text = "It looks like ", MessageType = MessageType.Text},
                        new MessagePart
                            {MessageType = MessageType.PersonMention, UserId = "testUser1Id", Text = "TestUser1"},
                        new MessagePart
                        {
                            Text = " has hidden their pegs too well for me to find them!",
                            MessageType = MessageType.Text
                        }
                    }
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot unpeg abstract sender for reasons",
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
                            Text = " unpeg abstract sender for reasons",
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
                1,
                new Message
                {
                    MessageParts = new[]
                    {
                        new MessagePart
                        {
                            Text = "It looks like abstract sender has hidden their pegs too well for me to find them!",
                            MessageType = MessageType.Text
                        }
                    }
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot unpeg TestUser1 for reasons",
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
                            Text = " unpeg ",
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
                4,
                new Message
                {
                    MessageParts = new[]
                    {
                        new MessagePart {Text = "I'm sorry ", MessageType = MessageType.Text},
                        new MessagePart
                            {MessageType = MessageType.PersonMention, UserId = "testUser2Id", Text = "Test User 2"},
                        new MessagePart
                        {
                            Text = ", I'm afraid I can't do that.",
                            MessageType = MessageType.Text
                        }
                    }
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot unpeg TestUser1 for reasons",
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
                            Text = " unpeg ",
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
                5,
                new Message
                {
                    MessageParts = new[]
                    {
                        new MessagePart
                        {
                            Text = "### HTTP Status Code 418: I'm a teapot.\n\nUnable to brew coffee. Or pegs.",
                            MessageType = MessageType.Text
                        }
                    }
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot unpeg abstract sender because reasons",
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
                            Text = " unpeg abstract sender because reasons",
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
                5,
                new Message
                {
                    MessageParts = new[]
                    {
                        new MessagePart
                        {
                            Text = "### HTTP Status Code 418: I'm a teapot.\n\nUnable to brew coffee. Or pegs.",
                            MessageType = MessageType.Text
                        }
                    }
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot unpeg TestUser1 for reasons",
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
                            Text = " unpeg ",
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
                6,
                new Message
                {
                    MessageParts = new[]
                    {
                        new MessagePart
                        {
                            Text = $@"```
Error: Access Denied user Test User 2 does not have the correct privileges
	at UnPeg (unpeg.js:126)
	at EveryoneButTestUser2 (unpeg.js:4253)
	at ExecuteBadCode (pockybot.js:1467)
	at DecrementPegs (pockybot.js:1535)
```"
                        }
                    }
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot unpeg abstract sender because reasons",
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
                            Text = " unpeg abstract sender because reasons",
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
                6,
                new Message
                {
                    MessageParts = new[]
                    {
                        new MessagePart
                        {
                            Text = $@"```
Error: Access Denied user Test User 2 does not have the correct privileges
	at UnPeg (unpeg.js:126)
	at EveryoneButTestUser2 (unpeg.js:4253)
	at ExecuteBadCode (pockybot.js:1467)
	at DecrementPegs (pockybot.js:1535)
```"
                        }
                    }
                }
            };
        }

        public static IEnumerable<object[]> DelayedUnpegMessageTestData()
        {
            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot unpeg TestUser1 for reasons",
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
                            Text = " unpeg ",
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
                    },
                    RoomId = "testRoomId"
                },
                0,
                new Message
                {
                    MessageParts = new[]
                    {
                        new MessagePart {Text = "Peg removed from ", MessageType = MessageType.Text},
                        new MessagePart
                            {MessageType = MessageType.PersonMention, UserId = "testUser1Id", Text = "TestUser1"},
                        new MessagePart {Text = ".", MessageType = MessageType.Text}
                    }
                },
                new Message
                {
                    Text = "Kidding!",
                    RoomId = "testRoomId"
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot unpeg abstract sender because reasons",
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
                            Text = " unpeg abstract sender because reasons",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUser2Id",
                        Username = "Test User 2",
                        Type = PersonType.Person
                    },
                    RoomId = "testRoomId"
                },
                0,
                new Message
                {
                    MessageParts = new[]
                    {
                        new MessagePart
                        {
                            Text = "Peg removed from abstract sender.",
                            MessageType = MessageType.Text
                        }
                    }
                },
                new Message
                {
                    Text = "Kidding!",
                    RoomId = "testRoomId"
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot unpeg TestUser1 for reasons",
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
                            Text = " unpeg ",
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
                    },
                    RoomId = "testRoomId"
                },
                2,
                new Message
                {
                    MessageParts = new[]
                    {
                        new MessagePart
                            {MessageType = MessageType.PersonMention, UserId = "testUser2Id", Text = "Test User 2"},
                        new MessagePart
                        {
                            Text = "'s peg has been removed...",
                            MessageType = MessageType.Text
                        }
                    }
                },
                new Message
                {
                    MessageParts = new[]
                    {
                        new MessagePart {Text = "But ", MessageType = MessageType.Text},
                        new MessagePart
                            {MessageType = MessageType.PersonMention, UserId = "testUser2Id", Text = "Test User 2"},
                        new MessagePart {Text = " stole it back!", MessageType = MessageType.Text}
                    },
                    RoomId = "testRoomId"
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot unpeg TestUser1 for reasons",
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
                            Text = " unpeg ",
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
                    },
                    RoomId = "testRoomId"
                },
                3,
                new Message
                {
                    MessageParts = new[]
                    {
                        new MessagePart {Text = "Peg given to ", MessageType = MessageType.Text},
                        new MessagePart
                            {MessageType = MessageType.PersonMention, UserId = "testUser1Id", Text = "TestUser1"},
                        new MessagePart
                        {
                            Text = ".",
                            MessageType = MessageType.Text
                        }
                    }
                },
                new Message
                {
                    MessageParts = new[]
                    {
                        new MessagePart {Text = "But ", MessageType = MessageType.Text},
                        new MessagePart
                            {MessageType = MessageType.PersonMention, UserId = "testUser1Id", Text = "TestUser1"},
                        new MessagePart {Text = " didn't want it!", MessageType = MessageType.Text}
                    },
                    RoomId = "testRoomId"
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot unpeg abstract sender reasons",
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
                            Text = " unpeg abstract sender reasons",
                            MessageType = MessageType.Text
                        }
                    },
                    Sender = new Person
                    {
                        UserId = "testUser2Id",
                        Username = "Test User 2",
                        Type = PersonType.Person
                    },
                    RoomId = "testRoomId"
                },
                3,
                new Message
                {
                    MessageParts = new[]
                    {
                        new MessagePart
                        {
                            Text = "Peg given to abstract.",
                            MessageType = MessageType.Text
                        }
                    }
                },
                new Message
                {
                    MessageParts = new[]
                    {
                        new MessagePart
                        {
                            Text = "But abstract didn't want it!",
                            MessageType = MessageType.Text
                        }
                    },
                    RoomId = "testRoomId"
                }
            };
        }
    }
}
