using System.Threading.Tasks;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    internal interface IUserLocationRepository
    {
        Task DeleteUserLocation(string userId);
        Task UpsertUserLocation(PockyUser user, string location);
    }
}
