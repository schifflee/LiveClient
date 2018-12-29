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
            File.WriteAllLines(Path.Combine(filePath, fileName), contents);
        }
        public static void DeleteFile(string filePath, string fileName)
        {
            File.Delete(Path.Combine(filePath, fileName));
        }
    }
}
