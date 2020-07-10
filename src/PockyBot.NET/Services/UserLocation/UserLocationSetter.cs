using System;
using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.UserLocation
{
    internal class UserLocationSetter : IUserLocationSetter
    {
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUserLocationRepository _userLocationRepository;
        private readonly IChatHelper _chatHelper;

        public UserLocationSetter(IPockyUserRepository pockyUserRepository, ILocationRepository locationRepository,
            IUserLocationRepository userLocationRepository, IChatHelper chatHelper)
        {
            _pockyUserRepository = pockyUserRepository;
            _locationRepository = locationRepository;
            _userLocationRepository = userLocationRepository;
            _chatHelper = chatHelper;
        }

        public async Task<string> SetUserLocation(string[] commands, string[] mentionedUsers, bool userIsAdmin, string meId)
        {
            if (commands.Length < 2)
            {
                if (userIsAdmin)
                {
                    return "User or location was not specified. Possible arguments are \"<location> me\" or \"<location> <list of users>\"";
                }

                return "User or location was not specified. Argument must be in the form of \"<location> me\"";
            }

            var locations = _locationRepository.GetAllLocations();
            var givenLocation = commands[0];
            var users = commands.Skip(1).ToArray();

            // Ensure specified location is valid
            if (!locations.Any(validLocation => givenLocation.Equals(validLocation, StringComparison.InvariantCultureIgnoreCase)))
            {
                return $"Location {givenLocation} does not exist. Valid values are: {string.Join(", ", locations)}.";
            }

            if (!userIsAdmin && mentionedUsers != null && mentionedUsers.Length > 0)
            {
                return "Permission denied. You may only set location for yourself.";
            }

            // Update user "me"
            if (users.Contains(UserLocationTypes.Me, StringComparer.InvariantCultureIgnoreCase))
            {
                await _userLocationRepository.UpsertUserLocation(_pockyUserRepository.GetUser(meId), givenLocation);
            }

            await SetMentionedUserLocations(givenLocation, mentionedUsers).ConfigureAwait(false);

            return "User locations added.";
        }

        private async Task SetMentionedUserLocations(string givenLocation, string[] mentionedUsers)
        {
            if (mentionedUsers == null)
            {
                return;
            }

            var tasks = mentionedUsers.Select(async mentionedUser =>
            {
                var user = _pockyUserRepository.GetUser(mentionedUser);
                if (user == null)
                {
                    var chatUser = await _chatHelper.People.GetPersonAsync(mentionedUser);
                    user = await _pockyUserRepository.AddOrUpdateUserAsync(chatUser.UserId, chatUser.Username);
                }
                await _userLocationRepository.UpsertUserLocation(user, givenLocation);
            }).ToList();

            await Task.WhenAll(tasks);
        }
    }
}
