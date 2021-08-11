using System;
using System.IO;
using System.Security.Cryptography;

namespace GetCheckSum
{
    class Program
    {
        private static class argument
        {
            internal static string TargetFile = null;
            internal static bool ExeLocation = false;
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                WriteHelp();
                return;
            }

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i].ToLower();
                if (arg.StartsWith("-"))
                {
                    switch (arg)
                    {
                        case "--help":
                            WriteHelp();
                            return;
                        case "--exe-location":
                            Console.WriteLine($"EXE path: {Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}");
                            break;
                        default:
                            Console.WriteLine("Unknown argument: \"" + arg +  "\"");
                            WriteHelp();
                            return;
                    }
                }
                else
                {
                    if (File.Exists(arg))
                    {
                        argument.TargetFile = arg;
                    }
                }
            }

            if (!File.Exists(argument.TargetFile))
            {
                WriteHelp();
                return;
            }

            Console.Write("Input: ");
            string file;

            if (args.Length == 0)
                file = Console.ReadLine().Replace("\"", "");
            else
                file = args[0];

            Console.WriteLine($"\nFile..: {file.Substring(file.LastIndexOf('\\') + 1)}");

            FileStream stream = File.OpenRead(file);

            CalculateMd5(stream);

            CalculateSha1(stream);

            CalculateSha256(stream);

            stream.Close();
            stream.Dispose();

            Console.WriteLine();
        }

        private static void WriteHelp()
        {
            Console.WriteLine("args:\n" +
                "--help             Write arguments then terminate.\n" +
                "--exe-location     Write location of this program on execution.");
        }

        private static void CalculateSha256(FileStream stream)
        {
            SHA256Managed sha256Managed = new SHA256Managed();
            byte[] computeHashSha256 = sha256Managed.ComputeHash(stream);
            Console.WriteLine($"SHA256: {BitConverter.ToString(computeHashSha256).Replace("-", "")}");
            sha256Managed.Dispose();
        }

        private static void CalculateSha1(FileStream stream)
        {
            SHA1Managed sha1Managed = new SHA1Managed();
            byte[] computeHashSha1 = sha1Managed.ComputeHash(stream);
            Console.WriteLine($"SHA1..: {BitConverter.ToString(computeHashSha1).Replace("-", "")}");
            sha1Managed.Dispose();
        }

        private static void CalculateMd5(FileStream stream)
        {
            MD5 md5 = MD5.Create();
                byte[] md5Hash = md5.ComputeHash(stream);
                Console.WriteLine($"MD5...: {BitConverter.ToString(md5Hash).Replace("-", "")}");
            md5.Dispose();
        }
    }
}
