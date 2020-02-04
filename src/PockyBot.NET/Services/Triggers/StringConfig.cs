using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Utilities;

namespace PockyBot.NET.Services.Triggers
{
    internal class StringConfig : ITrigger
    {
        private readonly IConfigRepository _configRepository;
        private readonly ITableHelper _tableHelper;
        public string Command => Commands.StringConfig;
        public bool DirectMessageAllowed => false;
        public bool CanHaveArgs => true;
        public string[] Permissions => new[] {Roles.Admin, Roles.Config};

        public StringConfig(IConfigRepository configRepository, ITableHelper tableHelper)
        {
            _configRepository = configRepository;
            _tableHelper = tableHelper;
        }

        public async Task<Message> Respond(Message message)
        {
            var command = string.Join("", message.MessageParts.Skip(1).Select(x => x.Text)).Trim().Remove(0, 4).Trim();
            var commandWords = command.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            if (commandWords.Count() < 2)
            {
                return new Message
                {
                    Text = $"Please specify a command. Possible values are {ConfigActions.Get}, {ConfigActions.Set}, {ConfigActions.Refresh}, {ConfigActions.Delete}."
                };
            }

            var newMessage = "";
            switch (commandWords[1])
            {
                case ConfigActions.Get:
                    newMessage = GetStringConfig();
                    break;
                case ConfigActions.Set:
                    break;
                case ConfigActions.Refresh:
                    break;
                case ConfigActions.Delete:
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
            var columnWidths = _tableHelper.GetStringConfigColumnWidths(stringConfig);

            var message = "Here is the current config:\n```\n";

            message += _tableHelper.PadString("Name", columnWidths[0]) + " | Value\n";
            message += "".PadLeft(columnWidths[0], '-') + "-+-" + "".PadRight(columnWidths[1], '-') + "\n";

            foreach (var config in stringConfig)
            {
                message += config.Name.PadRight(columnWidths[0]) + " | " + config.Value + "\n";
            }

            message += "```";
            return message;
        }
    }
}
