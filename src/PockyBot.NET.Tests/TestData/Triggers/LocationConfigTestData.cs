using System;
using System.Collections.Generic;
using System.Text;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Tests.TestData.Triggers
{
    public static class LocationConfigTestData
    {
        public static IEnumerable<object[]> RespondTestData()
        {
            // no command
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationconfig "
                        }
                    }
                },
                new PockyUser
                {
                    UserId = "testUserId",
                    Username = "Test User",
                    Roles = new List<Role>()
                },
                Array.Empty<string>(),
                new Message
                {
                    Text = "Please specify a command. Possible values are get, set and delete."
                }
            };

            // invalid command
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationconfig blah"
                        }
                    }
                },
                new PockyUser
                {
                    UserId = "testUserId",
                    Username = "Test User",
                    Roles = new List<Role>()
                },
                Array.Empty<string>(),
                new Message
                {
                    Text = "Unknown command. Possible values are get, set and delete."
                }
            };

            // get command, no locations
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationconfig get"
                        }
                    }
                },
                new PockyUser
                {
                    UserId = "testUserId",
                    Username = "Test User",
                    Roles = new List<Role>()
                },
                Array.Empty<string>(),
                new Message
                {
                    Text = "No locations set."
                }
            };

            // get command, some locations set
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationconfig get"
                        }
                    }
                },
                new PockyUser
                {
                    UserId = "testUserId",
                    Username = "Test User",
                    Roles = new List<Role>()
                },
                new []
                {
                    "Location1",
                    "Location2",
                    "Location3"
                },
                new Message
                {
                    Text = "Here are the current locations:\n* Location1\n* Location2\n* Location3"
                }
            };

            // set command, no permission
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationconfig set Location1"
                        }
                    }
                },
                new PockyUser
                {
                    UserId = "testUserId",
                    Username = "Test User",
                    Roles = new List<Role>()
                },
                Array.Empty<string>(),
                new Message
                {
                    Text = "Permission denied. You may only use the 'get' command."
                }
            };

            // set command, no location name
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationconfig set "
                        }
                    }
                },
                new PockyUser
                {
                    UserId = "testUserId",
                    Username = "Test User",
                    Roles = new List<Role> { new Role { UserRole = Roles.Admin } }
                },
                Array.Empty<string>(),
                new Message
                {
                    Text = "You must specify a location name to set."
                }
            };

            // set command, location already set
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationconfig set Location1"
                        }
                    }
                },
                new PockyUser
                {
                    UserId = "testUserId",
                    Username = "Test User",
                    Roles = new List<Role> { new Role { UserRole = Roles.Config } }
                },
                new List<string> { "Location1" },
                new Message
                {
                    Text = "Location value Location1 has already been set."
                }
            };

            // delete command, no permission
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationconfig delete Location1"
                        }
                    }
                },
                new PockyUser
                {
                    UserId = "testUserId",
                    Username = "Test User",
                    Roles = new List<Role>()
                },
                Array.Empty<string>(),
                new Message
                {
                    Text = "Permission denied. You may only use the 'get' command."
                }
            };

            // delete command, no location name
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationconfig delete "
                        }
                    }
                },
                new PockyUser
                {
                    UserId = "testUserId",
                    Username = "Test User",
                    Roles = new List<Role> { new Role { UserRole = Roles.Admin } }
                },
                Array.Empty<string>(),
                new Message
                {
                    Text = "You must specify a location name to delete."
                }
            };

            // delete command, location already deleted
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationconfig delete Location1"
                        }
                    }
                },
                new PockyUser
                {
                    UserId = "testUserId",
                    Username = "Test User",
                    Roles = new List<Role> { new Role { UserRole = Roles.Config } }
                },
                new List<string> { "Location2" },
                new Message
                {
                    Text = "Location value Location1 does not exist."
                }
            };
        }

        public static IEnumerable<object[]> SetLocationTestData()
        {
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationconfig set Location1"
                        }
                    }
                },
                new PockyUser
                {
                    UserId = "testUserId",
                    Username = "Test User",
                    Roles = new List<Role> { new Role { UserRole = Roles.Config } }
                },
                new List<string> { "Location2" },
                new Message
                {
                    Text = "Location has been set."
                },
                "Location1"
            };
        }
        public static IEnumerable<object[]> DeleteLocationTestData()
        {
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationconfig delete Location1"
                        }
                    }
                },
                new PockyUser
                {
                    UserId = "testUserId",
                    Username = "Test User",
                    Roles = new List<Role> { new Role { UserRole = Roles.Config } }
                },
                new List<string> { "Location1" },
                new Message
                {
                    Text = "Location has been deleted."
                },
                "Location1"
            };
        }
    }
}
