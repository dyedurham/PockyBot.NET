using System;
using System.Linq;
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
        private readonly PockyBotSettings _pockyBotSettings;
        public string Command => Commands.Help;
        public bool DirectMessageAllowed => true;
        public bool CanHaveArgs => true;
        public string[] Permissions => Array.Empty<string>();

        public Help(IPockyUserRepository pockyUserRepository, IOptions<PockyBotSettings> pockySettings, IConfigRepository configRepository)
        {
            _pockyUserRepository = pockyUserRepository;
            _configRepository = configRepository;
            _pockyBotSettings = pockySettings.Value;
        }

        public async Task<Message> Respond(Message message)
        {
            var command = string.Join(" ", message.MessageParts.Where(x => x.MessageType == MessageType.Text).Select(x => x.Text).ToArray());
            var user = _pockyUserRepository.GetUser(message.Sender.UserId);
            var newMessage = CreateHelpResponseMessage(command, user);
            return new Message
            {
                Text = newMessage
            };
        }

        private string CreateHelpResponseMessage(string command, PockyUser user)
        {
            if (string.IsNullOrEmpty(command)) {
                return CreateCommandListMessage(user);
            }
            switch (command.ToLower()) {
                case Commands.Peg:
                    return CreatePegHelpMessage();
                case Commands.Status:
                    return CreateStatusHelpMessage();
                case Commands.Keywords:
                    return this.createKeywordsHelpMessage();
                case Commands.Ping:
                    return this.createPingHelpMessage();
                case Commands.Welcome:
                    return this.createWelcomeHelpMessage();
                case Commands.Rotation:
                    return this.createRotationHelpMessage();
                case Commands.Winners:
                    return this.createWinnersHelpMessage(message);
                case Commands.Results:
                    return this.createResultsHelpMessage(message);
                case Commands.Reset:
                    return this.createResetHelpMessage(message);
                case Commands.Update:
                    return this.createUpdateHelpMessage(message);
                case Commands.Finish:
                    return this.createFinishHelpMessage(message);
                case Commands.NumberConfig:
                    return this.createNumberConfigHelpMessage(message);
                case Commands.StringConfig:
                    return this.createStringConfigHelpMessage(message);
                case Commands.RoleConfig:
                    return this.createRoleConfigHelpMessage(message);
                case Commands.LocationConfig:
                    return this.createLocationConfigHelpMessage(message);
                case Commands.UserLocation:
                    return this.createUserLocationHelpMessage(message);
                case Commands.RemoveUser:
                    return this.createRemoveUserHelpMessage(message);
                case Commands.LocationWeight:
                    return this.createLocationWeightHelpMessage(message);
                default:
                    return CreateDefaultHelpMessage();
            }
        }

        private string CreateDefaultHelpMessage()
        {
            return $"Command not found. To see a full list of commands type `@{_pockyBotSettings.BotName} help` or direct message me with `help`.";
        }

        private string CreateCommandListMessage(PockyUser user)
        {
            var newMessage = "## What I can do (List of Commands)\n\n" +
                 $"* {Commands.Peg}\n" +
                 $"* {Commands.Status}\n" +
                 //$"* {Commands.Keywords}\n" +
                 $"* {Commands.Ping}\n";
                 //$"* {Commands.Welcome}\n" +
                 //$"* {Commands.Rotation}\n" +
                 //$"* {Commands.LocationConfig}\n" +
                 //$"* {Commands.UserLocation}\n";

            // if (HasPermission(user, new []{Roles.Admin, Roles.Winners})) {
            //     newMessage += $"* {Commands.Winners}\n";
            // }

            // if (HasPermission(user, new []{Roles.Admin, Roles.Results})) {
            //     newMessage += $"* {Commands.Results}\n";
            // }


            if (HasPermission(user, new []{Roles.Admin, Roles.Reset})) {
                newMessage += $"* {Commands.Reset}\n";
            }

            // if (HasPermission(user, new []{Roles.Admin, Roles.Update})) {
            //     newMessage += $"* {Commands.Update}\n";
            // }

            if (HasPermission(user, new []{Roles.Admin, Roles.Finish})) {
                newMessage += $"* {Commands.Finish}\n";
            }

            // if (HasPermission(user, new []{Roles.Admin, Roles.Config})) {
            //     newMessage += $"* {Commands.NumberConfig}\n";
            //     newMessage += $"* {Commands.StringConfig}\n";
            //     newMessage += $"* {Commands.RoleConfig}\n";
            //     newMessage += $"* {Commands.LocationWeight}\n";
            // }

            // if (HasPermission(user, new []{Roles.Admin, Roles.RemoveUser})) {
            //     newMessage += $"* {Commands.RemoveUser}\n";
            // }

            newMessage += $"\nFor more information on a command type `@{_pockyBotSettings.BotName} help command-name` or direct message me with `help command-name`\n";
            newMessage += "\nI am still being worked on, so more features to come.";
            return newMessage;
        }

        private string CreatePegHelpMessage()
        {
            var keywordsRequired = _configRepository.GetGeneralConfig("requireValues") == 1;
            var newMessage = "### How to give a peg 🎁!\n" +
                $"1. To give someone a peg type: `@{_pockyBotSettings.BotName} ${Commands.Peg} @bob {{comment}}`.\n";

            if (keywordsRequired) {
                newMessage += "1. Note that your comment MUST include a keyword.";
            }
            return newMessage;
        }

        private string CreateStatusHelpMessage()
        {
            return "### How to check your status 📈!\n" +
                $"1. To get a PM type: `@{_pockyBotSettings.BotName} ${Commands.Status}` OR direct message me with `${Commands.Status}`.\n" +
                "1. I will PM you number of pegs you have left and who you gave it to.";
        }

        private bool HasPermission(PockyUser user, string[] permissions)
        {
            return user != null && user.Roles.Any(x =>
                       permissions.Contains(x.UserRole, StringComparer.OrdinalIgnoreCase));
        }
    }
}
