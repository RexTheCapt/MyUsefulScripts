using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Text.RegularExpressions;

namespace SystemCheck
{
    public partial class FormMain : Form
    {
        private List<MachineInfo> machineInfos = new List<MachineInfo>();

        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonGetInfo_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            machineInfos.Clear();

            OperatingSystem operatingSystem = Environment.OSVersion;
            string machineName = Environment.MachineName;
            string bioSmaker = HardwareInfo.GetBIOSmaker();
            string bioSserNo = HardwareInfo.GetBIOSserNo();
            string boardMaker = HardwareInfo.GetBoardMaker();
            string boardProductId = HardwareInfo.GetBoardProductId();
            string hddSerialNo = HardwareInfo.GetHDDSerialNo();
            string macAddress = HardwareInfo.GetMACAddress();
            string osInformation = HardwareInfo.GetOSInformation();
            string physicalMemory = HardwareInfo.GetPhysicalMemory();
            string processorId = HardwareInfo.GetProcessorId();
            string processorInformation = HardwareInfo.GetProcessorInformation();
            string noRamSlots = HardwareInfo.GetNoRamSlots();
            GetBiosInfo(out object manufacturer, out object model, out string biosVersion);

            string macRegex = "(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})";
            string macReplace = "$1:$2:$3:$4:$5:$6";

            machineInfos.Add(new MachineInfo("ID", numericUpDown1.Value));
            machineInfos.Add(new MachineInfo("Machine name", machineName));
            machineInfos.Add(new MachineInfo("Board maker", boardMaker));
            machineInfos.Add(new MachineInfo("Model", model));
            machineInfos.Add(new MachineInfo("Serial number", bioSserNo));
            machineInfos.Add(new MachineInfo("MAC address", Regex.Replace(macAddress, macRegex, macReplace)));
            machineInfos.Add(new MachineInfo("BIOS maker", bioSmaker));
            machineInfos.Add(new MachineInfo("BIOS version", biosVersion));
            machineInfos.Add(new MachineInfo("Board product ID", boardProductId));
            machineInfos.Add(new MachineInfo("HDD serial number", hddSerialNo));
            machineInfos.Add(new MachineInfo("RAM", physicalMemory));
            machineInfos.Add(new MachineInfo("CPU ID", processorId));
            machineInfos.Add(new MachineInfo("CPU info", processorInformation));
            machineInfos.Add(new MachineInfo("RAM slots", noRamSlots));
            machineInfos.Add(new MachineInfo("Manufacturer",manufacturer));
            machineInfos.Add(new MachineInfo("Operating system", osInformation));
            machineInfos.Add(new MachineInfo("OS Info", operatingSystem));

            listBox1.Items.Clear();

            foreach (MachineInfo mi in machineInfos)
            {
                listBox1.Items.Add(mi.Name);
            }
        }

        private void GetBiosInfo(out object manufacturer, out object model, out string biosVersion)
        {
            manufacturer = model = biosVersion = null;

            SelectQuery query = new SelectQuery(@"Select * from Win32_ComputerSystem");

            //initialize the searcher with the query it is supposed to execute
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                //execute the query
                foreach (ManagementBaseObject o in searcher.Get())
                {
                    ManagementObject process = (ManagementObject)o;
                    process.Get();
                    manufacturer = process["Manufacturer"];
                    model = process["Model"];
                }
            }

            ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
            ManagementObjectCollection collection = searcher1.Get();


            foreach (ManagementObject obj in collection)
            {
                biosVersion = $"{((string[])obj["BIOSVersion"])[0]}{(((string[])obj["BIOSVersion"]).Length == 2 ? $" - {((string[])obj["BIOSVersion"])[1]}" : "")}";
            }
        }

        private class MachineInfo
        {
            public string Name;
            public object Variable;

            public MachineInfo(string name, object variable)
            {
                Name = name;
                Variable = variable;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = machineInfos[listBox1.SelectedIndex].Variable.ToString();
        }

        private void buttonSaveInfo_Click(object sender, EventArgs e)
        {
            if (machineInfos.Count == 0)
            {
                buttonGetInfo_Click(sender, e);
            }

            machineInfos[0].Variable = numericUpDown1.Value;

            if (!File.Exists($"[PATH]\\{Environment.MachineName}.txt") || MessageBox.Show(
                    "Computer is already registered, do you want to overwrite?", "Computer already registered",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                StreamWriter sw = new StreamWriter($"[PATH]\\{Environment.MachineName}.txt");
                int maxLength = 0;

                foreach (MachineInfo info in machineInfos)
                {
                    if (maxLength < info.Name.Length)
                        maxLength = info.Name.Length;
                }

                foreach (MachineInfo machineInfo in machineInfos)
                {
                    string name = machineInfo.Name;

                    while (name.Length < maxLength)
                    {
                        name = $"{name}.";
                    }

                    sw.WriteLine($"{name}: {machineInfo.Variable}");
                }

                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
        }

        private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && !e.Shift)
                buttonGetInfo_Click(buttonGetInfo, e);
            else if (e.KeyCode == Keys.Enter && e.Shift)
                buttonSaveInfo_Click(buttonSaveInfo, e);
        }
    }
}