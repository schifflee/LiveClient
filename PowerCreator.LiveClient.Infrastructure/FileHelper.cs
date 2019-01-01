using System;
using System.Collections.Generic;
using System.IO;

namespace PowerCreator.LiveClient.Infrastructure
{
    public static class FileHelper
    {
        public static IEnumerable<string> ReadFileContent(string filePath, string fileName)
        {
            return File.ReadAllLines(Path.Combine(filePath, fileName));
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
    }
}
