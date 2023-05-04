using System;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Services.Triggers
{
    internal class Ping : ITrigger, IHelpMessageTrigger
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

        public string GetHelpMessage(string botName, PockyUser user)
        {
            return "### How to ping me 🏓!\n" +
                   $"1. To check whether I'm alive, type: `@{botName} {Commands.Ping}` OR direct message me with `{Commands.Ping}`.\n" +
                   "1. I will respond in the room you messaged me in if I am alive.";
        }
    }
}
