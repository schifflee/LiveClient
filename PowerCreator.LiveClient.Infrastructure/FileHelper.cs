using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PowerCreator.LiveClient.Infrastructure
{
    public static class FileHelper
    {
        public static void SaveJsonFile<T>(string filePath, string fileName, params T[] data)
        {
            if (!data.Any()) return;

            ICollection<string> contents = new List<string>();
            foreach (var item in data)
            {
                contents.Add(JsonHelper.SerializeObject(item));
            }
            DeleteFile(filePath, fileName);
            WriteAllLines(filePath, fileName, contents);

        }
        public static IEnumerable<string> ReadFileContent(string filePath, string fileName)
        {
            string fileFullPath = Path.Combine(filePath, fileName);
            if (FileExists(fileFullPath))
                return File.ReadAllLines(Path.Combine(filePath, fileName));
            else return Enumerable.Empty<string>();
        }
        public static void WriteAllLines(string filePath, string fileName, IEnumerable<string> contents)
        {
            CreateDirectory(filePath);
            File.WriteAllLines(Path.Combine(filePath, fileName), contents);
        }
        public static void DeleteFile(string filePath, string fileName)
        {
            string fileFullPath = Path.Combine(filePath, fileName);
            if (FileExists(fileFullPath))
                File.Delete(fileFullPath);
        }

        public static void CreateDirectory(string path)
        {
            if (!DirectoryExists(path))
                Directory.CreateDirectory(path);
        }
        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }
        public static bool FileExists(string fileFullPath)
        {
            return File.Exists(fileFullPath);
        }
        public static long GetFileSize(string filePath)
        {
            if (!File.Exists(filePath)) return 0;

            return new FileInfo(filePath).Length;
        }
    }
}
