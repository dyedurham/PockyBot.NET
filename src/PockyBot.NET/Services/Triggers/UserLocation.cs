using System;
using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class UserLocation : ITrigger
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

            await createUser;

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

        private async Task CreateUser(Person user)
        {
            await _pockyUserRepository.AddOrUpdateUserAsync(user.UserId, user.Username);
        }

        private bool UserIsAdmin(string userId)
        {
            var user = _pockyUserRepository.GetUser(userId);
            return user.HasRole(Role.ADMIN) || user.HasRole(Role.CONFIG);
        }
    }
}
