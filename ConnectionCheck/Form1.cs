using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionCheck
{
    public partial class Form1 : Form
    {
        private Timer _pingTimer;
        private Timer _updateInfo;

        private int _interval = 1000;
        private int _timeout = 1000;
        private int _failedInRow = 0;
        private int _maxGraphLength = 300;

        private long _success;
        private long _fail;

        private long _total => _success + _fail;
        private List<long> _time = new List<long>();
        private List<double> _timeAverage = new List<double>();
        private string _target = "1.1.1.1";
        private List<Task> _pings = new List<Task>();
        private DateTime _lastCheck = DateTime.Now;
        private DateTime _lastUpdate;

        private class Time
        {
            public static TimeSpan Offline;
            public static TimeSpan Online;
        }

        public Form1()
        {
            InitializeComponent();

            _pingTimer = new Timer();
            _pingTimer.Interval = _interval;
            _pingTimer.Tick += _pingTimer_Tick;
            _pingTimer.Enabled = true;

            _updateInfo = new Timer();
            _updateInfo.Interval = 1;
            _updateInfo.Tick += _updateInfo_Tick;
            _updateInfo.Enabled = true;

            textBox1.DoubleClick += TextBox1_DoubleClick;
        }

        private void TextBox1_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text);
        }

        private void _updateInfo_Tick(object sender, EventArgs e)
        {
            TimeSpan result = DateTime.Now - _lastCheck;

            int runningPings = GetRunningPings();

            if (_failedInRow > 5)
                Time.Offline += result;
            else
                Time.Online += result;

            _lastCheck = DateTime.Now;

            if (Form.ActiveForm == this || _lastUpdate.AddSeconds(5) < DateTime.Now)
            {
                string output =
                    $"Status:\r\n" +
                    $"Target:    {_target}\r\n" +
                    $"Success:   {_success}\r\n" +
                    $"Failed:    {_fail} [{_failedInRow}]\r\n" +
                    $"Total:     {_total}\r\n" +
                    $"Average:   {AverageTime():#.000}ms\r\n" +
                    $"Online:    {Time.Online}\r\n" +
                    $"Offline:   {Time.Offline}\r\n" +
                    $"Running:   {runningPings}\r\n" +
                    $"{GetTaskTimes()}\r\n";

                if (Form.ActiveForm == this)
                    textBox1.Text = output;
                else if (_lastUpdate.AddSeconds(5) < DateTime.Now)
                    textBox1.Text = output;

                _lastUpdate = DateTime.Now;

                var chartRoundtrip = chart1.Series[0];
                var chartAverage = chart1.Series[1];
                var chartTimeout = chart1.Series[2];

                chartRoundtrip.Points.Clear();
                chartAverage.Points.Clear();
                chartTimeout.Points.Clear();

                double min = _timeout, max = 0;

                for (int i = 0; i < _time.Count; i++)
                {
                    long l = _time[i];

                    if (l < min && l != -1)
                        min = l;

                    if (l > max)
                        max = l;
                }

                for (int i = 0; i < _time.Count; i++)
                {
                    double l = _time[i];

                    if (l == -1)
                    {
                        l = ((double)min + max) / 2;
                        chartTimeout.Points.Add(((double)min + max) / 2);
                    }
                    else
                        chartTimeout.Points.Add(0);

                    chartRoundtrip.Points.Add(l);
                    chartAverage.Points.Add(AverageTime() * 1000);

                }

                chart1.ChartAreas[0].AxisY.Minimum = min;

                if (max <= min)
                    max = min + 1;

                chart1.ChartAreas[0].AxisY.Maximum = max;
            }
        }

        private void _pingTimer_Tick(object sender, EventArgs e)
        {
            Action<object> action = (Object obj) =>
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();

                options.DontFragment = true;

                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);

                PingReply reply = pingSender.Send(_target, _timeout, buffer, options);

                if (reply.Status == IPStatus.Success)
                {
                    _success++;
                    _failedInRow = 0;
                    _time.Add(reply.RoundtripTime);

                    while (_time.Count > _maxGraphLength)
                    {
                        _time.RemoveAt(0);
                    }
                }
                else
                {
                    while (_time.Count > _maxGraphLength)
                    {
                        _time.RemoveAt(0);
                    }

                    _time.Add(-1);
                    _fail++;
                    _failedInRow++;
                }
            };

            Task task = new Task(action, DateTime.Now.AddMilliseconds(_timeout));
            task.Start();
            _pings.Add(task);
        }

        private string GetTaskTimes()
        {
            string s = "";

            foreach (Task t in _pings)
            {
                s += $"         - {(DateTime)t.AsyncState - DateTime.Now}\r\n";
            }

            return s;
        }

        private int GetRunningPings()
        {
            for (int i = _pings.Count - 1; i >= 0; i--)
            {
                if (_pings[i].IsCompleted)
                {
                    _pings[i].Dispose();
                    _pings.RemoveAt(i);
                }
            }

            return _pings.Count;
        }

        private double AverageTime()
        {
            if (_time.Count > 0)
                try
                {
                    _timeAverage.Add(_time.Average());
                    return _time.Average() / 1000;
                }
                catch (InvalidOperationException ioe)
                {
                    if (ioe.Message.StartsWith("Collection was modified;"))
                    {
                        int count = 0;
                        long total = 0;

                        for (int i = 0; i < _time.Count; i++)
                        {
                            count++;
                            total += _time[i];
                        }

                        _timeAverage.Add((double)total / count);
                        return total / count;
                    }

                    throw ioe;
                }
            else
            {
                _timeAverage.Add(0);
                return 0;
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            _pingTimer.Enabled = false;
            _updateInfo.Enabled = false;

            _success = 0;
            _fail = 0;
            _time = new List<long>();

            Action<object> action = (Object obj) =>
            {
                for (int i = _pings.Count - 1; i >= 0; i--)
                {
                    _pings[i].Wait();
                    _pings[i].Dispose();
                    _pings.RemoveAt(i);
                }
            };

            Task task = new Task(action, "Ping remover");
            task.Start();

            Time.Offline = TimeSpan.Zero;
            Time.Online = TimeSpan.Zero;

            while (_pings.Count != 0)
            {
                textBox1.Text = $"ETA {(DateTime)_pings[_pings.Count - 1].AsyncState - DateTime.Now} left on last ping";
                Update();
            }

            _pingTimer.Enabled = true;
            _updateInfo.Enabled = true;
        }
    }
}
