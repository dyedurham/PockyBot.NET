using System;
using System.Collections.Generic;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Tests.TestData.Triggers
{
    public static class RoleConfigTestData
    {
        public static IEnumerable<object[]> GetRoleConfigTestData()
        {
            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig ",
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
                            Text = " roleconfig ",
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
                new List<PockyUser>(),
                new[] { "Please specify a command. Possible values are get, set, and delete." }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig blah",
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
                            Text = " roleconfig blah",
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
                new List<PockyUser>(),
                new[] { "Unknown command. Possible values are get, set, and delete." }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig get",
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
                            Text = " roleconfig get",
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
                new List<PockyUser>(),
                new[] { "No roles have been set." }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig get",
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
                            Text = " roleconfig get",
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
                new List<PockyUser>
                {
                    new PockyUser
                    {
                        UserId = "mrMime",
                        Username = "Mr Mime",
                        Roles = new List<UserRole>
                        {
                            new UserRole { Role = Role.UNMETERED },
                            new UserRole { Role = Role.REMOVEUSER }
                        }
                    },
                    new PockyUser
                    {
                        UserId = "puff",
                        Username = "Jigglypuff",
                        Roles = new List<UserRole>{ new UserRole { Role = Role.ADMIN } }
                    },
                    new PockyUser
                    {
                        UserId = "pika",
                        Username = "Pikachu",
                        Roles = new List<UserRole>{ new UserRole { Role = Role.ADMIN } }
                    }
                },
                new[] { "Here is the current config:", "* Mr Mime: UNMETERED, REMOVEUSER", "* Jigglypuff: ADMIN", "* Pikachu: ADMIN" }
            };
        }

        public static IEnumerable<object[]> UnsuccessfulSetRoleConfigTestData()
        {
            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig set",
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
                            Text = " roleconfig set",
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
                new Person
                {
                    UserId = "puff",
                    Username = "Jigglypuff"
                },
                new PockyUser
                {
                    UserId = "puff",
                    Username = "Jigglypuff",
                    Roles = new List<UserRole> { new UserRole { Role = Role.ADMIN } }
                },
                new Message
                {
                    Text = "You must specify a user and a role to set."
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig set",
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
                            Text = " roleconfig set",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Jigglypuff",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = " INVALID",
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
                new Person
                {
                    UserId = "puff",
                    Username = "Jigglypuff"
                },
                new PockyUser
                {
                    UserId = "puff",
                    Username = "Jigglypuff",
                    Roles = new List<UserRole> { new UserRole { Role = Role.ADMIN } }
                },
                new Message
                {
                    Text = "Please mention a user you want to set a role for."
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig set",
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
                            Text = " roleconfig set",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Jigglypuff",
                            MessageType = MessageType.PersonMention,
                            UserId = "puff"
                        },
                        new MessagePart
                        {
                            Text = " INVALID",
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
                new Person
                {
                    UserId = "puff",
                    Username = "Jigglypuff"
                },
                new PockyUser
                {
                    UserId = "puff",
                    Username = "Jigglypuff",
                    Roles = new List<UserRole> { new UserRole { Role = Role.ADMIN } }
                },
                new Message
                {
                    Text = $"Invalid role INVALID. Valid values are: {string.Join(", ", Enum.GetValues(typeof(Role)))}."
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig set",
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
                            Text = " roleconfig set",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Jigglypuff",
                            MessageType = MessageType.PersonMention,
                            UserId = "puff"
                        },
                        new MessagePart
                        {
                            Text = " Admin",
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
                new Person
                {
                    UserId = "puff",
                    Username = "Jigglypuff"
                },
                new PockyUser
                {
                    UserId = "puff",
                    Username = "Jigglypuff",
                    Roles = new List<UserRole> { new UserRole { Role = Role.ADMIN } }
                },
                new Message
                {
                    Text = "Role ADMIN is already set for user Jigglypuff."
                }
            };
        }

        public static IEnumerable<object[]> UnsuccessfulDeleteRoleConfigTestData()
        {
            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig delete",
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
                            Text = " roleconfig delete",
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
                new Person
                {
                    UserId = "puff",
                    Username = "Jigglypuff"
                },
                new PockyUser
                {
                    UserId = "puff",
                    Username = "Jigglypuff",
                    Roles = new List<UserRole>()
                },
                new Message
                {
                    Text = "You must specify a user and a role to delete."
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig delete",
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
                            Text = " roleconfig delete",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Jigglypuff",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = " INVALID",
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
                new Person
                {
                    UserId = "puff",
                    Username = "Jigglypuff"
                },
                new PockyUser
                {
                    UserId = "puff",
                    Username = "Jigglypuff",
                    Roles = new List<UserRole>()
                },
                new Message
                {
                    Text = "Please mention a user you want to delete a role for."
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig delete",
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
                            Text = " roleconfig delete",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Jigglypuff",
                            MessageType = MessageType.PersonMention,
                            UserId = "puff"
                        },
                        new MessagePart
                        {
                            Text = " INVALID",
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
                new Person
                {
                    UserId = "puff",
                    Username = "Jigglypuff"
                },
                new PockyUser
                {
                    UserId = "puff",
                    Username = "Jigglypuff",
                    Roles = new List<UserRole>()
                },
                new Message
                {
                    Text = $"Invalid role INVALID. Valid values are: {string.Join(", ", Enum.GetValues(typeof(Role)))}."
                }
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig delete",
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
                            Text = " roleconfig delete",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Jigglypuff",
                            MessageType = MessageType.PersonMention,
                            UserId = "puff"
                        },
                        new MessagePart
                        {
                            Text = " Admin",
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
                new Person
                {
                    UserId = "puff",
                    Username = "Jigglypuff"
                },
                new PockyUser
                {
                    UserId = "puff",
                    Username = "Jigglypuff",
                    Roles = new List<UserRole>()
                },
                new Message
                {
                    Text = "Role ADMIN is not set for user Jigglypuff."
                }
            };
        }

        public static IEnumerable<object[]> SuccessfulSetRoleConfigTestData()
        {
            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig set",
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
                            Text = " roleconfig set",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Jigglypuff",
                            MessageType = MessageType.PersonMention,
                            UserId = "puff"
                        },
                        new MessagePart
                        {
                            Text = " Admin",
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
                new Person
                {
                    UserId = "puff",
                    Username = "Jigglypuff"
                },
                new PockyUser
                {
                    UserId = "puff",
                    Username = "Jigglypuff",
                    Roles = new List<UserRole>()
                },
                new Message
                {
                    Text = "Role has been set."
                },
                "puff",
                Role.ADMIN
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig set",
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
                            Text = " roleconfig set",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Jigglypuff",
                            MessageType = MessageType.PersonMention,
                            UserId = "puff"
                        },
                        new MessagePart
                        {
                            Text = " UNMETERED",
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
                new Person
                {
                    UserId = "puff",
                    Username = "Jigglypuff"
                },
                new PockyUser
                {
                    UserId = "puff",
                    Username = "Jigglypuff",
                    Roles = new List<UserRole> { new UserRole { Role = Role.ADMIN } }
                },
                new Message
                {
                    Text = "Role has been set."
                },
                "puff",
                Role.UNMETERED
            };
        }

        public static IEnumerable<object[]> SuccessfulDeleteRoleConfigTestData()
        {
            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig delete",
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
                            Text = " roleconfig delete",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Jigglypuff",
                            MessageType = MessageType.PersonMention,
                            UserId = "puff"
                        },
                        new MessagePart
                        {
                            Text = " Admin",
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
                new Person
                {
                    UserId = "puff",
                    Username = "Jigglypuff"
                },
                new PockyUser
                {
                    UserId = "puff",
                    Username = "Jigglypuff",
                    Roles = new List<UserRole> { new UserRole { Role = Role.ADMIN } }
                },
                new Message
                {
                    Text = "Role has been deleted."
                },
                "puff",
                Role.ADMIN
            };

            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot roleconfig delete",
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
                            Text = " roleconfig delete",
                            MessageType = MessageType.Text
                        },
                        new MessagePart
                        {
                            Text = "Jigglypuff",
                            MessageType = MessageType.PersonMention,
                            UserId = "puff"
                        },
                        new MessagePart
                        {
                            Text = " UNMETERED",
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
                new Person
                {
                    UserId = "puff",
                    Username = "Jigglypuff"
                },
                new PockyUser
                {
                    UserId = "puff",
                    Username = "Jigglypuff",
                    Roles = new List<UserRole> { new UserRole { Role = Role.ADMIN }, new UserRole { Role = Role.UNMETERED} }
                },
                new Message
                {
                    Text = "Role has been deleted."
                },
                "puff",
                Role.UNMETERED
            };
        }
    }
}
