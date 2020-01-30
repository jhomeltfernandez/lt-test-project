using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LTTestProject.Business.Services
{
    public interface IFileService
    {
        Task<string> UploadFileToFolder(IFormFile ufile, string destFolder);
    }
}