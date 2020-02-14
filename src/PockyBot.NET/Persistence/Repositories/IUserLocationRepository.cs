using System.Threading.Tasks;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    internal interface IUserLocationRepository
    {
        Task DeleteUserLocation(PockyUser user);
        Task DeleteUserLocation(string userId);
        Task AddUserLocation(PockyUser user, Location location);
    }
}
