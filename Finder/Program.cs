using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finder
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Console.WriteLine("Please input file path: ");
            string path = Console.ReadLine().Replace("\"", "").Trim();
            Console.WriteLine("Please input search text: ");
            string searchText = Console.ReadLine().Trim();

            List<string> list = new List<string>();
            using (StreamReader sr = new(path))
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.Contains(searchText))
                        list.Add(line);
                }

            using (StreamWriter sw = new("output.txt"))
                foreach(string s in list)
                    sw.WriteLine(s);

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = Path.GetFullPath("output.txt")
            };
            Process.Start(psi);
        }
    }
}
