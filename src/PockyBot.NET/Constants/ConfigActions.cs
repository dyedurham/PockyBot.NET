namespace PockyBot.NET.Constants
{
    internal static class ConfigActions
    {
        public const string Get = "get";
        public const string Add = "add";
        public const string Delete = "delete";

        public static string[] All() => new[]
        {
            Get, Add, Delete
        };
    }
}
