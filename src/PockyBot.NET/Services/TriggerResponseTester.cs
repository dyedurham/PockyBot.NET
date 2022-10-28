using System;
using System.Linq;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Options;
using PockyBot.NET.Configuration;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Triggers;

namespace PockyBot.NET.Services
{
    internal class TriggerResponseTester : ITriggerResponseTester
    {
        private readonly PockyBotSettings _settings;
        private readonly IPockyUserRepository _pockyUserRepository;

        public TriggerResponseTester(IOptions<PockyBotSettings> settings, IPockyUserRepository pockyUserRepository)
        {
            _settings = settings.Value;
            _pockyUserRepository = pockyUserRepository;
        }

        public bool ShouldTriggerInRoom(Message message, ITrigger trigger)
        {
            if (message.MessageParts.Length < 2
                || message.MessageParts[0].MessageType != MessageType.PersonMention
                || message.MessageParts[0].UserId != _settings.BotId
                || (trigger.Permissions.Length > 0 && !HasPermission(message.Sender.UserId, trigger.Permissions)))
            {
                return false;
            }

            if (trigger.CanHaveArgs)
            {
                return message.MessageParts[1].Text.Trim()
                    .StartsWith(trigger.Command, StringComparison.OrdinalIgnoreCase);
            }

            return message.MessageParts.Length == 2 && string.Equals(message.MessageParts[1].Text.Trim(),
                       trigger.Command, StringComparison.OrdinalIgnoreCase);
        }

        public bool ShouldTriggerInDirectMessage(Message message, ITrigger trigger)
        {
            if (trigger.Permissions.Length > 0 && !HasPermission(message.Sender.UserId, trigger.Permissions))
            {
                return false;
            }

            if (trigger.CanHaveArgs)
            {
                return message.Text.Trim().StartsWith(trigger.Command, StringComparison.OrdinalIgnoreCase);
            }

            return string.Equals(message.Text.Trim(), trigger.Command, StringComparison.OrdinalIgnoreCase);
        }

        private bool HasPermission(string senderId, Role[] permissions)
        {
            var user = _pockyUserRepository.GetUser(senderId);
            return user != null && user.Roles.Any(x =>
                       permissions.Contains(x.Role));
        }
    }
}
