using System.Threading.Tasks;

namespace PockyBot.NET.Services
{
    internal interface IUserLocationService
    {
        string GetUserLocation(string[] commands, string[] mentionedUsers, string meId);
        Task<string> SetUserLocation(string[] commands, string[] mentionedUsers, bool userIsAdmin, string meId);
        Task<string> DeleteUserLocation(string[] commands, string[] mentionedUsers, bool userIsAdmin, string meId);
    }
}
