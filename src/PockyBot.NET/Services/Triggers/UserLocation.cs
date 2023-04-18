using System;
using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.UserLocation;

namespace PockyBot.NET.Services.Triggers
{
    internal class UserLocation : ITrigger, IHelpMessageTrigger
    {
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IUserLocationGetter _userLocationGetter;
        private readonly IUserLocationSetter _userLocationSetter;
        private readonly IUserLocationDeleter _userLocationDeleter;

        public string Command => Commands.UserLocation;
        public bool DirectMessageAllowed => false;
        public bool CanHaveArgs => true;
        public Role[] Permissions => Array.Empty<Role>();

        public UserLocation(IPockyUserRepository pockyUserRepository, IUserLocationGetter userLocationGetter,
            IUserLocationSetter userLocationSetter, IUserLocationDeleter userLocationDeleter)
        {
            _pockyUserRepository = pockyUserRepository;
            _userLocationGetter = userLocationGetter;
            _userLocationSetter = userLocationSetter;
            _userLocationDeleter = userLocationDeleter;
        }

        public async Task<Message> Respond(Message message)
        {
            var createUser = CreateUser(message.Sender);
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

            await createUser.ConfigureAwait(false);

            return new Message
            {
                Text = command.ToLowerInvariant() switch
                {
                    Actions.Get => _userLocationGetter.GetUserLocation(commands, mentionedUsers, meId),
                    Actions.Set => await _userLocationSetter.SetUserLocation(commands, mentionedUsers, userIsAdmin, meId),
                    Actions.Delete => await _userLocationDeleter.DeleteUserLocation(commands, mentionedUsers, userIsAdmin,
                        meId),
                    _ => "Unknown command. Possible values are get, set, and delete."
                }
            };
        }

        public string GetHelpMessage(string botName, PockyUser user)
        {
            if (user.HasRole(Role.Admin) || user.HasRole(Role.Config)) {
                return "### How to configure user location values!\n" +
                       $"1. To get user locations for yourself or others, type `@{botName} {Commands.UserLocation} {Actions.Get} me|all|unset|@User`\n" +
                       $"1. To set user locations, type `@{botName} {Commands.UserLocation} {Actions.Set} {{location}} me|@User1 @User2`\n" +
                       $"1. To delete user locations, type `@{botName} {Commands.UserLocation} {Actions.Delete} me|@User1 @User2`\n" +
                       "1. I will respond in the room you messaged me in.";
            }
            return "### How to config your user location value!\n" +
                   $"1. To get user locations for yourself or others, type `@{botName} {Commands.UserLocation} {Actions.Get} me|all|unset|@User`\n" +
                   $"1. To set your user location, type `@{botName} {Commands.UserLocation} {Actions.Set} {{location}} me`\n" +
                   "    * To bulk configure user locations, please ask an admin.\n" +
                   $"1. To delete your user location, type `@{botName} {Commands.UserLocation} {Actions.Delete} me`\n" +
                   "1. I will respond in the room you messaged me in.";
        }

        private async Task CreateUser(Person user)
        {
            await _pockyUserRepository.AddOrUpdateUserAsync(user.UserId, user.Username);
        }

        private bool UserIsAdmin(string userId)
        {
            var user = _pockyUserRepository.GetUser(userId);
            return user.HasRole(Role.Admin) || user.HasRole(Role.Config);
        }
    }
}
