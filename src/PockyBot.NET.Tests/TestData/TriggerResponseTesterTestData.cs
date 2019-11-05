using System.Collections.Generic;
using System.Linq;
using GlobalX.ChatBots.Core.Messages;
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
                GetPockyUser(new string[0]),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", true, false, new string[0]),
                true
            };

            // no permissions, correct command (different casing), no args
            yield return new object[]
            {
                GetBasicSettings(),
                GetPockyUser(new string[0]),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", true, false, new string[0]),
                true
            };

            // no permissions, correct command, args (allowed)
            yield return new object[]
            {
                GetBasicSettings(),
                GetPockyUser(new string[0]),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", true, true, new string[0]),
                true
            };

            // permissions correct, correct command, no args
            yield return new object[]
            {
                GetBasicSettings(),
                GetPockyUser(new[] {"ping"} ),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", true, false, new[] { "ping", "admin" } ),
                true
            };

            // wrong mention
            yield return new object[]
            {
                GetBasicSettings(),
                GetPockyUser(new string[0]),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", true, false, new string[0]),
                false
            };

            // no permissions, wrong command, no args
            yield return new object[]
            {
                GetBasicSettings(),
                GetPockyUser(new string[0]),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", true, false, new string[0]),
                false
            };

            // no permissions, correct command, args (none allowed)
            yield return new object[]
            {
                GetBasicSettings(),
                GetPockyUser(new string[0]),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", true, false, new string[0]),
                false
            };

            // incorrect permissions, correct command, no args
            yield return new object[]
            {
                GetBasicSettings(),
                GetPockyUser(new string[0]),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", true, false, new[] { "ping" } ),
                false
            };
        }

        public static IEnumerable<object[]> DirectMessageTestData()
        {
            // no permissions, correct command, no args
            yield return new object[]
            {
                GetPockyUser(new string[0]),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", true, false, new string[0]),
                true
            };

            // no permissions, correct command (different casing and whitespace), no args
            yield return new object[]
            {
                GetPockyUser(new string[0]),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", true, false, new string[0]),
                true
            };

            // no permissions, correct command, args (allowed)
            yield return new object[]
            {
                GetPockyUser(new string[0]),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", true, true, new string[0]),
                true
            };

            // permissions correct, correct command, no args
            yield return new object[]
            {
                GetPockyUser(new[] {"ping"} ),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", true, false, new[] { "ping", "admin" } ),
                true
            };

            // direct message not allowed
            yield return new object[]
            {
                GetPockyUser(new string[0]),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", false, false, new string[0]),
                false
            };

            // no permissions, wrong command, no args
            yield return new object[]
            {
                GetPockyUser(new string[0]),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", true, false, new string[0]),
                false
            };

            // no permissions, correct command, args (none allowed)
            yield return new object[]
            {
                GetPockyUser(new string[0]),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", true, false, new string[0]),
                false
            };

            // incorrect permissions, correct command, no args
            yield return new object[]
            {
                GetPockyUser(new string[0]),
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
                    SenderId = "testUserId",
                    SenderName = "Test User"
                },
                GetTrigger("ping", true, false, new[] { "ping" } ),
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

        private static PockyUser GetPockyUser(string[] roles)
        {
            return new PockyUser
            {
                UserId = "testUserId",
                Username = "Test User",
                Roles = roles.Select(x => new Role { UserId = "testUserId", UserRole = x } ).ToList()
            };
        }

        private static ITrigger GetTrigger(string command, bool directMessageAllowed, bool canHaveArgs, string[] permissions)
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
