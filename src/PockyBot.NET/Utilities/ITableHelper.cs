using System.Collections.Generic;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Utilities
{
    internal interface ITableHelper
    {
        List<int> GetStringConfigColumnWidths(IList<StringConfig> values);
        string PadString(string stringToPad, int length);
    }
}
