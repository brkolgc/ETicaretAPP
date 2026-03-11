using ETicaretAPI.Infrastructure.Operations;

namespace ETicaretAPI.Infrastructure.Services.Storage
{
    public class Storage
    {
        protected delegate bool HasFile(string pathOrContainerName, string fileName);
        protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFile hasFileMethod)
        {
            return await Task.Run(() =>
            {
                string extension = Path.GetExtension(fileName);
                string baseName = Path.GetFileNameWithoutExtension(fileName);
                string newFileName = $"{FileNameOperation.CharacterRegulatory(baseName)}{extension}";

                int counter = 1;
                //while (File.Exists(Path.Combine(pathOrContainerName, newFileName)))
                //{
                //    newFileName = $"{FileNameOperation.CharacterRegulatory(baseName)}-{counter}{extension}";
                //    counter++;
                //}

                //azure, aws, localstorage gibi yapılanmaların her birinde dosya varlık kontrolü farklı olacağı için delegate ile ilgili metotu alıyorum
                while (hasFileMethod(pathOrContainerName, newFileName))
                {
                    newFileName = $"{FileNameOperation.CharacterRegulatory(baseName)}-{counter}{extension}";
                    counter++;
                }
                return newFileName;
            });
        }
    }
}
