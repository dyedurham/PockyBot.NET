using System.Linq;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Options;
using PockyBot.NET.Configuration;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal abstract class Trigger : ITrigger
    {
        protected readonly PockyBotSettings _settings;
        protected readonly IPockyUserRepository _pockyUserRepository;

        public Trigger(IOptions<PockyBotSettings> settings, IPockyUserRepository pockyUserRepository)
        {
            _settings = settings.Value;
            _pockyUserRepository = pockyUserRepository;
        }

        protected abstract string Command { get; }
        protected abstract bool DirectMessageAllowed { get; }
        protected abstract bool CanHaveArgs { get; }
        protected abstract bool PermissionsRequired { get; }
        protected string[] Permissions { get; } = new string[0];

        public bool ShouldTriggerInRoom(Message message)
        {
            if (message.MessageParts.Length < 2 || message.MessageParts[0].MessageType != MessageType.PersonMention
                || message.MessageParts[0].UserId != _settings.BotId
                || (PermissionsRequired && !HasPermission(message.SenderId)))
            {
                return false;
            }

            if (CanHaveArgs)
            {
                return message.MessageParts[1].Text.Trim().ToLower().StartsWith(Command.ToLower());
            }
            else
            {
                return message.MessageParts.Length == 2 && message.MessageParts[1].Text.Trim().ToLower() == Command.ToLower();
            }
        }

        public bool ShouldTriggerInDirectMessage(Message message)
        {
            if (!DirectMessageAllowed || (PermissionsRequired && !HasPermission(message.SenderId)))
            {
                return false;
            }

            if (CanHaveArgs)
            {
                return message.Text.Trim().ToLower().StartsWith(Command.ToLower());
            }
            else
            {
                return message.Text.Trim().ToLower() == Command.ToLower();
            }
        }

        private bool HasPermission(string senderId)
        {
            var user = _pockyUserRepository.GetUser(senderId);
            return user != null && user.Roles.Any(x => Permissions.Contains(x.UserRole));
        }

        public abstract Message Respond(Message message);
    }
}
