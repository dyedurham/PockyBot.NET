using GlobalX.ChatBots.Core.Messages;

namespace PockyBot.NET.Services.Pegs
{
    internal interface IPegRequestValidator
    {
        void ValidatePegRequest(Message message);
    }
}
