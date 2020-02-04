using System.Collections.Generic;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    internal interface IPockyUserRepository
    {
        PockyUser GetUser(string userId);
        PockyUser AddOrUpdateUser(string userId, string username);
        List<PockyUser> GetAllUsersWithPegs();
        List<PockyUser> GetAllUsersLocations();
    }
}
