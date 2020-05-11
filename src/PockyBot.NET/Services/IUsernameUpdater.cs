using System.Collections.Generic;
using System.Threading.Tasks;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Services
{
    internal interface IUsernameUpdater
    {
        Task<List<PockyUser>> UpdateUsernames(List<PockyUser> users);
    }
}
