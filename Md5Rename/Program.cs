using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Md5Rename
{
    class Program
    {
        private static string _path = Directory.GetCurrentDirectory();
        private static PathType _pathType = PathType.Directory;
        private static bool _loop = false;

        static void Main(string[] args)
        {
            #region Set variables
            for (int i = 0; i < args.Length; i++)
                {
                    string arg = args[i];

                    if (arg.StartsWith("-"))
                    {
                        if (arg.Equals("--loop", StringComparison.OrdinalIgnoreCase))
                        {
                            _loop = true;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Unknown argument: \"{arg}\"");
                            return;
                        }
                    }
                    else
                    {
                        if (_path == null)
                        {
                            _path = arg;

                            if (File.Exists(_path))
                            {
                                _pathType = PathType.File;
                                _path = Path.GetFullPath(_path);
                            }
                            else if (Directory.Exists(_path))
                            {
                                _pathType = PathType.Directory;
                                _path = Path.GetFullPath(_path);
                            }
                            else
                                _pathType = PathType.Invalid;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid");
                            return;
                        }
                    }
                }
            #endregion

            if (_loop && _pathType != PathType.Directory)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The loop argument can only be used with directory.");
                return;
            }

            switch (_pathType)
            {
                case PathType.Invalid:
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Invalid path: \"{_path}\"");
                    }
                    return;
                case PathType.File:
                    {
                        HandleFile();
                    }
                    return;
                    case PathType.Directory:
                    {
                        HandlePath();
                    }
                    return;
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void HandlePath()
        {
            List<string> registerFiles = new List<string>();

            bool firstRun = true;
            while (firstRun || _loop)
            {
                List<string> files = new(Directory.GetFiles(_path));

                if (!files.Contains(Path.GetFullPath("DONT_MD5")))
                    foreach (var file in files)
                    {
                        if (File.GetLastWriteTime(file).AddSeconds(10) > DateTime.Now)
                            continue;

                        if (!registerFiles.Contains(file))
                        {
                            registerFiles.Add(HandleFile(file));
                            Console.WriteLine();
                        }
                    }
                else
                    throw new Exception("Found file \"DONT_MD5\", will do nothing");

                for (int i = registerFiles.Count - 1; i >= 0; i--)
                {
                    string file = registerFiles[i];
                    if (!File.Exists(file))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Lost file \"{file}\"");
                        Console.ForegroundColor = default;
                        registerFiles.Remove(file);
                    }
                }

                firstRun = false;

                if (_loop)
                    Thread.Sleep(1000);
            }
        }

        private static string HandleFile(string file = null)
        {
            if (file == null)
                file = _path;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"Target file: \"");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(file);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\"");

            string md5 = GetMd5(file);
            PrintValue(name: "MD5", value: md5);

            string ext = file.Substring(file.LastIndexOf('.') + 1);
            PrintValue("Extention", ext);

            string newName = $"{md5}.{ext}";
            PrintValue("New name", newName);

            string path = file.Substring(0, file.LastIndexOf(Path.DirectorySeparatorChar));
            PrintValue("path", path);

            string newPath = $"{path}{Path.DirectorySeparatorChar}{newName}";

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("File moved: ");

            if (file.ToUpper().Equals(newPath.ToUpper()))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Already named");

                if (!file.Equals(newPath))
                {
                    File.Move(file, $"{path}\\{newName}");
                    Console.Write(" (capitalized)");
                }

                Console.WriteLine();
                return newPath;
            }
            else if (File.Exists(newPath))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                File.Delete(file);
                Console.WriteLine("Duplicate");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                File.Move(file, newPath);
                Console.WriteLine("Renamed");
            }
            return newPath;
        }

        private static string GetMd5(string file)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant().ToUpper();
                }
            }
        }

        private static void PrintValue(string name, string value)
        {
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{name}: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(value);
            Console.ForegroundColor = foregroundColor;
        }

        private enum PathType
        {
            Invalid = 0,
            File = 1,
            Directory = 2
        }
    }
}
