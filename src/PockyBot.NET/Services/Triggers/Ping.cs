using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Options;
using PockyBot.NET.Configuration;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class Ping : Trigger
    {
        public Ping(IOptions<PockyBotSettings> settings, IPockyUserRepository pockyUserRepository) : base(settings, pockyUserRepository) { }

        protected override string Command => Commands.Ping;

        protected override bool DirectMessageAllowed => true;

        protected override bool CanHaveArgs => false;

        protected override bool PermissionsRequired => false;

        public override Message Respond(Message message) {
            return new Message {
                Text = "pong. I'm alive!"
            };
        }
    }
}
