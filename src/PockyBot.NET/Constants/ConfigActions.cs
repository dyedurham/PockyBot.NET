using System.Collections.Generic;

namespace PockyBot.NET.Constants
{
    internal static class ConfigActions
    {
        public const string Get = "get";
        public const string Add = "saddet";
        public const string Delete = "delete";

        public static string[] All()
        {
            return new string[]
            {
                Get, Add, Delete
            };
        }
    }
}
