using System;
using System.Linq;
using System.Threading.Tasks;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.UserLocation
{
    internal class UserLocationDeleter : IUserLocationDeleter
    {
        private readonly IUserLocationRepository _userLocationRepository;

        public UserLocationDeleter(IUserLocationRepository userLocationRepository)
        {
            _userLocationRepository = userLocationRepository;
        }

        public async Task<string> DeleteUserLocation(string[] commands, string[] mentionedUsers, bool userIsAdmin, string meId)
        {
            if (commands.Length < 1)
            {
                if (userIsAdmin)
                {
                    return "No user specified. Possible arguments are me or a list of mentioned users.";
                }

                return "No user specified. Possible arguments are me.";
            }

            if (!userIsAdmin && mentionedUsers != null && mentionedUsers.Length > 0)
            {
                return "Permission denied. You may only delete yourself.";
            }

            if (commands.Contains(UserLocationTypes.Me, StringComparer.InvariantCultureIgnoreCase))
            {
                await _userLocationRepository.DeleteUserLocation(meId);
            }

            await DeleteMentionedUsers(mentionedUsers).ConfigureAwait(false);

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
