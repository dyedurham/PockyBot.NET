using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class LocationConfig : ITrigger
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IPockyUserRepository _pockyUserRepository;

        public string Command => Commands.LocationConfig;

        public bool DirectMessageAllowed => false;

        public bool CanHaveArgs => true;

        public Role[] Permissions => Array.Empty<Role>();

        public LocationConfig(ILocationRepository locationRepository, IPockyUserRepository pockyUserRepository)
        {
            _locationRepository = locationRepository;
            _pockyUserRepository = pockyUserRepository;
        }

        public async Task<Message> Respond(Message message)
        {
            var fullText = message.MessageParts.Skip(1).Select(x => x.Text);
            var commands = string.Join("", fullText)
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (commands.Length < 2)
            {
                return new Message
                {
                    Text = "Please specify a command. Possible values are get, add, and delete."
                };
            }

            var locations = _locationRepository.GetAllLocations();
            string response;

            switch (commands[1].ToLower(CultureInfo.InvariantCulture))
            {
                case Actions.Get:
                    response = GetLocationMessage(locations);
                    break;
                case Actions.Add:
                    response = await AddLocation(message, commands, locations).ConfigureAwait(false);
                    break;
                case Actions.Delete:
                    response = await DeleteLocation(message, commands, locations).ConfigureAwait(false);
                    break;
                default:
                    response = "Unknown command. Possible values are get, add, and delete.";
                    break;
            }

            return new Message
            {
                Text = response
            };
        }

        private static string GetLocationMessage(string[] locations)
        {
            if (locations.Length == 0)
            {
                return "No locations added.";
            }

            return $"Here are the current locations:\n* {string.Join("\n* ", locations)}";
        }

        private async Task<string> AddLocation(Message message, string[] commands, string[] locations)
        {
            var user = _pockyUserRepository.GetUser(message.Sender.UserId);
            if (!(user.HasRole(Role.ADMIN) || user.HasRole(Role.CONFIG)))
            {
                return "Permission denied. You may only use the 'get' command.";
            }

            if (commands.Length != 3)
            {
                return "You must specify a location name to add.";
            }

            if (locations.Contains(commands[2], StringComparer.InvariantCultureIgnoreCase))
            {
                return $"Location value {commands[2]} has already been added.";
            }

            await _locationRepository.AddLocation(commands[2]);
            return "Location has been added.";
        }

        private async Task<string> DeleteLocation(Message message, string[] commands, string[] locations)
        {
            var user = _pockyUserRepository.GetUser(message.Sender.UserId);
            if (!(user.HasRole(Role.ADMIN) || user.HasRole(Role.CONFIG)))
            {
                return "Permission denied. You may only use the 'get' command.";
            }

            if (commands.Length != 3)
            {
                return "You must specify a location name to delete.";
            }

            if (!locations.Contains(commands[2], StringComparer.InvariantCultureIgnoreCase))
            {
                return $"Location value {commands[2]} does not exist.";
            }

            await _locationRepository.DeleteLocation(commands[2]);
            return "Location has been deleted.";
        }
    }
}
