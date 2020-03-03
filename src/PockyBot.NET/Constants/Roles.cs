namespace PockyBot.NET.Constants
{
    internal static class
        Roles
    {
        public const string Unmetered = "UNMETERED";
        public const string Admin = "ADMIN";
        public const string Finish = "FINISH";
        public const string Reset = "RESET";
        public const string Config = "CONFIG";
        public const string RemoveUser = "REMOVEUSER";

        public static string[] All => new[] { Unmetered, Admin, Finish, Reset, Config, RemoveUser };
    }
}
