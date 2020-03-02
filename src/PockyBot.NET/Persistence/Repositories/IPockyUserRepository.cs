using System.Collections.Generic;
using System.Threading.Tasks;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    internal interface IPockyUserRepository
    {
        PockyUser GetUser(string userId);
        List<PockyUser> GetUsersByUsername(string username);
        Task<PockyUser> AddOrUpdateUser(string userId, string username);
        List<PockyUser> GetAllUsersWithPegs();
        Task RemoveUser(PockyUser user);
    }
}
