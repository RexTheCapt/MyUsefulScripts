using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GenerateSystemCheckReport
{
    public partial class FormMain : Form
    {
        private string _path = $"[PATH]";

        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonSelectOutput_Click(object sender, EventArgs e)
        {
            Properties.Settings settings = Properties.Settings.Default;

            LastSavedFileLocation lsfl = LastSavedFileLocation.Default;

            saveFileDialog1.Reset();
            saveFileDialog1.InitialDirectory = lsfl.Path;
            saveFileDialog1.FileName = lsfl.Filename;
            saveFileDialog1.Filter = "CSV (*.CSV)|*.CSV";
            DialogResult dialogResult = saveFileDialog1.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog1.OpenFile());

                bool writtenColNames = false;
                string[] files = Directory.GetFiles(_path).OrderBy(File.GetLastWriteTime).Reverse().Where(x=> !checkBox1.Checked || File.GetLastWriteTime(x) > settings.LastGeneration).ToArray();

                foreach (string file in files)
                {
                    StreamReader sr = new StreamReader(file);

                    List<ComputerInfo> computerInfos = new List<ComputerInfo>();

                    while (!sr.EndOfStream)
                    {
                        string streamRead = sr.ReadLine();
                        string[] strings = { $"{streamRead.Substring(0, streamRead.IndexOf(':'))}", $"{streamRead.Substring(streamRead.IndexOf(':') + 1)}" };
                        strings[0] = strings[0].Replace(".", "");

                        computerInfos.Add(new ComputerInfo(strings[0], strings[1]));
                    }

                    sr.Close();
                    sr.Dispose();

                    if (!writtenColNames)
                    {
                        sw.Write($"Age;");

                        foreach (ComputerInfo info in computerInfos)
                        {
                            sw.Write($"{info.Name};".Trim());
                        }

                        sw.WriteLine();
                        writtenColNames = true;
                    }

                    sw.Write($"{(DateTime.Now - File.GetLastWriteTime(file)).Days};");

                    foreach (ComputerInfo info in computerInfos)
                    {
                        sw.Write($"{info.Variable};".Trim());
                    }

                    sw.WriteLine();
                }

                sw.Flush();
                sw.Close();
                sw.Dispose();

                lsfl.Filename = saveFileDialog1.FileName;
                lsfl.Path = saveFileDialog1.InitialDirectory;
                lsfl.Save();
            }

            settings.LastGeneration = DateTime.Now;
            settings.Save();

            Environment.Exit(0);
        }

        private class ComputerInfo
        {
            public string Variable;
            public string Name;

            public ComputerInfo(string name, string variable)
            {
                Variable = variable;
                Name = name;
            }
        }
    }
}
