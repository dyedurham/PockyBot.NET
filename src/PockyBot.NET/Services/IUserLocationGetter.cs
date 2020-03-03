namespace PockyBot.NET.Services
{
    public interface IUserLocationGetter
    {
        string GetUserLocation(string[] commands, string[] mentionedUsers, string meId);
    }
}
