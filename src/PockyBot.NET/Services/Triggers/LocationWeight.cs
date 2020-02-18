using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class LocationWeight : ITrigger
    {
        private readonly IConfigRepository _configRepository;
        private readonly ILocationRepository _locationRepository;

        public string Command => Commands.LocationWeight;

        public bool DirectMessageAllowed => false;

        public bool CanHaveArgs => true;

        public string[] Permissions => new[] { Roles.Admin, Roles.Config };

        public LocationWeight(IConfigRepository configRepository, ILocationRepository locationRepository)
        {
            _configRepository = configRepository;
            _locationRepository = locationRepository;
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
                    Text = "Please specify a command. Possible values are get, set, and delete."
                };
            }

            var locations = _locationRepository.GetAllLocations();
            string response;

            switch (commands[1].ToLower(CultureInfo.InvariantCulture))
            {
                case Actions.Get:
                    response = GetLocationWeightMessage(locations);
                    break;
                case Actions.Set:
                    response = await SetLocationWeight(commands, locations).ConfigureAwait(false);
                    break;
                case Actions.Delete:
                    response = await DeleteLocationWeight(commands, locations).ConfigureAwait(false);
                    break;
                default:
                    response = "Unknown command. Possible values are get, set, and delete.";
                    break;
            }

            return new Message
            {
                Text = response
            };
        }

        private string GetLocationWeightMessage(string[] locations)
        {
            var allConfigs = _configRepository.GetAllGeneralConfig();
            var locationWeights = new List<string>();

            for (var i = 0; i < locations.Length; i++)
            {
                for (var j = i + 1; j < locations.Length; j++)
                {
                    var config = GetLocationWeightConfig(locations[i], locations[j], allConfigs);
                    if (config != null)
                    {
                        locationWeights.Add($"{locations[i]} <-> {locations[j]}: {config.Value}");
                    }
                }
            }

            if (locationWeights.Count == 0)
            {
                return "No location weights set.";
            }

            return $"Here are the current location weights:\n\n* {string.Join("\n* ", locationWeights)}";
        }

        private GeneralConfig GetLocationWeightConfig(string location1, string location2, IList<GeneralConfig> allConfigs)
        {
            var senderToReceiver = $"locationWeight{location1}to{location2}".ToLower(CultureInfo.InvariantCulture);
            var receiverToSender = $"locationWeight{location2}to{location1}".ToLower(CultureInfo.InvariantCulture);

            var config = allConfigs.FirstOrDefault(x => x.Name.ToLower() == senderToReceiver || x.Name.ToLower() == receiverToSender);

            return config;
        }

        private async Task<string> SetLocationWeight(string[] commands, string[] locations)
        {
            if (commands.Length != 5)
            {
                return "Please specify two locations and a weight.";
            }

            if (!locations.Contains(commands[2], StringComparer.InvariantCultureIgnoreCase))
            {
                return $"Location value \"{commands[2]}\" is invalid.";
            }

            if (!locations.Contains(commands[3], StringComparer.InvariantCultureIgnoreCase))
            {
                return $"Location value \"{commands[3]}\" is invalid.";
            }

            var isValidInteger = int.TryParse(commands[4], out int desiredValue);

            if (!isValidInteger)
            {
                return "Weight must be set to a whole number.";
            }

            if (desiredValue < 0)
            {
                return "Weight must be greater than or equal to zero.";
            }

            var configName = $"locationWeight{commands[2]}to{commands[3]}";
            var existingConfig =
                GetLocationWeightConfig(commands[2], commands[3], _configRepository.GetAllGeneralConfig());
            if (existingConfig != null)
            {
                configName = existingConfig.Name;
            }

            await _configRepository.SetGeneralConfig(configName, desiredValue);
            return "Location weight has been set.";
        }

        private async Task<string> DeleteLocationWeight(string[] commands, string[] locations)
        {
            if (commands.Length != 4)
            {
                return "You must specify a two location names to delete the weighting for.";
            }

            if (!locations.Contains(commands[2], StringComparer.InvariantCultureIgnoreCase))
            {
                return $"Location value \"{commands[2]}\" is invalid.";
            }

            if (!locations.Contains(commands[3], StringComparer.InvariantCultureIgnoreCase))
            {
                return $"Location value \"{commands[3]}\" is invalid.";
            }

            var existingConfig =
                GetLocationWeightConfig(commands[2], commands[3], _configRepository.GetAllGeneralConfig());
            if (existingConfig == null)
            {
                return $"No weighting found for locations {commands[2]} and {commands[3]}.";
            }

            await _configRepository.DeleteGeneralConfig(existingConfig);
            return "Location weight has been deleted.";
        }
    }
}
