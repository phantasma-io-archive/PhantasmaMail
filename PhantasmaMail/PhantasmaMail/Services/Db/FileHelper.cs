using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PCLStorage;
using PhantasmaMail.Models;

namespace PhantasmaMail.Services.Db
{
    public static class FileHelper
    {
        public static string PhantasmaFolder = "phantasma";
        public static string DbFile = "db.json";
        public static async Task<bool> IsFileExistAsync(this string fileName, IFolder rootFolder = null)
        {
            // get hold of the file system  
            var folder = rootFolder ?? FileSystem.Current.LocalStorage;
            var folderexist = await folder.CheckExistsAsync(fileName);
            // already run at least once, don't overwrite what's there  
            if (folderexist == ExistenceCheckResult.FileExists) return true;
            return false;
        }

        public static async Task<bool> IsFolderExistAsync(this string folderName, IFolder rootFolder = null)
        {
            // get hold of the file system  
            var folder = rootFolder ?? FileSystem.Current.LocalStorage;
            var folderexist = await folder.CheckExistsAsync(folderName);
            // already run at least once, don't overwrite what's there  
            if (folderexist == ExistenceCheckResult.FolderExists) return true;
            return false;
        }

        public static async Task<IFolder> CreateFolder(this string folderName, IFolder rootFolder = null)
        {
            var folder = rootFolder ?? FileSystem.Current.LocalStorage;
            folder = await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
            return folder;
        }

        public static async Task<IFile> CreateFile(this string filename, IFolder rootFolder = null)
        {
            var folder = rootFolder ?? FileSystem.Current.LocalStorage;
            var file = await folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            return file;
        }

        public static async Task<bool> WriteTextAllAsync(this string filename, string content = "",
            IFolder rootFolder = null)
        {
            var file = await filename.CreateFile(rootFolder);
            await file.WriteAllTextAsync(content);
            return true;
        }

        public static async Task<string> ReadAllTextAsync(this string fileName, IFolder rootFolder = null)
        {
            var content = "";
            var folder = rootFolder ?? FileSystem.Current.LocalStorage;
            var exist = await fileName.IsFileExistAsync(folder);
            if (exist)
            {
                var file = await folder.GetFileAsync(fileName);
                content = await file.ReadAllTextAsync();
            }

            return content;
        }

        public static async Task UpdateMessages(string text)
        {
            var rootFolder = FileSystem.Current.LocalStorage;
            var folder = await rootFolder.GetFolderAsync(PhantasmaFolder);
            var list = JsonConvert.SerializeObject(AppSettings.SentMessages.ToList(), AppSettings.JsonSettings());
            if (!string.IsNullOrEmpty(list))
            {
                await DbFile.WriteTextAllAsync(list, folder);
            }
        }
    }
}