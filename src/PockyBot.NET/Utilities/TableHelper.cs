using System;
using System.Collections.Generic;
using System.Linq;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Utilities
{
    internal class TableHelper : ITableHelper
    {
        public List<int> GetStringConfigColumnWidths(IList<StringConfig> values)
        {
            return new List<int>
            {
                GetColumnWidth(values.Select(x => x.Name), "Name"),
                GetColumnWidth(values.Select(x => x.Value), "Value")
            };
        }

        private static int GetColumnWidth(IEnumerable<string> values, string columnName)
        {
            var maxValueLength = values.Select(x => x.Length).Max();
            return Math.Max(maxValueLength, columnName.Length);
        }

        public string PadString(string stringToPad, int length) {
            var a = (length / 2) - (stringToPad.Length / 2);
            return stringToPad.PadRight(a + stringToPad.Length).PadLeft(length);
        }
    }
}
