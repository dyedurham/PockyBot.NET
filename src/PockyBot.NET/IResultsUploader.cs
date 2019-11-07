using System.Threading.Tasks;

namespace PockyBot.NET
{
    public interface IResultsUploader
    {
        Task<string> UploadResults(string fileContent);
    }
}
