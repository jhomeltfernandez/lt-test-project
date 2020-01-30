using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LTTestProject.Business.Services
{
    public class FileService : IFileService
    {
        public async Task<string> UploadFileToFolder(IFormFile ufile, string destFolder)
        {
            if (ufile != null && ufile.Length > 0)
            {
                var fileName = GenerateFileName(Path.GetFileName(ufile.FileName));
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), destFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ufile.CopyToAsync(fileStream);
                }
                return $"{destFolder}/{fileName}".Replace("wwwroot\\","").Replace("\\", "/");
            }

            return string.Empty;
        }

        private string GenerateFileName(string fileName)
        {
            string strFileName = string.Empty;
            string[] strName = fileName.Split('.');
            strFileName = Guid.NewGuid().ToString() + "." + strName[strName.Length - 1];
            return strFileName;
        }
    }
}
