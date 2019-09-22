using System.Linq;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Options;
using PockyBot.NET.Configuration;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Triggers;

namespace PockyBot.NET.Services
{
    internal class TriggerResponseTester : ITriggerResponseTester
    {
        protected readonly PockyBotSettings _settings;
        protected readonly IPockyUserRepository _pockyUserRepository;

        public TriggerResponseTester(IOptions<PockyBotSettings> settings, IPockyUserRepository pockyUserRepository)
        {
            _settings = settings.Value;
            _pockyUserRepository = pockyUserRepository;
        }

        public bool ShouldTriggerInRoom(Message message, ITrigger trigger)
        {
            if (message.MessageParts.Length < 2 || message.MessageParts[0].MessageType != MessageType.PersonMention
                || message.MessageParts[0].UserId != _settings.BotId
                || (trigger.Permissions.Length > 0 && !HasPermission(message.SenderId, trigger.Permissions)))
            {
                return false;
            }

            if (trigger.CanHaveArgs)
            {
                return message.MessageParts[1].Text.Trim().ToLower().StartsWith(trigger.Command.ToLower());
            }
            else
            {
                return message.MessageParts.Length == 2 && message.MessageParts[1].Text.Trim().ToLower() == trigger.Command.ToLower();
            }
        }

        public bool ShouldTriggerInDirectMessage(Message message, ITrigger trigger)
        {
            if (!trigger.DirectMessageAllowed || (trigger.Permissions.Length > 0 && !HasPermission(message.SenderId, trigger.Permissions)))
            {
                return false;
            }

            if (trigger.CanHaveArgs)
            {
                return message.Text.Trim().ToLower().StartsWith(trigger.Command.ToLower());
            }
            else
            {
                return message.Text.Trim().ToLower() == trigger.Command.ToLower();
            }
        }

        private bool HasPermission(string senderId, string[] permissions)
        {
            var user = _pockyUserRepository.GetUser(senderId);
            return user != null && user.Roles.Any(x => permissions.Contains(x.UserRole));
        }
    }
}
