using System.Collections.Generic;
using System.Threading.Tasks;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    internal interface IPockyUserRepository
    {
        PockyUser GetUser(string userId);
        List<PockyUser> GetUsersByUsername(string username);
        PockyUser AddOrUpdateUser(string userId, string username);
        List<PockyUser> GetAllUsersWithPegs();
        List<PockyUser> GetAllUsersLocations();
        Task RemoveUser(PockyUser user);
    }
}
