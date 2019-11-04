using System.Collections.Generic;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    public interface IConfigRepository
    {
        IList<GeneralConfig> GetAllGeneralConfig();
        int? GetGeneralConfig(string name);
        IList<StringConfig> GetAllStringConfig();
        IList<string> GetStringConfig(string name);
    }
}
