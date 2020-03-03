using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services
{
    internal class UserLocationService : IUserLocationService
    {
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUserLocationRepository _userLocationRepository;

        internal UserLocationService(IPockyUserRepository pockyUserRepository, ILocationRepository locationRepository,
            IUserLocationRepository userLocationRepository)
        {
            _pockyUserRepository = pockyUserRepository;
            _locationRepository = locationRepository;
            _userLocationRepository = userLocationRepository;
        }

        public string GetUserLocation(string[] commands, string[] mentionedUsers, string meId)
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

        public async Task<string> SetUserLocation(string[] commands, string[] mentionedUsers, bool userIsAdmin, string meId)
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
            var users = commands.Skip(1).ToArray();

            // Ensure specified location is valid
            if (!locations.Any(validLocation => givenLocation.Equals(validLocation, StringComparison.InvariantCultureIgnoreCase)))
            {
                return $"Location {givenLocation} does not exist. Valid values are: {string.Join(", ", locations)}.";
            }

            // Update user "me"
            if (string.Equals(users[0], UserLocationTypes.Me, StringComparison.InvariantCultureIgnoreCase))
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

        public async Task<string> DeleteUserLocation(string[] commands, string[] mentionedUsers, bool userIsAdmin, string meId)
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

        private async Task DeleteMentionedUsers(string[] mentionedUsers)
        {
            var tasks = mentionedUsers.Select(user =>
                _userLocationRepository.DeleteUserLocation(user)).ToList();

            await Task.WhenAll(tasks);
        }
    }
}
