using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class Rotation : ITrigger
    {
        private readonly IConfigRepository _configRepository;
        public string Command => Commands.Rotation;
        public bool DirectMessageAllowed => true;
        public bool CanHaveArgs => false;
        public Role[] Permissions => Array.Empty<Role>();

        public Rotation(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public Task<Message> Respond(Message message)
        {
            var rotation = _configRepository.GetStringConfig("rotation").First();
            var builder = new StringBuilder("## Here's the snack buying rotation:\n\n");

            foreach (var item in rotation.Split(','))
            {
                builder.AppendLine($"* {item}");
            }

            return Task.FromResult(new Message
            {
                Text = builder.ToString()
            });
        }
    }
}
