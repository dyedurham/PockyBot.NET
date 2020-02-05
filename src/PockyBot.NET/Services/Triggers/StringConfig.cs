using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class StringConfig : ITrigger
    {
        private readonly IConfigRepository _configRepository;
        public string Command => Commands.StringConfig;
        public bool DirectMessageAllowed => false;
        public bool CanHaveArgs => true;
        public string[] Permissions => new[] {Roles.Admin, Roles.Config};

        public StringConfig(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public async Task<Message> Respond(Message message)
        {
            var command = string.Join("", message.MessageParts.Skip(1).Select(x => x.Text)).Trim().Remove(0, 4).Trim();
            var commandWords = command.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            if (commandWords.Count() < 2)
            {
                return new Message
                {
                    Text = $"Please specify a command. Possible values are {string.Join(", ", ConfigActions.All())}."
                };
            }

            string newMessage;
            switch (commandWords[1])
            {
                case ConfigActions.Get:
                    newMessage = GetStringConfig();
                    break;
                case ConfigActions.Add:
                    newMessage = await AddStringConfig(commandWords);
                    break;
                case ConfigActions.Delete:
                    newMessage = await DeleteStringConfig(commandWords);
                    break;
                default:
                    newMessage = $"Invalid string config command. Possible values are {string.Join(", ", ConfigActions.All())}";
                    break;
            }

            return new Message
            {
                Text = newMessage
            };
        }

        private string GetStringConfig()
        {
            var stringConfig = _configRepository.GetAllStringConfig();
            var groupedConfig = stringConfig.GroupBy(x => x.Name);
            var message = "Here is the current config (**name:** value):\n";

            foreach (var grouping in groupedConfig)
            {
                message += $"* **{grouping.Key}:**\n";
                foreach (var config in grouping)
                {
                    message += $"    * {config.Value}\n";
                }
            }
            return message;
        }

        private async Task<string> AddStringConfig(List<string> commandWords)
        {
            if (commandWords.Count < 4) {
                return "You must specify a config name and value to add.";
            }

            if (_configRepository.GetStringConfig(commandWords[2]).Contains(commandWords[3], StringComparer.OrdinalIgnoreCase))
            {
                return $"String config name:value pair {commandWords[2]}:{commandWords[3]} already exists";
            }

            await _configRepository.AddStringConfig(commandWords[2].ToLower(), commandWords[3]);
            return "Config has been set";
        }

        private async Task<string> DeleteStringConfig(List<string> commandWords)
        {
            if (commandWords.Count < 4) {
                return "You must specify a config name and value to be deleted";
            }

            if (!_configRepository.GetStringConfig(commandWords[2]).Contains(commandWords[3], StringComparer.OrdinalIgnoreCase)) {
                return $"String config name:value pair {commandWords[2]}:{commandWords[3]} does not exist";
            }

            await _configRepository.DeleteStringConfig(commandWords[2].ToLower(), commandWords[3]);
            return "Config has been deleted";
        }
    }
}
