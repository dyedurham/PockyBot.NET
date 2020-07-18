using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using Microsoft.Extensions.Logging;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Services.Triggers
{
    internal class Unpeg : ITrigger
    {
        private readonly IRandomnessHandler _randomnessHandler;
        private readonly IChatHelper _chatHelper;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly ILogger<Unpeg> _logger;
        private readonly string[] _separatorWords = { "for", "because" };

        public string Command => Commands.Unpeg;

        public bool DirectMessageAllowed => false;

        public bool CanHaveArgs => true;

        public Role[] Permissions => Array.Empty<Role>();

        public Unpeg(IRandomnessHandler randomnessHandler, IChatHelper chatHelper, IBackgroundTaskQueue backgroundTaskQueue, ILogger<Unpeg> logger)
        {
            _randomnessHandler = randomnessHandler;
            _chatHelper = chatHelper;
            _backgroundTaskQueue = backgroundTaskQueue;
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

            var response = SendRandomResponse(message.Sender, recipientIsPerson, recipientName, recipientId, message.RoomId);
            return Task.FromResult(new Message
            {
                MessageParts = response
            });
        }

        private MessagePart[] SendRandomResponse(Person sender, bool recipientIsPerson, string recipientName,
            string recipientId, string roomId)
        {
            var random = _randomnessHandler.Random.Next(7);
            switch (random)
            {
                case 0:
                    SendQueuedMessage(new Message {Text = "Kidding!", RoomId = roomId});
                    return GetRecipientPegRemovedResponse(recipientIsPerson, recipientName, recipientId);
                case 1:
                    return GetPegsHiddenResponse(recipientIsPerson, recipientName, recipientId);
                case 2:
                    SendQueuedMessage(new Message
                    {
                        MessageParts = GetPegStolenBackResponse(recipientIsPerson, recipientName, recipientId),
                        RoomId = roomId
                    });
                    return GetSenderPegRemovedResponse(sender);
                case 3:
                    SendQueuedMessage(new Message
                    {
                        MessageParts = GetRecipientDidntWantItResponse(recipientIsPerson, recipientName, recipientId),
                        RoomId = roomId
                    });
                    return GetPegGivenToRecipientResponse(recipientIsPerson, recipientName, recipientId);
                case 4:
                    return GetICantDoThatResponse(sender);
                case 5:
                    return new[]
                    {
                        new MessagePart
                        {
                            Text = "### HTTP Status Code 418: I'm a teapot.\n\nUnable to brew coffee. Or pegs.",
                            MessageType = MessageType.Text
                        }
                    };
                case 6:
                    return GetPermissionDeniedResponse(sender);
                default:
                    return new[]
                    {
                        new MessagePart
                        {
                            Text = "Whoops, looks like RNGesus did not smile upon you today.",
                            MessageType = MessageType.Text
                        }
                    };
            }
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
                    await Task.Delay(TimeSpan.FromSeconds(30), token);
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

        private MessagePart[] GetRecipientPegRemovedResponse(bool recipientIsPerson, string recipientName, string recipientId)
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

        private MessagePart[] GetPegsHiddenResponse(bool recipientIsPerson, string recipientName, string recipientId)
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

        private MessagePart[] GetSenderPegRemovedResponse(Person sender)
        {
            return new[]
            {
                new MessagePart
                    {MessageType = MessageType.PersonMention, UserId = sender.UserId, Text = sender.Username},
                new MessagePart {Text = "'s peg has been removed...", MessageType = MessageType.Text}
            };
        }

        private MessagePart[] GetPegStolenBackResponse(bool recipientIsPerson, string recipientName, string recipientId)
        {
            if (recipientIsPerson)
            {
                return new[]
                {
                    new MessagePart {Text = "But ", MessageType = MessageType.Text},
                    new MessagePart
                        {MessageType = MessageType.PersonMention, UserId = recipientId, Text = recipientName},
                    new MessagePart {Text = " stole it back!", MessageType = MessageType.Text}
                };
            }

            return new[] {new MessagePart {Text = $"But {recipientName} stole it back!"}};
        }

        private MessagePart[] GetPegGivenToRecipientResponse(bool recipientIsPerson, string recipientName,
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

        private MessagePart[] GetRecipientDidntWantItResponse(bool recipientIsPerson, string recipientName,
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

        private MessagePart[] GetICantDoThatResponse(Person sender)
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

        private MessagePart[] GetPermissionDeniedResponse(Person sender)
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
