using NpgsqlTypes;

namespace PockyBot.NET.Persistence.Models
{
    internal enum Role
    {
        [PgName("ADMIN")]
        Admin,
        [PgName("UNMETERED")]
        Unmetered,
        [PgName("RESULTS")]
        Results,
        [PgName("FINISH")]
        Finish,
        [PgName("RESET")]
        Reset,
        [PgName("UPDATE")]
        Update,
        [PgName("WINNERS")]
        Winners,
        [PgName("CONFIG")]
        Config,
        [PgName("USERLOCATION")]
        UserLocation,
        [PgName("REMOVEUSER")]
        RemoveUser
    }
}
