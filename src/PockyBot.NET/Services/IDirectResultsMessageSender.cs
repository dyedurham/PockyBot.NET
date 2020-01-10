using System.Collections.Generic;
using System.Threading.Tasks;
using PockyBot.NET.Models;

namespace PockyBot.NET.Services
{
    internal interface IDirectResultsMessageSender
    {
        Task SendDirectMessagesAsync(List<PegRecipient> recipients);
    }
}
