using System;
using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
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

        public string[] Permissions => Array.Empty<string>();

        public LocationConfig(ILocationRepository locationRepository, IPockyUserRepository pockyUserRepository)
        {
            _locationRepository = locationRepository;
            _pockyUserRepository = pockyUserRepository;
        }

        public async Task<Message> Respond(Message message)
        {
            var fullText = message.MessageParts.Skip(1).Select(x => x.Text);
            var commands = string.Join("", fullText)
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (commands.Length < 2)
            {
                return new Message
                {
                    Text = "Please specify a command. Possible values are get, set and delete."
                };
            }

            string[] locations = _locationRepository.GetAllLocations();
            string response;
            
            switch (commands[1].ToLower())
            {
                case Actions.Get:
                    response = GetLocationMessage(locations);
                    break;
                case Actions.Set:
                    response = await SetLocation(message, commands, locations);
                    break;
                case Actions.Delete:
                    response = await DeleteLocation(message, commands, locations);
                    break;
                default:
                    response = "Unknown command. Possible values are get, set and delete.";
                    break;
            }

            return new Message
            {
                Text = response
            };
        }

        private string GetLocationMessage(string[] locations)
        {
            if (locations.Length == 0)
            {
                return "No locations set";
            }

            return $"Here are the current locations:\n* {string.Join("\n* ", locations)}";
        }

        private async Task<string> SetLocation(Message message, string[] commands, string[] locations)
        {
            var user = _pockyUserRepository.GetUser(message.Sender.UserId);
            if (!(user.HasRole(Roles.Admin) || user.HasRole(Roles.Config)))
            {
                return "Permission denied. You may only use the 'get' command.";
            }

            if (commands.Length != 3)
            {
                return "You must specify a location name to set.";
            }

            if (locations.Contains(commands[2], StringComparer.InvariantCultureIgnoreCase))
            {
                return $"Location value {commands[2]} has already been set.";
            }

            await _locationRepository.SetLocation(commands[2]);
            return "Location has been set.";
        }

        private async Task<string> DeleteLocation(Message message, string[] commands, string[] locations)
        {
            var user = _pockyUserRepository.GetUser(message.Sender.UserId);
            if (!(user.HasRole(Roles.Admin) || user.HasRole(Roles.Config)))
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
