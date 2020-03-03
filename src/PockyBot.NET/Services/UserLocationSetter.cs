using System;
using System.Linq;
using System.Threading.Tasks;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services
{
    public class UserLocationSetter : IUserLocationSetter
    {
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUserLocationRepository _userLocationRepository;

        internal UserLocationSetter(IPockyUserRepository pockyUserRepository, ILocationRepository locationRepository,
            IUserLocationRepository userLocationRepository)
        {
            _pockyUserRepository = pockyUserRepository;
            _locationRepository = locationRepository;
            _userLocationRepository = userLocationRepository;
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
    }
}
