using ETicaretAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services
{
    public class FileService
    {
        async Task<string> FileRenameAsync(string path, string fileName)
        {
            return await Task.Run(() =>
            {
                string extension = Path.GetExtension(fileName);
                string baseName = Path.GetFileNameWithoutExtension(fileName);
                string newFileName = $"{FileNameOperation.CharacterRegulatory(baseName)}{extension}";

                int counter = 1;
                while (File.Exists(Path.Combine(path, newFileName)))
                {
                    newFileName = $"{FileNameOperation.CharacterRegulatory(baseName)}-{counter}{extension}";
                    counter++;
                }
                return newFileName;
            });
        }
    }
}
