using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RegisterNewClient.Infrastructure
{
    public interface IFileSystem
    {
        Task<string[]> ListDirectoriesAsync(string directoryPath);
        Task<string[]> ListFilesAsync(string directoryPath);
        Task<IEnumerable<string>> SearchFilesAsync(string directory, string searchPattern, bool recursive = true);
        StreamReader OpenFileToRead(string filePath);
        StreamWriter OpenFileToWrite(string filePath);
        bool FileExists(string filePath);
        string GetDatabasePath();
        string GetStoragePath();
        string GetJsonPath();
        string GetStoragePathToImage();

    }
}
