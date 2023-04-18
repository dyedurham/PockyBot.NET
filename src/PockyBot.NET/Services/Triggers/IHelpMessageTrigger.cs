using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Services.Triggers
{
    internal interface IHelpMessageTrigger
    {
        string Command { get; }
        Role[] Permissions { get; }
        string GetHelpMessage(string botName, PockyUser user);
    }
}
