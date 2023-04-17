using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Options;
using PockyBot.NET.Configuration;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class Help : ITrigger
    {
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IConfigRepository _configRepository;
        private readonly IEnumerable<ITrigger> _triggers;
        private readonly PockyBotSettings _pockyBotSettings;
        public string Command => Commands.Help;
        public bool DirectMessageAllowed => true;
        public bool CanHaveArgs => true;
        public Role[] Permissions => Array.Empty<Role>();

        public Help(IPockyUserRepository pockyUserRepository, IOptions<PockyBotSettings> pockySettings, IConfigRepository configRepository, IEnumerable<ITrigger> triggers)
        {
            _pockyUserRepository = pockyUserRepository;
            _configRepository = configRepository;
            _triggers = triggers;
            _pockyBotSettings = pockySettings.Value;
        }

        public Task<Message> Respond(Message message)
        {
            var partsToSkip = message.MessageParts[0].MessageType == MessageType.PersonMention ? 1 : 0;
            var command = string.Join("", message.MessageParts.Skip(partsToSkip).Select(x => x.Text)).Trim().Remove(0, 4).Trim();
            var user = _pockyUserRepository.GetUser(message.Sender.UserId);
            var newMessage = CreateHelpResponseMessage(command, user);
            return Task.FromResult(new Message
            {
                Text = newMessage
            });
        }

        public string GetHelpMessage(string botName, PockyUser user)
        {
            return $"Type `@{botName} {Commands.Help} <command-name>` for information on a command";
        }

        private string CreateHelpResponseMessage(string command, PockyUser user)
        {
            if (string.IsNullOrEmpty(command)) {
                return CreateCommandListMessage(user);
            }

            var trigger = _triggers.FirstOrDefault(x =>
                x.Command.Equals(command, StringComparison.InvariantCultureIgnoreCase) &&
                HasPermission(user, x.Permissions));
            if (trigger != null)
            {
                return trigger.GetHelpMessage(_pockyBotSettings.BotName, user);
            }

            return CreateDefaultHelpMessage();
        }

        private string CreateDefaultHelpMessage()
        {
            return $"Command not found. To see a full list of commands type `@{_pockyBotSettings.BotName} help` or direct message me with `help`.";
        }

        private string CreateCommandListMessage(PockyUser user)
        {
            var stringBuilder = new StringBuilder("## What I can do (List of Commands)\n\n");
            foreach (var trigger in _triggers)
            {
                if (HasPermission(user, trigger.Permissions))
                {
                    stringBuilder.Append($"* {trigger.Command}\n");
                }
            }

            stringBuilder.Append($"\nFor more information on a command type `@{_pockyBotSettings.BotName} help command-name` or direct message me with `help command-name`\n");
            stringBuilder.Append("\nI am still being worked on, so more features to come.");
            return stringBuilder.ToString();
        }

        private static bool HasPermission(PockyUser user, Role[] permissions)
        {
            return user != null && (permissions.Length == 0 || user.Roles.Any(x =>
                       permissions.Contains(x.Role)));
        }
    }
}
