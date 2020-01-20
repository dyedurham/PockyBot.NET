using System.Collections.Generic;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    internal interface IConfigRepository
    {
        IList<GeneralConfig> GetAllGeneralConfig();
        int? GetGeneralConfig(string name);
        IList<StringConfig> GetAllStringConfig();
        IList<string> GetStringConfig(string name);
    }
}
