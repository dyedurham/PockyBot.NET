namespace PockyBot.NET.Services.UserLocation
{
    public interface IUserLocationGetter
    {
        string GetUserLocation(string[] commands, string[] mentionedUsers, string meId);
    }
}
