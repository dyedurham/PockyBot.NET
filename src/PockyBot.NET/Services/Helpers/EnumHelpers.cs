using System;
using System.Linq;

namespace PockyBot.NET.Services.Helpers
{
    internal static class EnumHelpers
    {
        public static bool IsEnumDefinedCaseInsensitive(Type enumType, string value)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type supplied must be enum");
            }

            return Enum.GetNames(enumType).Any(x => string.Equals(x, value, StringComparison.OrdinalIgnoreCase));
        }
    }
}
