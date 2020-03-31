using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class NumberConfig : ITrigger
    {
        private readonly IConfigRepository _configRepository;

        public string Command => Commands.NumberConfig;

        public bool DirectMessageAllowed => false;

        public bool CanHaveArgs => true;

        public Role[] Permissions => new[] { Role.Admin, Role.Config };

        public NumberConfig(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public async Task<Message> Respond(Message message)
        {
            var command = string.Join("", message.MessageParts.Skip(1).Select(x => x.Text))
                .Trim().Remove(0, 4).Trim();
            var commandWords = command.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            if (commandWords.Count < 2)
            {
                return new Message
                {
                    Text = "Please specify a command. Possible values are get, set, and delete."
                };
            }

            string responseText;
            switch (commandWords[1])
            {
                case Actions.Get:
                    responseText = GetNumberConfigMessage();
                    break;
                case Actions.Set:
                    responseText = await SetNumberConfigMessage(commandWords).ConfigureAwait(false);
                    break;
                case Actions.Delete:
                    responseText = await DeleteNumberConfigMessage(commandWords).ConfigureAwait(false);
                    break;
                default:
                    responseText = "Unknown command. Possible values are get, set, and delete.";
                    break;
            }

            return new Message
            {
                Text = responseText
            };
        }

        private string GetNumberConfigMessage()
        {
            var allGeneralConfig = _configRepository.GetAllGeneralConfig();

            if (allGeneralConfig.Count == 0)
            {
                return "No number configs have been set.";
            }

            var numberConfigMessageBuilder = new StringBuilder("Here is the current config:\n\n");
            foreach (var config in allGeneralConfig)
            {
                numberConfigMessageBuilder.AppendLine($"* {config.Name}: {config.Value}");
            }

            return numberConfigMessageBuilder.ToString();
        }

        private async Task<string> SetNumberConfigMessage(List<string> commandWords)
        {
            if (commandWords.Count != 4)
            {
                return "You must specify a config name and value to set.";
            }

            if (!int.TryParse(commandWords[3], out int value))
            {
                return "Value must be set to a number.";
            }

            if (value < 0)
            {
                return "Value must be greater than or equal to zero.";
            }

            if (commandWords[2] == "minimum" && value > _configRepository.GetGeneralConfig("limit"))
            {
                return "Minimum pegs must be less than or equal to peg limit.";
            }

            await _configRepository.SetGeneralConfig(commandWords[2], value);
            return "Value has been set.";
        }

        private async Task<string> DeleteNumberConfigMessage(List<string> commandWords)
        {
            if (commandWords.Count != 3)
            {
                return "You must specify a config name to delete.";
            }

            if (!_configRepository.GetGeneralConfig(commandWords[2]).HasValue)
            {
                return $"Config value {commandWords[2]} does not exist.";
            }

            await _configRepository.DeleteGeneralConfig(commandWords[2]);
            return "Config has been deleted.";
        }
    }
}
