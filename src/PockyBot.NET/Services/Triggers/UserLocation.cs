using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class UserLocation : ITrigger
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IPockyUserRepository _pockyUserRepository;

        public string Command => Commands.UserLocation;
        public bool DirectMessageAllowed => false;
        public bool CanHaveArgs => true;
        public string[] Permissions => Array.Empty<string>();

        public UserLocation(ILocationRepository locationRepository, IPockyUserRepository pockyUserRepository)
        {
            _locationRepository = locationRepository;
            _pockyUserRepository = pockyUserRepository;
        }

        public async Task<Message> Respond(Message message)
        {
            var fullText = message.MessageParts.Skip(1).Select(x => x.Text);

            var mentionedUsers = message.MessageParts.Skip(1)
                .Where(x => x.MessageType == MessageType.PersonMention)
                .Select(x => x.UserId)
                .ToArray();

            var commands = string.Join("", fullText)
                .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (commands.Length < 2)
            {
                return new Message
                {
                    Text = "Please specify a command. Possible values are get, set, and delete."
                };
            }

            var meId = message.Sender.UserId;
            var userIsAdmin = UserIsAdmin(meId);
            var command = commands[1];
            commands = commands.Skip(2).ToArray();
            string response;

            switch (command.ToLowerInvariant())
            {
                case Actions.Get:
                    response = await UserLocationGetMessage(commands, mentionedUsers, meId);
                    break;
                case Actions.Set:
                    throw new NotImplementedException();
                    //response = await UserLocationSet(commands, mentionedUsers, userIsAdmin, meId);
                    break;
                case Actions.Delete:
                    throw new NotImplementedException();
                    //response = await UserLocationDelete(commands, mentionedUsers, userIsAdmin, meId);
                    break;
                default:
                    response = "Unknown command. Possible values are get, set, and delete.";
                    break;
            }

            return new Message
            {
                Text = response
            };
        }

        private bool UserIsAdmin(string userId)
        {
            var user = _pockyUserRepository.GetUser(userId);
            return user.HasRole(Roles.Admin) || user.HasRole(Roles.Config);
        }

        private async Task<string> UserLocationGetMessage(string[] commands, string[] mentionedUsers, string meId)
        {

            if (commands.Length < 1)
            {
                return "Please specify a user or group of users. Possible arguments are me, unset, all, or a list of mentioned users.";
            }

            switch (commands[0].ToLowerInvariant())
            {
                case UserLocationTypes.Me:
                    var me = _pockyUserRepository.GetUser(meId);
                    return $"Your location is {me.Location.Location}";
                case UserLocationTypes.All:
                    return GetAllUserLocations();
                case UserLocationTypes.Unset:
                    return GetUnsetUserLocations();
                default:
                    return GetMentionedUserLocations(mentionedUsers);
            }
        }

        private string GetAllUserLocations()
        {
            var allUserLocations = _pockyUserRepository.GetAllUsersLocations();
            var builder = new StringBuilder("Here are all users' locations:\n\n");
            foreach (var user in allUserLocations)
            {
                if (user.Location != null && !string.IsNullOrWhiteSpace(user.Location.Location))
                {
                    builder.AppendLine($"* **{user.Username}**: {user.Location.Location}");
                }
            }

            return builder.ToString();
        }

        private string GetUnsetUserLocations()
        {
            var allUserLocations = _pockyUserRepository.GetAllUsersLocations();
            var builder = new StringBuilder("Here are the users without a location set:\n\n");
            foreach (var user in allUserLocations)
            {
                if (user.Location == null || string.IsNullOrWhiteSpace(user.Location.Location))
                {
                    builder.AppendLine($"* {user.Username}");
                }
            }

            return builder.ToString();
        }

        private string GetMentionedUserLocations(string[] mentionedUsers)
        {
            if (mentionedUsers.Length < 1)
            {
                return "Please specify a user or group of users. Possible arguments are me, unset, all, or a list of mentioned users.";
            }

            var builder = new StringBuilder("Here are the users' locations:\n\n");
            foreach (var userId in mentionedUsers)
            {
                var user = _pockyUserRepository.GetUser(userId);
                builder.AppendLine($"* **{user.Username}**: {user.Location.Location}");
            }

            return builder.ToString();
        }
    }
}
