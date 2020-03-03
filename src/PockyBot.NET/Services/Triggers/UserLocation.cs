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
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IUserLocationService _userLocationService;

        public string Command => Commands.UserLocation;
        public bool DirectMessageAllowed => false;
        public bool CanHaveArgs => true;
        public string[] Permissions => Array.Empty<string>();

        public UserLocation(IPockyUserRepository pockyUserRepository, IUserLocationService userLocationService)
        {
            _pockyUserRepository = pockyUserRepository;
            _userLocationService = userLocationService;
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

            return new Message
            {
                Text = command.ToLowerInvariant() switch
                {
                    Actions.Get => _userLocationService.GetUserLocation(commands, mentionedUsers, meId),
                    Actions.Set => await _userLocationService.SetUserLocation(commands, mentionedUsers, userIsAdmin, meId),
                    Actions.Delete => await _userLocationService.DeleteUserLocation(commands, mentionedUsers, userIsAdmin,
                        meId),
                    _ => "Unknown command. Possible values are get, set, and delete."
                }
            };
        }

        private bool UserIsAdmin(string userId)
        {
            var user = _pockyUserRepository.GetUser(userId);
            return user.HasRole(Roles.Admin) || user.HasRole(Roles.Config);
        }
    }
}
