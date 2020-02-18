using System;
using System.Collections.Generic;
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
        private readonly IUserLocationRepository _userLocationRepository;

        public string Command => Commands.UserLocation;
        public bool DirectMessageAllowed => false;
        public bool CanHaveArgs => true;
        public string[] Permissions => Array.Empty<string>();

        public UserLocation(
            ILocationRepository locationRepository,
            IPockyUserRepository pockyUserRepository,
            IUserLocationRepository userLocationRepository)
        {
            _locationRepository = locationRepository;
            _pockyUserRepository = pockyUserRepository;
            _userLocationRepository = userLocationRepository;
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
                    response = GetUserLocation(commands, mentionedUsers, meId);
                    break;
                case Actions.Set:
                    throw new NotImplementedException();
                    response = await SetUserLocation(commands, mentionedUsers, userIsAdmin, meId);
                    break;
                case Actions.Delete:
                    response = await DeleteUserLocation(commands, mentionedUsers, userIsAdmin, meId);
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

        private string GetUserLocation(string[] commands, string[] mentionedUsers, string meId)
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

        private async Task<string> SetUserLocation(string[] commands, string[] mentionedUsers, bool userIsAdmin, string meId)
        {
            if (commands.Length < 2)
            {
                if (userIsAdmin)
                {
                    return "User or location was not specified. Possible arguments are \"<location> me\" or \"<location> <list of users>\"";
                }

                return "User or location was not specified. Argument must be in the form of \"<location> me";
            }

            var locations = _locationRepository.GetAllLocations();
            var givenLocation = commands[0];
            var users = commands.Skip(1);

            // Ensure specified location is valid
            if (!locations.Any(validLocation => givenLocation.Contains(validLocation, StringComparison.InvariantCultureIgnoreCase)))
            {
                return $"Location {givenLocation} does not exist. Valid values are: {string.Join(", ", locations)}.";
            }

            // Update user "me"
            if (string.Equals(commands[0], UserLocationTypes.Me, StringComparison.InvariantCultureIgnoreCase))
            {
                await _userLocationRepository.UpsertUserLocation(_pockyUserRepository.GetUser(meId), givenLocation);
                return "User location added.";
            }

            // Update user list
            var tasks = mentionedUsers.Select(user =>
                _userLocationRepository.UpsertUserLocation(_pockyUserRepository.GetUser(user), givenLocation)).ToList();

            await Task.WhenAll(tasks);
            return "User locations added.";
        }


        private async Task<string> DeleteUserLocation(string[] commands, string[] mentionedUsers, bool userIsAdmin, string meId)
        {
            if (commands.Length < 1)
            {
                if (userIsAdmin)
                {
                    return "No user specified. Possible argument are me or a list of mentioned users.";
                }

                return "No user specified. Possible arguments are me.";
            }

            if (string.Equals(commands[0], UserLocationTypes.Me, StringComparison.InvariantCultureIgnoreCase))
            {
                await _userLocationRepository.DeleteUserLocation(meId);
            }

            if (!userIsAdmin)
            {
                return "Permission denied. You may only delete yourself.";
            }

            await DeleteMentionedUsers(mentionedUsers);

            return "User locations removed.";
        }

        private async Task DeleteMentionedUsers(string[] mentionedUsers)
        {
            var tasks = mentionedUsers.Select(user =>
                _userLocationRepository.DeleteUserLocation(user)).ToList();

            await Task.WhenAll(tasks);
        }
    }
}
