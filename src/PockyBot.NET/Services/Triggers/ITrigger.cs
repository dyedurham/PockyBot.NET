using GlobalX.ChatBots.Core.Messages;

namespace PockyBot.NET.Services.Triggers
{
    internal interface ITrigger
    {
        string Command { get; }
        bool DirectMessageAllowed { get; }
        bool CanHaveArgs { get; }
        string[] Permissions { get; }
        Message Respond(Message message);
    }
}
