using FireBlade.SystemManager.Helpers;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;
using System.Management;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace FireBlade.SystemManager.DefaultPlugin.Pages
{
    public partial class InfoPage : UserControl
    {
        public InfoPage()
        {
            InitializeComponent();

            // Workaround because SHGetStockIconInfo doesn't let us pick a custom size
            StockIconHelper.GetLocation(StockIcon.DesktopPC, out string path, out int idx);

            IntPtr hModule = LoadLibraryEx(path, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);
            if (hModule != IntPtr.Zero)
            {
                IntPtr hIco = IntPtr.Zero;

                try
                {
                    int hr = LoadIconWithScaleDown(hModule, (IntPtr)(-idx), 64, 64, out hIco);

                    if (hr != 0)
                        Marshal.ThrowExceptionForHR(hr);

                    Icon temp = Icon.FromHandle(hIco);
                    Icon icon = (Icon)temp.Clone();

                    temp.Dispose();

                    pictureBox1.Image = icon.ToBitmap();
                }
                finally
                {
                    FreeLibrary(hModule);

                    if (hIco != IntPtr.Zero)
                        DestroyIcon(hIco);
                }
            }
        }

        [DllImport("comctl32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int LoadIconWithScaleDown(
            IntPtr hinst,
            IntPtr pszName,
            int cx,
            int cy,
            out IntPtr phico);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr LoadLibraryEx(
    string lpFileName,
    IntPtr hFile,
    uint dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeLibrary(
            IntPtr hModule);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        private const uint LOAD_LIBRARY_AS_DATAFILE = 0x00000002;

        private void LoadInfo()
        {
            DisposeObjects();
            label4.Text = Environment.MachineName;
            
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory"))
            {
                using (var results = searcher.Get())
                {
                    ulong bytes = 0;

                    // for some reason somehow the WMI results are separated by RAM stick or something
                    foreach (var stick in results)
                    {
                        bytes += (ulong)stick["Capacity"];
                    }

                    label5.Text = FormatBytes(bytes);
                }
            }

            listView1.BeginUpdate();
            listView1.Items.Clear();

            Task.Run(() =>
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
                {
                    using (var results = searcher.Get())
                    {
                        foreach (var item in results.OfType<ManagementObject>())
                            Invoke(() => listView1.Items.Add(new ListViewItem
                            {
                                Tag = item,
                                Text = TryGetValue(item, "Name", $"Untitled Processor #{listView1.Items.Count - 1}")
                            }));
                        // TODO: FINISH THE REST OF THE MODERNIZED SYS INFO PAGE, THEN TEST THE PLUGIN GALLERY SHIT, AND FINALLY BE DONE
                    }
                }
            });

            listView1.EndUpdate();
        }

        private void DisposeObjects()
        {
            foreach (var item in listView1.Items.Cast<ListViewItem>())
                if (item.Tag is ManagementObject o)
                    o.Dispose();
        }

        private string AvailabilityToString(ushort available) => available switch
        {
            1 => "Other",
            2 => "Unknown",
            3 => "Running/Full Power",
            4 => "Warning",
            5 => "In Test",
            6 => "Not Applicable",
            7 => "Power Off",
            8 => "Off Line",
            9 => "Off Duty",
            10 => "Degraded",
            11 => "Not Installed",
            12 => "Install Error",
            13 => "Power Save (Unknown)",
            14 => "Power Save (Low Power Mode)",
            15 => "Power Save (Standby)",
            16 => "Power Cycle",
            17 => "Power Save (Warning)",
            18 => "Paused",
            19 => "Not Ready",
            20 => "Not Configured",
            21 => "Quiet",
            _ => "Not found"
        };

        private string TryGetValue(ManagementObject obj, string prop, string fallback = "Unknown")
        {
            // sorry, couldn't find a method to check if something exists
            try
            {
                return obj[prop].ToString();
            }
            catch
            {
                return fallback;
            }
        }

        private T TryGetValue<T>(ManagementObject obj, string prop)
        {
            try
            {
                // we can't use the as keyword here because it only works with reference types
                var val = obj[prop];
                return val is T o ? o : default;
            }
            catch
            {
                return default;
            }
        }

        private static string FormatBytes(ulong bytes)
        {
            const double unit = 1024;

            if (bytes < unit)
                return $"{bytes} B";

            double value = bytes;
            string[] units = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            int unitIndex = 0;

            while (value >= unit && unitIndex < units.Length - 1)
            {
                value /= unit;
                unitIndex++;
            }

            return $"{value:0.00} {units[unitIndex]}";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            LoadInfo();
        }

        private void InfoPage_Load(object sender, EventArgs e)
        {
            LoadInfo();
        }

        private string FormatClockSpeed(uint mhz)
        {
            if (mhz == uint.MaxValue)
                return "Unknown";

            if (mhz < 1000)
                return mhz.ToString() + " MHz";

            double ghz = ((double)mhz) / 1000;
            return ghz.ToString("0.00") + " GHz";
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.Tag is ManagementObject item)
            {
                label6.Text = TryGetValue(item, "Name");
                label9.Text = TryGetValue<ushort>(item, "Architecture") switch
                {
                    (ushort)0u => "x86",
                    (ushort)1u => "MIPS",
                    (ushort)2u => "Alpha",
                    (ushort)3u => "PowerPC",
                    (ushort)5u => "ARM",
                    (ushort)6u => "IA64",
                    (ushort)9u => "x64",
                    (ushort)12u => "ARM64",
                    _ => "Unknown"
                };

                label1.Text = TryGetValue(item, "AddressWidth");
                label11.Text = AvailabilityToString(TryGetValue<ushort>(item, "Availability"));
                label13.Text = TryGetValue(item, "Caption");
                label15.Text = TryGetValue<ushort>(item, "CpuStatus") switch
                {
                    0 => "Unknown",
                    1 => "CPU Enabled",
                    2 => "CPU Disabled by User via BIOS Setup",
                    3 => "CPU Disabled by BIOS (POST Error)",
                    4 => "Idle",
                    5 or 6 => "Unidentified value (Reserved)",
                    7 => "Other",
                    _ => "Unknown"
                };
                label17.Text = FormatClockSpeed(TryGetValue<uint?>(item, "CurrentClockSpeed") ?? uint.MaxValue);
            }
        }
    }
}
