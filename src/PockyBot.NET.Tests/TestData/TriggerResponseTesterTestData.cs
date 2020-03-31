using System.Collections.Generic;
using System.Linq;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using NSubstitute;
using PockyBot.NET.Configuration;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Services.Triggers;

namespace PockyBot.NET.Tests.TestData
{
    public static class TriggerResponseTesterTestData
    {
        public static IEnumerable<object[]> RoomTestData()
        {
            // no permissions, correct command, no args
            yield return new object[]
            {
                GetBasicSettings(),
                GetPockyUser(new Role[0]),
                new Message
                {
                    Text = "TestBot ping",
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
                            Text = " ping",
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
                GetTrigger("ping", true, false, new Role[0]),
                true
            };

            // no permissions, correct command (different casing), no args
            yield return new object[]
            {
                GetBasicSettings(),
                GetPockyUser(new Role[0]),
                new Message
                {
                    Text = "TestBot Ping",
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
                            Text = " Ping",
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
                GetTrigger("ping", true, false, new Role[0]),
                true
            };

            // no permissions, correct command, args (allowed)
            yield return new object[]
            {
                GetBasicSettings(),
                GetPockyUser(new Role[0]),
                new Message
                {
                    Text = "TestBot ping with a lil extra",
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
                            Text = " ping with a lil extra",
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
                GetTrigger("ping", true, true, new Role[0]),
                true
            };

            // permissions correct, correct command, no args
            yield return new object[]
            {
                GetBasicSettings(),
                GetPockyUser(new[] {Role.Config} ),
                new Message
                {
                    Text = "TestBot ping",
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
                            Text = " ping",
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
                GetTrigger("ping", true, false, new[] { Role.Config, Role.Admin } ),
                true
            };

            // wrong mention
            yield return new object[]
            {
                GetBasicSettings(),
                GetPockyUser(new Role[0]),
                new Message
                {
                    Text = "TestBot ping",
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
                            Text = " ping",
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
                GetTrigger("ping", true, false, new Role[0]),
                false
            };

            // no permissions, wrong command, no args
            yield return new object[]
            {
                GetBasicSettings(),
                GetPockyUser(new Role[0]),
                new Message
                {
                    Text = "TestBot notping",
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
                            Text = " notping",
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
                GetTrigger("ping", true, false, new Role[0]),
                false
            };

            // no permissions, correct command, args (none allowed)
            yield return new object[]
            {
                GetBasicSettings(),
                GetPockyUser(new Role[0]),
                new Message
                {
                    Text = "TestBot ping with a lil extra",
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
                            Text = " ping with a lil extra",
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
                GetTrigger("ping", true, false, new Role[0]),
                false
            };

            // incorrect permissions, correct command, no args
            yield return new object[]
            {
                GetBasicSettings(),
                GetPockyUser(new Role[0]),
                new Message
                {
                    Text = "TestBot ping",
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
                            Text = " ping",
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
                GetTrigger("ping", true, false, new[] { Role.Config } ),
                false
            };
        }

        public static IEnumerable<object[]> DirectMessageTestData()
        {
            // no permissions, correct command, no args
            yield return new object[]
            {
                GetPockyUser(new Role[0]),
                new Message
                {
                    Text = "ping",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "ping",
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
                GetTrigger("ping", true, false, new Role[0]),
                true
            };

            // no permissions, correct command (different casing and whitespace), no args
            yield return new object[]
            {
                GetPockyUser(new Role[0]),
                new Message
                {
                    Text = "Ping  ",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "Ping  ",
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
                GetTrigger("ping", true, false, new Role[0]),
                true
            };

            // no permissions, correct command, args (allowed)
            yield return new object[]
            {
                GetPockyUser(new Role[0]),
                new Message
                {
                    Text = "ping with a lil extra",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "ping with a lil extra",
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
                GetTrigger("ping", true, true, new Role[0]),
                true
            };

            // permissions correct, correct command, no args
            yield return new object[]
            {
                GetPockyUser(new[] {Role.Config} ),
                new Message
                {
                    Text = "ping",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "ping",
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
                GetTrigger("ping", true, false, new[] { Role.Config, Role.Admin } ),
                true
            };

            // direct message not allowed
            yield return new object[]
            {
                GetPockyUser(new Role[0]),
                new Message
                {
                    Text = "ping",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "ping",
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
                GetTrigger("ping", false, false, new Role[0]),
                false
            };

            // no permissions, wrong command, no args
            yield return new object[]
            {
                GetPockyUser(new Role[0]),
                new Message
                {
                    Text = "notping",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "notping",
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
                GetTrigger("ping", true, false, new Role[0]),
                false
            };

            // no permissions, correct command, args (none allowed)
            yield return new object[]
            {
                GetPockyUser(new Role[0]),
                new Message
                {
                    Text = "ping with a lil extra",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "ping with a lil extra",
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
                GetTrigger("ping", true, false, new Role[0]),
                false
            };

            // incorrect permissions, correct command, no args
            yield return new object[]
            {
                GetPockyUser(new Role[0]),
                new Message
                {
                    Text = "ping",
                    MessageParts = new[]
                    {
                        new MessagePart
                        {
                            Text = "ping",
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
                GetTrigger("ping", true, false, new[] { Role.Config } ),
                false
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

        private static PockyUser GetPockyUser(Role[] roles)
        {
            return new PockyUser
            {
                UserId = "testUserId",
                Username = "Test User",
                Roles = roles.Select(x => new UserRole { UserId = "testUserId", Role = x } ).ToList()
            };
        }

        private static ITrigger GetTrigger(string command, bool directMessageAllowed, bool canHaveArgs, Role[] permissions)
        {
            var trigger = Substitute.For<ITrigger>();
            trigger.Command.Returns(command);
            trigger.DirectMessageAllowed.Returns(directMessageAllowed);
            trigger.CanHaveArgs.Returns(canHaveArgs);
            trigger.Permissions.Returns(permissions);
            return trigger;
        }
    }
}
