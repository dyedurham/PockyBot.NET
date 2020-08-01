using System.Text;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.UserLocation
{
    internal class UserLocationGetter : IUserLocationGetter
    {
        private readonly IPockyUserRepository _pockyUserRepository;

        public UserLocationGetter(IPockyUserRepository pockyUserRepository)
        {
            _pockyUserRepository = pockyUserRepository;
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
                    return $"Your location is {me.Location?.Location ?? "unset"}";
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
                if (user.Location != null && !string.IsNullOrWhiteSpace(user.Location.Location))
                {
                    builder.AppendLine($"* **{user.Username}**: {user.Location.Location}");
                }
                else
                {
                    builder.AppendLine($"* **{user.Username}**: No location set");
                }
            }

            return builder.ToString();
        }
    }
}
