using System.Collections.Generic;
using PockyBot.NET.Models;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Services.Pegs
{
    internal interface IPegResultsHelper
    {
        List<PegCategory> GetCategories(List<PegRecipient> allRecipients);
        List<PegRecipient> GetWinners(List<PegRecipient> allRecipients);
        List<PegRecipient> MapUsersToPegRecipients(List<PockyUser> users);
    }
}
