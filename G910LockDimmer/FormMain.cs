using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LedCSharp;
using System.Threading;
using System.IO;

namespace G910LockDimmer
{
    public partial class FormMain : Form
    {
        private bool init;
        private GlobalKeyboardHook gHook;
        private List<KeyCount> keyCounts = new List<KeyCount>();
        private int lastHour = DateTime.Now.Hour;
        private DateTime recordFromTime = DateTime.Now;

        public FormMain()
        {
            InitializeComponent();

            #region G910 code
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;

            FormClosing += Form1_FormClosing;

            notifyIcon1.Visible = true;
            notifyIcon1.Icon = SystemIcons.Application;

            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.AddRange(new MenuItem[] {
                new MenuItem("Exit", ExitProgram),
                new MenuItem("Flash", FlashLighting),
                new MenuItem("Show lock", ShowLockLight)
            });

            notifyIcon1.ContextMenu = contextMenu;

            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            #endregion

            this.Load += FormMain_Load;
            this.FormClosing += FormMain_FormClosing;
            textBox1.DoubleClick += TextBox1_DoubleClick;
        }

        private void TextBox1_DoubleClick(object sender, EventArgs e)
        {
            keyCounts.Clear();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveTextBoxContent();
            gHook.unhook();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            gHook = new GlobalKeyboardHook();
            gHook.KeyDown += GHook_KeyDown;

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
                gHook.HookedKeys.Add(key);

            gHook.hook();
        }

        private void GHook_KeyDown(object sender, KeyEventArgs e)
        {
            bool exists = false;
            foreach (KeyCount kc in keyCounts)
                if (kc.Char == (char)e.KeyCode)
                {
                    exists = true;
                    kc.count++;
                    break;
                }

            if (!exists)
                keyCounts.Add(new KeyCount((char)e.KeyCode));

            string display = "";

            foreach (KeyCount kc in keyCounts)
            {
                string c;

                switch (kc.Char)
                {
                    #region A - Z
                    case (char)65:
                    case (char)66:
                    case (char)67:
                    case (char)68:
                    case (char)69:
                    case (char)70:
                    case (char)71:
                    case (char)72:
                    case (char)73:
                    case (char)74:
                    case (char)75:
                    case (char)76:
                    case (char)77:
                    case (char)78:
                    case (char)79:
                    case (char)80:
                    case (char)81:
                    case (char)82:
                    case (char)83:
                    case (char)84:
                    case (char)85:
                    case (char)86:
                    case (char)87:
                    case (char)88:
                    case (char)89:
                    case (char)90:
                    #endregion
                    #region 0 - 9
                    case (char)48:
                    case (char)49:
                    case (char)50:
                    case (char)51:
                    case (char)52:
                    case (char)53:
                    case (char)54:
                    case (char)55:
                    case (char)56:
                    case (char)57:
                    case (char)58:
                        c = kc.Char.ToString();
                        break;
                    #endregion
                    #region System keys
                    #region Right Windows
                    case (char)91:
                        c = "R-WIN";
                        break;
                    #endregion
                    #region Caps Lock
                    case (char)20:
                        c = "CAPS";
                        break;
                    #endregion
                    #endregion
                    #region Modifiers
                    #region Left Control
                    case (char)162:
                        c = "L-CTRL";
                        break;
                    #endregion
                    #region Right control
                    case (char)163:
                        c = "R-CTRL";
                        break;
                    #endregion
                    #region Left Shift
                    case (char)160:
                        c = "L-SHIFT";
                        break;
                    #endregion
                    #region Right shift
                    case (char)161:
                        c = "R-SHIFT";
                        break;
                    #endregion
                    #region Left Alt
                    case (char)164:
                        c = "L-Alt";
                        break;
                    #endregion
                    #endregion
                    #region Tab
                    case (char)9:
                        c = "TAB";
                        break;
                    #endregion
                    #region Pipe
                    case (char)220:
                        c = "PIPE";
                        break;
                    #endregion
                    #region questionmark
                    case (char)187:
                        c = "?";
                        break;
                    #endregion
                    #region back slash
                    case (char)219:
                        c = "\\";
                        break;
                    #endregion
                    #region Backspace
                    case (char)8:
                        c = "\\b";
                        break;
                    #endregion
                    #region enter
                    case (char)13:
                        c = "\\r";
                        break;
                    #endregion
                    #region Context
                    case (char)93:
                        c = "CONTEXT";
                        break;
                    #endregion
                    #region Right windows
                    case (char)92:
                        c = "R-WIN";
                        break;
                    #endregion
                    #region Space
                    case (char)32:
                        c = "SPACE";
                        break;
                    #endregion
                    #region Insert
                    case (char)45:
                        c = "INSERT";
                        break;
                    #endregion
                    #region Home
                    case (char)36:
                        c = "HOME";
                        break;
                    #endregion
                    #region Page up
                    case (char)33:
                        c = "PG-UP";
                        break;
                    #endregion
                    #region Delete
                    case (char)46:
                        c = "DEL";
                        break;
                    #endregion
                    #region End
                    case (char)35:
                        c = "END";
                        break;
                    #endregion
                    #region Page down
                    case (char)34:
                        c = "PG-DWN";
                        break;
                    #endregion
                    #region Arrow keys
                    #region Arrow left
                    case (char)37:
                        c = "ARROW LEFT";
                        break;
                    #endregion
                    #region Arrow up
                    case (char)38:
                        c = "ARROW UP";
                        break;
                    #endregion
                    #region Arrow right
                    case (char)39:
                        c = "ARROW RIGHT";
                        break;
                    #endregion
                    #region Arrow down
                    case (char)40:
                        c = "ARROW DOWN";
                        break;
                    #endregion
                    #endregion
                    #region Keypad
                    #region Keypad 0
                    case (char)96:
                        c = "KP-0";
                        break;
                    #endregion
                    #region Keypad 1
                    case (char)97:
                        c = "KP-1";
                        break;
                    #endregion
                    #region Keypad 2
                    case (char)98:
                        c = "KP-2";
                        break;
                    #endregion
                    #region Keypad 3
                    case (char)99:
                        c = "KP-3";
                        break;
                    #endregion
                    #region Keypad 4
                    case (char)100:
                        c = "KP-4";
                        break;
                    #endregion
                    #region Keypad 5
                    case (char)101:
                        c = "KP-5";
                        break;
                    #endregion
                    #region Keypad 6
                    case (char)102:
                        c = "KP-6";
                        break;
                    #endregion
                    #region Keypad 7
                    case (char)103:
                        c = "KP-7";
                        break;
                    #endregion
                    #region Keypad 8
                    case (char)104:
                        c = "KP-8";
                        break;
                    #endregion
                    #region Keypad 9
                    case (char)105:
                        c = "KP-9";
                        break;
                    #endregion
                    #region keypad comma
                    case (char)110:
                        c = "KP-COMMA";
                        break;
                    #endregion
                    #region keypad add
                    case (char)107:
                        c = "KP-ADD";
                        break;
                    #endregion
                    #region keypad subtract
                    case (char)109:
                        c = "KP-SUB";
                        break;
                    #endregion
                    #region keypad multiply
                    case (char)106:
                        c = "KP-MUL";
                        break;
                    #endregion
                    #region keypad divide
                    case (char)111:
                        c = "KP-DIV";
                        break;
                    #endregion
                    #endregion
                    #region Numlock
                    case (char)144:
                        c = "NUMLCK";
                        break;
                    #endregion
                    #region dash
                    case (char)189:
                        c = "SUB";
                        break;
                    #endregion
                    #region Dot
                    case (char)190:
                        c = "DOT";
                        break;
                    #endregion
                    #region Comma
                    case (char)188:
                        c = "COMMA";
                        break;
                    #endregion
                    #region pause
                    case (char)19:
                        c = "PAUSE";
                        break;
                    #endregion
                    #region Scroll lock
                    case (char)145:
                        c = "SCR-LCK";
                        break;
                    #endregion
                    #region Print screen
                    case (char)44:
                        c = "PRNT-SCRN";
                        break;
                    #endregion
                    #region Function keys
                    #region F1
                    case (char)112:
                        c = "F1";
                        break;
                    #endregion
                    #region F2
                    case (char)113:
                        c = "F2";
                        break;
                    #endregion
                    #region F3
                    case (char)114:
                        c = "F3";
                        break;
                    #endregion
                    #region F4
                    case (char)115:
                        c = "F4";
                        break;
                    #endregion
                    #region F5
                    case (char)116:
                        c = "F5";
                        break;
                    #endregion
                    #region F6
                    case (char)117:
                        c = "F6";
                        break;
                    #endregion
                    #region F7
                    case (char)118:
                        c = "F7";
                        break;
                    #endregion
                    #region F8
                    case (char)119:
                        c = "F8";
                        break;
                    #endregion
                    #region F9
                    case (char)120:
                        c = "F9";
                        break;
                    #endregion
                    #region F10
                    case (char)121:
                        c = "F10";
                        break;
                    #endregion
                    #region F11
                    case (char)122:
                        c = "F11";
                        break;
                    #endregion
                    #region F12
                    case (char)123:
                        c = "F12";
                        break;
                    #endregion
                    #endregion
                    #region Escape
                    case (char)27:
                        c = "ESC";
                        break;
                    #endregion
                    #region Alt GR
                    case (char)165:
                        c = "ALTGR";
                        break;
                    #endregion
                    #region < lss
                    case (char)226:
                        c = "<";
                        break;
                    #endregion
                    #region star
                    case (char)191:
                        c = "*";
                        break;
                    #endregion
                    case (char)186:
                        c = "^";
                        break;
                    default:
                        //throw new Exception("Unhandled key");
                        c = $"U_{(int)e.KeyCode}";
                        break;
                }

                display += $"{c}: {kc.count}\r\n";
            }

            textBox1.Text = display;
        }

        private class KeyCount
        {
            public readonly char Char;
            public int count = 1;

            public KeyCount(char c)
            {
                Char = c;
            }
        }

        #region G910 code
        private void ShowLockLight(object sender, EventArgs e)
        {
            MenuItem m = (MenuItem)sender;

            m.Checked = !m.Checked;

            if (m.Checked)
            {
                SystemEvents_SessionSwitch(sender, new SessionSwitchEventArgs(SessionSwitchReason.SessionLock));
            }
            else
            {
                SystemEvents_SessionSwitch(sender, new SessionSwitchEventArgs(SessionSwitchReason.SessionUnlock));
            }
        }

        private void FlashLighting(object sender, EventArgs e)
        {
            if (!init)
                init = LogitechGSDK.LogiLedInit();

            if (init)
            {
                LogitechGSDK.LogiLedPulseLighting(100, 0, 0, 500, 250);
                Thread.Sleep(500);

                LogitechGSDK.LogiLedShutdown();
                init = false;
            }
        }

        private void ExitProgram(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;

            if (init)
                LogitechGSDK.LogiLedShutdown();
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                if (!init)
                    init = LogitechGSDK.LogiLedInit();

                LogitechGSDK.LogiLedSetLighting(15, 0, 0);
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                LogitechGSDK.LogiLedPulseLighting(0, 100, 0, 500, 250);
                Thread.Sleep(500);
                if (init)
                {
                    LogitechGSDK.LogiLedShutdown();
                    init = false;
                }
            }
        }
        #endregion

        private void timerHourlySave_Tick(object sender, EventArgs e)
        {
            if (lastHour != DateTime.Now.Hour)
            {
                SaveTextBoxContent();
                lastHour = DateTime.Now.Hour;
            }
        }

        private void SaveTextBoxContent()
        {
            if (!Directory.Exists("Keys"))
                Directory.CreateDirectory("Keys");

            StreamWriter sw = new StreamWriter($"Keys/{DateTime.Now.ToString().Replace(":", "-")}.txt");

            sw.WriteLine($"Keypresses recorded from {recordFromTime} to {DateTime.Now}");
            sw.WriteLine(textBox1.Text);

            sw.Close();
            sw.Dispose();

            textBox1.Text = "";
            keyCounts.Clear();

            recordFromTime = DateTime.Now;
        }
    }
}
