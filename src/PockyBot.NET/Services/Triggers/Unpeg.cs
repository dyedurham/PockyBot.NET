using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using Microsoft.Extensions.Logging;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Services.Helpers;

namespace PockyBot.NET.Services.Triggers
{
    internal class Unpeg : ITrigger, IHelpMessageTrigger
    {
        private readonly IRandomnessHandler _randomnessHandler;
        private readonly IChatHelper _chatHelper;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IAsyncDelayer _asyncDelayer;
        private readonly ILogger<Unpeg> _logger;
        private readonly string[] _separatorWords = { "for", "because" };

        public string Command => Commands.Unpeg;

        public bool DirectMessageAllowed => false;

        public bool CanHaveArgs => true;

        public Role[] Permissions => Array.Empty<Role>();

        public Unpeg(IRandomnessHandler randomnessHandler, IChatHelper chatHelper, IBackgroundTaskQueue backgroundTaskQueue, IAsyncDelayer asyncDelayer, ILogger<Unpeg> logger)
        {
            _randomnessHandler = randomnessHandler;
            _chatHelper = chatHelper;
            _backgroundTaskQueue = backgroundTaskQueue;
            _asyncDelayer = asyncDelayer;
            _logger = logger;
        }

        public Task<Message> Respond(Message message)
        {
            var recipientIsPerson = true;
            var recipientName = message.MessageParts[1].Text.Trim().Substring(Command.Length).Trim();
            string recipientId = null;

            if (string.IsNullOrEmpty(recipientName))
            {
                if (message.MessageParts.Length < 3 || message.MessageParts[2].MessageType != MessageType.PersonMention)
                {
                    return Task.FromResult(new Message
                    {
                        Text = "Please provide someone or something to unpeg."
                    });
                }

                recipientName = message.MessageParts[2].Text;
                recipientId = message.MessageParts[2].UserId;
            }
            else
            {
                recipientIsPerson = false;
                recipientName = GetRecipientNameFromString(recipientName);
                if (recipientName == "me")
                {
                    recipientIsPerson = true;
                    recipientName = message.Sender.Username;
                    recipientId = message.Sender.UserId;
                }
            }

            var response = SendRandomResponse(message.Sender, recipientIsPerson, recipientName, recipientId,
                message.RoomId, message.Id);
            return Task.FromResult(new Message
            {
                MessageParts = response
            });
        }

        public string GetHelpMessage(string botName, PockyUser user)
        {
            return "### How to unpeg someone 💔!\n" +
                   $"1. To unpeg someone or something type: `@{botName} {Commands.Unpeg} @Person/thing {{comment}}`.\n";
        }

        private MessagePart[] SendRandomResponse(Person sender, bool recipientIsPerson, string recipientName,
            string recipientId, string roomId, string parentId)
        {
            Func<MessagePart[]>[] actions =
            {
                () =>
                {
                    SendQueuedMessage(new Message {Text = "Kidding!", RoomId = roomId, ParentId = parentId});
                    return GetRecipientPegRemovedResponse(recipientIsPerson, recipientName, recipientId);
                },
                () =>
                    GetPegsHiddenResponse(recipientIsPerson, recipientName, recipientId),
                () =>
                {
                    SendQueuedMessage(new Message
                    {
                        MessageParts = GetSenderPegStolenBackResponse(sender),
                        RoomId = roomId,
                        ParentId = parentId
                    });
                    return GetSenderPegRemovedResponse(sender);
                },
                () =>
                {
                    SendQueuedMessage(new Message
                    {
                        MessageParts =
                            GetRecipientDidntWantItResponse(recipientIsPerson, recipientName, recipientId),
                        RoomId = roomId,
                        ParentId = parentId
                    });
                    return GetPegGivenToRecipientResponse(recipientIsPerson, recipientName, recipientId);
                },
                () =>
                    GetICantDoThatResponse(sender),
                () =>
                {
                    return new[]
                    {
                        new MessagePart
                        {
                            Text = "### HTTP Status Code 418: I'm a teapot.\n\nUnable to brew coffee. Or pegs.",
                            MessageType = MessageType.Text
                        }
                    };
                },
                () => GetPermissionDeniedResponse(sender)
            };

            var random = _randomnessHandler.Random.Next(actions.Length);
            return actions[random]();
        }

        private string GetRecipientNameFromString(string message)
        {
            var separatorIndex = -1;
            foreach (var separator in _separatorWords)
            {
                var index = message.IndexOf(separator, StringComparison.OrdinalIgnoreCase);
                if (separatorIndex == -1 || (index > -1 && index < separatorIndex))
                {
                    separatorIndex = index;
                }
            }

            if (separatorIndex == -1)
            {
                separatorIndex = message.IndexOf(' ');
            }

            if (separatorIndex == -1)
            {
                return message;
            }

            return message.Substring(0, separatorIndex).Trim();
        }

        private void SendQueuedMessage(Message message)
        {
            _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
            {
                try
                {
                    await _asyncDelayer.Delay(TimeSpan.FromSeconds(30), token);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogWarning($"Message sending cancelled.");
                }

                if (!token.IsCancellationRequested)
                {
                    await _chatHelper.Messages.SendMessageAsync(message);
                }
            });
        }

        private static MessagePart[] GetRecipientPegRemovedResponse(bool recipientIsPerson, string recipientName, string recipientId)
        {
            if (recipientIsPerson)
            {
                return new[]
                {
                    new MessagePart {Text = "Peg removed from ", MessageType = MessageType.Text},
                    new MessagePart
                        {MessageType = MessageType.PersonMention, UserId = recipientId, Text = recipientName},
                    new MessagePart {Text = ".", MessageType = MessageType.Text}
                };
            }

            return new[] { new MessagePart { Text = $"Peg removed from {recipientName}." } };
        }

        private static MessagePart[] GetPegsHiddenResponse(bool recipientIsPerson, string recipientName, string recipientId)
        {
            if (recipientIsPerson)
            {
                return new[]
                {
                    new MessagePart {Text = "It looks like ", MessageType = MessageType.Text},
                    new MessagePart
                    {
                        MessageType = MessageType.PersonMention,
                        UserId = recipientId,
                        Text = recipientName
                    },
                    new MessagePart
                    {
                        Text = " has hidden their pegs too well for me to find them!",
                        MessageType = MessageType.Text
                    }
                };
            }

            return new[]
            {
                new MessagePart
                {
                    Text =
                        $"It looks like {recipientName} has hidden their pegs too well for me to find them!"
                }
            };
        }

        private static MessagePart[] GetSenderPegRemovedResponse(Person sender)
        {
            return new[]
            {
                new MessagePart
                    {MessageType = MessageType.PersonMention, UserId = sender.UserId, Text = sender.Username},
                new MessagePart {Text = "'s peg has been removed...", MessageType = MessageType.Text}
            };
        }

        private static MessagePart[] GetSenderPegStolenBackResponse(Person sender)
        {
            return new[]
            {
                new MessagePart {Text = "But ", MessageType = MessageType.Text},
                new MessagePart
                    {MessageType = MessageType.PersonMention, UserId = sender.UserId, Text = sender.Username},
                new MessagePart {Text = " stole it back!", MessageType = MessageType.Text}
            };
        }

        private static MessagePart[] GetPegGivenToRecipientResponse(bool recipientIsPerson, string recipientName,
            string recipientId)
        {
            if (recipientIsPerson)
            {
                return new[]
                {
                    new MessagePart {Text = "Peg given to ", MessageType = MessageType.Text},
                    new MessagePart
                        {MessageType = MessageType.PersonMention, UserId = recipientId, Text = recipientName},
                    new MessagePart {Text = ".", MessageType = MessageType.Text}
                };
            }

            return new[] {new MessagePart {Text = $"Peg given to {recipientName}.", MessageType = MessageType.Text}};
        }

        private static MessagePart[] GetRecipientDidntWantItResponse(bool recipientIsPerson, string recipientName,
            string recipientId)
        {
            if (recipientIsPerson)
            {
                return new[]
                {
                    new MessagePart {Text = "But ", MessageType = MessageType.Text},
                    new MessagePart
                        {MessageType = MessageType.PersonMention, UserId = recipientId, Text = recipientName},
                    new MessagePart {Text = " didn't want it!", MessageType = MessageType.Text}
                };
            }

            return new[] { new MessagePart { Text = $"But {recipientName} didn't want it!", MessageType = MessageType.Text } };
        }

        private static MessagePart[] GetICantDoThatResponse(Person sender)
        {
            return new[]
            {
                new MessagePart { Text = "I'm sorry ", MessageType = MessageType.Text },
                new MessagePart
                {
                    MessageType = MessageType.PersonMention,
                    UserId = sender.UserId,
                    Text = sender.Username
                },
                new MessagePart { Text = ", I'm afraid I can't do that.", MessageType = MessageType.Text }
            };
        }

        private static MessagePart[] GetPermissionDeniedResponse(Person sender)
        {
            Regex nonAlphaRegex = new Regex("[^0-9a-z]", RegexOptions.IgnoreCase);
            string cleanSenderName = nonAlphaRegex.Replace(sender.Username, "");
            return new[]
            {
                new MessagePart
                {
                    Text = $@"```
Error: Access Denied user {sender.Username} does not have the correct privileges
	at UnPeg (unpeg.js:126)
	at EveryoneBut{cleanSenderName} (unpeg.js:4253)
	at ExecuteBadCode (pockybot.js:1467)
	at DecrementPegs (pockybot.js:1535)
```"
                }
            };
        }
    }
}
