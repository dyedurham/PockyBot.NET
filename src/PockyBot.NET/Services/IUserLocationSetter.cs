using System.Threading.Tasks;

namespace PockyBot.NET.Services
{
    public interface IUserLocationSetter
    {
        Task<string> SetUserLocation(string[] commands, string[] mentionedUsers, bool userIsAdmin, string meId);
    }
}
