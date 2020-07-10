using System.Threading.Tasks;

namespace PockyBot.NET.Services.UserLocation
{
    public interface IUserLocationDeleter
    {
        Task<string> DeleteUserLocation(string[] commands, string[] mentionedUsers, bool userIsAdmin, string meId);
    }
}
