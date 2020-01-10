using System.Collections.Generic;
using PockyBot.NET.Models;

namespace PockyBot.NET.Services
{
    internal interface IDirectResultsMessageSender
    {
        void SendDirectMessages(List<PegRecipient> recipients);
    }
}