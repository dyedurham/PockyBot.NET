using System.Collections.Generic;
using System.Threading.Tasks;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    internal interface IPockyUserRepository
    {
        PockyUser GetUser(string userId);
        List<PockyUser> GetUsersByUsername(string username);
        Task<PockyUser> AddOrUpdateUserAsync(string userId, string username);
        List<PockyUser> GetAllUsersWithPegs();
        Task RemoveUserAsync(PockyUser user);
        Task<List<PockyUser>> GetAllUserRolesAsync();
        Task AddRoleAsync(string userId, string role);
        Task RemoveRoleAsync(string userId, string role);
    }
}
