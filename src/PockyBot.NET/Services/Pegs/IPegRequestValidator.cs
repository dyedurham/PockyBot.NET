using GlobalX.ChatBots.Core.Messages;

namespace PockyBot.NET.Services.Pegs
{
    internal interface IPegRequestValidator
    {
        bool ValidatePegRequest(Message message, out string errorMessage);
    }
}
