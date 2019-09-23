using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;

namespace PockyBot.NET.Services.Triggers
{
    internal class Ping : ITrigger
    {
        public string Command => Commands.Ping;

        public bool DirectMessageAllowed => true;

        public bool CanHaveArgs => false;

        public string[] Permissions => new string[0];

        public Message Respond(Message message) {
            return new Message {
                Text = "pong. I'm alive!"
            };
        }
    }
}
