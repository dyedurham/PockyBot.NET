using System.Threading.Tasks;

namespace PockyBot.NET.Persistence.Repositories
{
    internal interface ILocationRepository
    {
        Task DeleteLocation(string location);
        string[] GetAllLocations();
        Task AddLocation(string location);
    }
}
