using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageDifference
{
    public partial class FormMain : Form
    {
        private string[] _allFiles;
        private List<bool> _hashCurrent = new List<bool>();

        private readonly int _Size = 128;
        private int _current = -1;
        private int _check;

        private Timer _autoCheckTimer = null;

        public FormMain()
        {
            InitializeComponent();

            FormSetPath fsp = new FormSetPath();
            fsp.ShowDialog();

            if (fsp.DialogResult == DialogResult.OK)
            {
                List<string> files = new List<string>();

                foreach(string s in fsp.path)
                {
                    files.AddRange(GetFiles(s));
                }

                _allFiles = files.ToArray();

                numericUpDownCheck.Maximum = numericUpDownCurrent.Maximum = _allFiles.Count();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private List<bool> GetHash(Bitmap bmpSource)
        {
            try
            {
                List<bool> lResult = new List<bool>();

                Bitmap bmpMin = new Bitmap(bmpSource, new Size(_Size, _Size));

                for (int j = 0; j < bmpMin.Height; j++)
                    for (int i = 0; i < bmpMin.Width; i++)
                    {
                        lResult.Add(bmpMin.GetPixel(i, j).GetBrightness() < 0.5f);
                    }

                return lResult;
            }
            catch
            {
                return new List<bool>();
            }
        }

        private string[] GetFiles(string path)
        {
            List<string> files = new List<string>();

            files.AddRange(Directory.GetFiles(path));

            foreach(string s in Directory.GetDirectories(path))
            {
                files.AddRange(GetFiles(s));
            }

            return files.ToArray();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
        }

        private Image ImageHash(List<bool> hash)
        {
            Bitmap b1 = new Bitmap(_Size, _Size);

            for (int i = 0; i < hash.Count; i++)
            {
                b1.SetPixel(i / _Size, i % _Size, hash[i] ? Color.White : Color.Black);
            }

            return b1;
        }

        private Image GetNextImage(int startIndex, out int endIndex)
        {
            for (int i = startIndex; i < _allFiles.Length; i++)
            {
                string s = _allFiles[i].ToUpper();
                if (s.EndsWith(".GIF") || s.EndsWith(".lnk".ToUpper()))
                    continue;

                endIndex = i;

                return Bitmap.FromFile(_allFiles[i]);
            }

            throw new Exception("No valid files found");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckImage();
        }

        private decimal CheckImage()
        {
            Retry:
            if(_check >= _allFiles.Length)
            {
                pictureBoxCurrent.Image?.Dispose();
            }

            if(pictureBoxCurrent.Image == null)
            {
                pictureBoxCurrent.Image = GetNextImage(++_current, out _check);
                _hashCurrent = GetHash((Bitmap)pictureBoxCurrent.Image);
                pictureBoxCurrent16.Image = ImageHash(_hashCurrent);
                label1.Text = $"{_current + 1}/{_allFiles.Length}";
            }

            try
            {
                pictureBoxCheck.Image?.Dispose();
                pictureBoxCheck.Image = GetNextImage(++_check, out _check);
                label2.Text = $"{_check + 1}/{_allFiles.Length}";
            }
            catch (Exception ee)
            {
                if (ee.Message == "No valid files found")
                {
                    pictureBoxCurrent.Image.Dispose();
                    pictureBoxCurrent.Image = null;
                    goto Retry;
                }
            }

            if (!File.Exists(_allFiles[_current]) || !File.Exists(_allFiles[_check]))
                return -1;

            Update();
            List<bool> hash = GetHash((Bitmap)pictureBoxCheck.Image);

            pictureBoxCheck16.Image = ImageHash(hash);
            
            decimal likelyness = (_hashCurrent.Zip(hash, (i, j) => i == j).Count(eq => eq) / (decimal)(_Size * _Size));
            
            buttonManual.Text = (likelyness * 100).ToString("000") + "%";

            Update();

            return likelyness;
        }

        private void buttonAuto_Click(object sender, EventArgs e)
        {
            if(_autoCheckTimer == null)
            {
                _autoCheckTimer = new Timer
                {
                    Interval = 1,
                    Enabled = false
                };

                _autoCheckTimer.Tick += _autoCheckTimer_Tick;

                _autoCheckTimer.Enabled = true;
            }
            else
            {
                _autoCheckTimer.Dispose();
                _autoCheckTimer = null;
            }
        }

        private void _autoCheckTimer_Tick(object sender, EventArgs e)
        {
            _autoCheckTimer.Enabled = false;
            
            if(CheckImage() >= numericUpDownPercent.Value / 100)
            {
                if(!Directory.Exists("similars"))
                {
                    Directory.CreateDirectory("similars");
                }

                if(!Directory.Exists($"similars\\{_current} - {_check}"))
                {
                    Directory.CreateDirectory($"similars\\{_current} - {_check}");

                    string current = _allFiles[_current];
                    string check = _allFiles[_check];

                    File.Copy(current, $"similars\\{_current} - {_check}\\current{current.Substring(current.LastIndexOf('.'))}");
                    File.Copy(check, $"similars\\{_current} - {_check}\\check{check.Substring(check.LastIndexOf('.'))}");

                    StreamWriter sw = new StreamWriter($"similars\\{_current} - {_check}\\paths.txt");

                    sw.WriteLine($"Current: {current}");
                    sw.WriteLine($"Check:   {check}");

                    sw.Flush();
                    sw.Close();
                    sw.Dispose();

                    if (checkBox1.Checked)
                        Process.Start($"similars\\");
                }

                _autoCheckTimer.Dispose();
            }
            
            _autoCheckTimer.Enabled = true;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void buttonSkip_Click(object sender, EventArgs e)
        {
            pictureBoxCurrent.Image.Dispose();
            pictureBoxCurrent.Image = null;
        }

        private void SelectionIndex(object sender, EventArgs e)
        {
            pictureBoxCheck.Image?.Dispose();
            pictureBoxCurrent.Image?.Dispose();
            pictureBoxCheck16.Image?.Dispose();
            pictureBoxCurrent16.Image?.Dispose();

            pictureBoxCurrent.Image = Image.FromFile(_allFiles[(int)numericUpDownCurrent.Value]);
            pictureBoxCheck.Image = Image.FromFile(_allFiles[(int)numericUpDownCheck.Value]);

            _hashCurrent = GetHash((Bitmap)pictureBoxCurrent.Image);
            _current = (int)numericUpDownCurrent.Value;
            List<bool> hash = GetHash((Bitmap)pictureBoxCheck.Image);
            _check = (int)numericUpDownCheck.Value;

            pictureBoxCurrent16.Image = ImageHash(_hashCurrent);
            pictureBoxCheck16.Image = ImageHash(hash);

            float likelyness = (_hashCurrent.Zip(hash, (i, j) => i == j).Count(eq => eq) / (float)(_Size * _Size));
            buttonManual.Text = $"{likelyness * 100}%";
        }

        private void numericUpDownPercent_ValueChanged(object sender, EventArgs e)
        {
            foreach (string dir in Directory.GetDirectories("similars"))
            {
                if (dir.StartsWith($"similars\\{_current} -"))
                {
                    Directory.Delete(dir, true);
                    _check = _current + 1;
                }
            }
        }
    }
}
