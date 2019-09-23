using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    public interface IPockyUserRepository
    {
        PockyUser GetUser(string userId);
        PockyUser AddOrUpdateUser(string userId, string username);
    }
}
