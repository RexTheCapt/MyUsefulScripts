using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace BatteryTest
{
    public partial class FormMain : Form
    {
        private bool _overrideCharging => CreateMD5(Environment.UserName) == "3024AD1858C2954549360F5576E02A20";

        private List<int> _levelChangeList = new List<int>();
        private DateTime _lastCheckDateTime;
        private bool _overideTimeCheck;
        private bool _firstRemoved;
        private int _lastPercentage;

        private DateTime _startDateTime;
        private DateTime _endDateTime;

        private Color _defaultButtonColor;
        private FileInfo _resultFileInfo = new FileInfo("X:\\Tools\\BatteryTest",$"{Environment.MachineName}.txt");

        public FormMain()
        {
            InitializeComponent();

            _lastCheckDateTime = DateTime.MinValue;

            _defaultButtonColor = button1.BackColor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _endDateTime = (_startDateTime = DateTime.Now).AddHours(1);
            _levelChangeList.Clear();
            _overideTimeCheck = true;
            _lastCheckDateTime = DateTime.Now;
            timerCheckBatteryChange_Tick(timerCheckBatteryChange, EventArgs.Empty);
            timerCheckBatteryChange.Enabled = true;
            label1.Text = "Started";
            timerFlashFinished.Enabled = false;
            button1.BackColor = _defaultButtonColor;
            _firstRemoved = false;
        }

        private void timerCheckBatteryChange_Tick(object sender, EventArgs e)
        {
            if ((SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Offline || _overrideCharging) && (_lastCheckDateTime.Minute != DateTime.Now.Minute || _overideTimeCheck))
            {
                label1.Text = $"Start time: {_startDateTime}, battery level: {SystemInformation.PowerStatus.BatteryLifePercent * 100}";

                _overideTimeCheck = false;

                DateTime currentDateTime = DateTime.Now;
                int currentPercentage = (int) (SystemInformation.PowerStatus.BatteryLifePercent * 100);

                _lastCheckDateTime = currentDateTime;

                if(_levelChangeList.Count == 0)
                {
                    _levelChangeList.Add(0);
                }
                else
                {
                    _levelChangeList.Add(currentPercentage - _lastPercentage);

                    if (!_firstRemoved)
                    {
                        _firstRemoved = true;
                        _levelChangeList.RemoveAt(0);
                    }
                }

                int totalChange = 0;

                foreach (int i in _levelChangeList)
                {
                    totalChange += i;
                }

                if(totalChange != 0 && _levelChangeList.Count != 0)
                {
                    float avg = (float) totalChange / _levelChangeList.Count;
                    int minutes = (int)(100 / (0 - avg));
                    TimeSpan timeSpan = new TimeSpan(0, minutes, 0);
                    button1.Text = $"{avg}%/m, estimated time: {timeSpan}";
                }
                else
                {
                    button1.Text = $"[N/A]%/m, estimated time: [N/A]";
                }

                _lastPercentage = currentPercentage;
            }
            else if (SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Online && !_overrideCharging)
            {
                _levelChangeList.Clear();
                button1.Text = $"Unplug power source at 50%";
                label1.Text = "Computer is charging... [test not running]";
                _endDateTime = (_startDateTime = DateTime.Now).AddHours(1);

                if (SystemInformation.PowerStatus.BatteryLifePercent * 100 > 50f)
                {
                    label1.Text = button1.Text = "Enough charge to start, please unplug power source";
                    Console.Beep(500, 100);
                }
            }

            if (DateTime.Now > _endDateTime && (_levelChangeList.Count >= 60 || _overrideCharging))
            {
                _endDateTime = DateTime.Now;

                ((Timer) sender).Enabled = false;

                if (!Directory.Exists(_resultFileInfo.Path))
                    Directory.CreateDirectory(_resultFileInfo.Path);
                
                StreamWriter sw = new StreamWriter(_resultFileInfo.ToString());

                label1.Text = "Testing is done";

                int total = 0;

                foreach (int i in _levelChangeList)
                {
                    total += i;
                }

                float avg = (float) total / _levelChangeList.Count;
                int minutes = (int)(100 / (0 - avg));
                TimeSpan est = new TimeSpan(0,minutes, 0);

                sw.WriteLine($"Results of battery testing:\n" +
                             $"Computer name:  {Environment.MachineName}\n" +
                             $"Start time:     {_startDateTime}\n" +
                             $"End time:       {_endDateTime}\n" +
                             $"Avg per min:    {avg:000.00}%\n" +
                             $"Estimated time: {est}\n");

                for (int j = 0; j < _levelChangeList.Count; j++)
                {
                    sw.WriteLine($"{j + 1:00}: {_levelChangeList[j]}");
                }

                sw.Flush();
                sw.Close();
                sw.Dispose();

                timerFlashFinished.Enabled = true;
                timerCheckBatteryChange.Enabled = false;

                Process.Start(_resultFileInfo.ToString());
            }
        }

        private void timerUpdateBatteryBar_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = (int)(SystemInformation.PowerStatus.BatteryLifePercent * 100);
            label2.Text = $"Current power level: {(SystemInformation.PowerStatus.BatteryLifePercent * 100)}";
        }
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private void timerFlashFinished_Tick(object sender, EventArgs e)
        {
            button1.BackColor = button1.BackColor == Color.Lime ? Color.White : Color.Lime;

            Console.Beep(500, 100);
        }

        private class FileInfo
        {
            public string Path;
            public string Name;

            public FileInfo(string path, string name)
            {
                Path = path;
                Name = name;
            }

            public override string ToString()
            {
                return $"{Path}\\{Name}";
            }
        }
    }
}
