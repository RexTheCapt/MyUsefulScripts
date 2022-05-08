using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteEmptyDirectories
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            DeleteEmptyDirsInDirectory(Directory.GetCurrentDirectory());
            //DeleteEmptyDirsInDirectory(@"D:\Pictures\");
        }

        private static void DeleteEmptyDirsInDirectory(string dir)
        {
            foreach (string path in Directory.GetDirectories(dir))
            {
                if (!PathHasContent(path))
                {
                    FileAttributes attr = File.GetAttributes(path);

                    if (!attr.HasFlag(FileAttributes.Directory)) throw new Exception("Non directory path got marked for deletion!");

                    Console.WriteLine($"Removing {path}");
                    Directory.Delete(path, false);
                }
            }
        }

        private static bool PathHasContent(string path)
        {
            string[] files = Directory.GetFiles(path);

            foreach (var dir in Directory.GetDirectories(path))
            {
                if (!PathHasContent(dir)) DeleteEmptyDirsInDirectory(path);
                else files = new string[] { dir };
            }

            //if (files.Length != 0) return true;
            return files.Length != 0;

            //return false;
        }
    }
}
