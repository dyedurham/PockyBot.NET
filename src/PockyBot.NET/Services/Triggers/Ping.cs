using System;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Services.Triggers
{
    internal class Ping : ITrigger
    {
        public string Command => Commands.Ping;

        public bool DirectMessageAllowed => true;

        public bool CanHaveArgs => false;

        public Role[] Permissions => Array.Empty<Role>();

        public Task<Message> Respond(Message message) {
            return Task.FromResult(new Message {
                Text = "pong. I'm alive!"
            });
        }
    }
}
