using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RegisterNewClient.Infrastructure;

namespace RegisterNewClient.Droid.PlatformCode
{
    class AndroidFileSystem : IFileSystem
    {
        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string GetDatabasePath()
        {
            var dbFolderName = "Registry";
            var dbFileName = "Registry.db";


            var externalStorageDirectory = GetStoragePath();
            var absoluteDatabaseDirectory = Path.Combine(externalStorageDirectory, dbFolderName);

            if (!Directory.Exists(absoluteDatabaseDirectory))
                Directory.CreateDirectory(absoluteDatabaseDirectory);

            var absoluteDatabaseFileName = Path.Combine(absoluteDatabaseDirectory, dbFileName);

            //if (FileExists(absoluteDatabaseFileName))
                //File.Delete(absoluteDatabaseFileName);

            return absoluteDatabaseFileName;

        }

        public string GetJsonPath()
        {
            var dbFolderName = "Registry";
            var jsonFileName = "Registry.json";


            var externalStorageDirectory = GetStoragePath();
            var absoluteDatabaseDirectory = Path.Combine(externalStorageDirectory, dbFolderName);

            if (!Directory.Exists(absoluteDatabaseDirectory))
                Directory.CreateDirectory(absoluteDatabaseDirectory);

            var absoluteDatabaseFileName = Path.Combine(absoluteDatabaseDirectory, jsonFileName);

            //if (FileExists(absoluteDatabaseFileName))
            //File.Delete(absoluteDatabaseFileName);

            return absoluteDatabaseFileName;
        }

        public string GetStoragePath()
        {

            return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
        }

        public string GetStoragePathToImage()
        {
            var dbFolderName = "Registry";
            var imageFolderName = "Image";
            var externalStorageDirectory = GetStoragePath();
            var absoluteDatabaseDirectory = Path.Combine(externalStorageDirectory, dbFolderName,imageFolderName);
            if (!Directory.Exists(absoluteDatabaseDirectory))
                Directory.CreateDirectory(absoluteDatabaseDirectory);
            return absoluteDatabaseDirectory; 
        }

        public Task<string[]> ListDirectoriesAsync(string directoryPath)
        {
            return Task.Factory.StartNew(delegate
            {
                return Directory.GetDirectories(directoryPath);
            });
        }

        public Task<string[]> ListFilesAsync(string directoryPath)
        {
            return Task.Factory.StartNew(delegate
            {
                return Directory.GetFiles(directoryPath);
            });
        }

        public StreamReader OpenFileToRead(string filePath)
        {
            return new StreamReader(File.OpenRead(filePath));
        }

        public StreamWriter OpenFileToWrite(string filePath)
        {
            return new StreamWriter(File.OpenWrite(filePath));
        }

        public Task<IEnumerable<string>> SearchFilesAsync(string directory, string searchPattern, bool recursive = true)
        {
            return Task.Factory.StartNew(delegate
            {
                if (recursive)
                    return Directory.EnumerateFiles(directory, searchPattern, SearchOption.AllDirectories);
                return Directory.EnumerateFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);
            });
        }
    }
}