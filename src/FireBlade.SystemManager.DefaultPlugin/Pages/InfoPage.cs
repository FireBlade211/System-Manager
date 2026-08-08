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

        private const int LVM_FIRST = 0x1000;
        private const int LVM_SETGROUPINFO = LVM_FIRST + 147;

        private const uint LVGF_STATE = 0x00000004;

        private const uint LVGS_NORMAL = 0x00000000;
        private const uint LVGS_COLLAPSED = 0x00000001;
        private const uint LVGS_HIDDEN = 0x00000002;
        private const uint LVGS_NOHEADER = 0x00000004;
        private const uint LVGS_COLLAPSIBLE = 0x00000008;
        private const uint LVGS_FOCUSED = 0x00000010;
        private const uint LVGS_SELECTED = 0x00000020;
        private const uint LVGS_SUBSETED = 0x00000040;
        private const uint LVGS_SUBSETLINKFOCUSED = 0x00000080;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct LVGROUP
        {
            public uint cbSize;
            public uint mask;
            public IntPtr pszHeader;
            public int cchHeader;
            public IntPtr pszFooter;
            public int cchFooter;
            public int iGroupId;
            public uint stateMask;
            public uint state;
            public uint uAlign;
            public IntPtr pszSubtitle;
            public uint cchSubtitle;
            public IntPtr pszTask;
            public uint cchTask;
            public IntPtr pszDescriptionTop;
            public uint cchDescriptionTop;
            public IntPtr pszDescriptionBottom;
            public uint cchDescriptionBottom;
            public int iTitleImage;
            public int iExtendedImage;
            public int iFirstItem;
            public IntPtr pszSubsetTitle;
            public uint cchSubsetTitle;
        }

        private CancellationTokenSource _wmiTokenSrc;
        //private HashSet<object> Items = new HashSet<object>();

        private void LoadInfo()
        {
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

            label6.Text = GetProcessorName();

            listView1.UseWaitCursor = true;

            listView1.Items.Clear();
            listView1.BeginUpdate();

            _wmiTokenSrc = new CancellationTokenSource();
            button1.Enabled = _wmiTokenSrc.Token.CanBeCanceled;

            progressBar1.Value = 0;
            progressBar1.Visible = true;
            label7.Visible = true;
            button1.Visible = true;

            // Wait until UI operations are finished.
            BeginInvoke(() => Task.Run(() =>
            {
                using (ManagementObjectSearcher rootSearcher = new ManagementObjectSearcher(
                    "SELECT * FROM meta_class"))
                {
                    var classes = rootSearcher.Get();
                    Invoke(() => progressBar1.Maximum = classes.Count);
                    
                    foreach (var c in classes)
                    {
                        if (_wmiTokenSrc.Token.IsCancellationRequested) break;

                        string className = c["__CLASS"].ToString();

                        // blacklist to make it faster
                        if (className.StartsWith("CIM_")) continue;
                        if (className.Equals("Win32_SoftwareElement", StringComparison.Ordinal)) continue;
                        if (className.Equals("Win32_NTDomain", StringComparison.Ordinal)) continue;
                        if (className.Equals("Win32_CodecFile", StringComparison.Ordinal)) continue;
                        if (className.Equals("Win32_NTEventlogFile", StringComparison.Ordinal)) continue;
                        if (className.Contains("Directory")) continue;
                        if (className.Equals("Win32_NTLogEvent", StringComparison.Ordinal)) continue;
                        if (className.Equals("Win32_PageFile", StringComparison.Ordinal)) continue;
                        if (className.EndsWith("Service")) continue;
                        if (className.Contains("Driver")) continue;
                        if (className.Equals("Win32_Thread", StringComparison.Ordinal)) continue;
                        if (className.Contains("COM")) continue;
                        if (className.Contains("Win32_SoftwareFeature")) continue;
                        if (className.Equals("Win32_OptionalFeature", StringComparison.Ordinal)) continue;
                        if (className.Contains("Component")) continue;
                        if (className.Contains("CommandLine")) continue;
                        if (className.Contains("Win32_SecuritySetting")) continue;
                        if (className.Equals("Win32_LoadOrderGroupServiceMembers", StringComparison.Ordinal)) continue;
                        if (className.Equals("Win32_GroupUser", StringComparison.Ordinal)) continue;
                        if (className.Equals("Win32_MemoryDeviceArray", StringComparison.Ordinal)) continue;
                        if (className.Contains("ProgramGroup")) continue;
                        if (className.Equals("Win32_PhysicalMemoryLocation", StringComparison.Ordinal)) continue;
                        if (className.Equals("Win32_Product", StringComparison.Ordinal)) continue;
                        if (className.Equals("Win32_MSIResource", StringComparison.Ordinal)) continue;
                        if (className.Equals("Win32_ComputerSystemProduct", StringComparison.Ordinal)) continue;
                        if (className.Equals("Win32_ServiceControl", StringComparison.Ordinal)) continue;
                        if (className.Equals("Win32_Property", StringComparison.Ordinal)) continue;
                        if (className.Equals("Win32_PatchPackage", StringComparison.Ordinal)) continue;
                        if (className.Equals("Win32_ShortcutAction", StringComparison.Ordinal)) continue;

                        double percentage = ((double)progressBar1.Value / (double)progressBar1.Maximum) * 100.0;

                        Invoke(() => label7.Text = $"Processing: {className}" +
                        $"\n{percentage:0.##}%");

                        using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                            "SELECT * FROM " + className))
                        {
                            var results = searcher.Get().Cast<ManagementObject>();

                            try
                            {
                                if (results.Any())
                                {
                                    int index = 0;

                                    foreach (var instance in results)
                                    {
                                        if (_wmiTokenSrc.Token.IsCancellationRequested) break;

                                        Invoke(() => progressBar1.Maximum += instance.Properties.Count);

                                        ListViewGroup group = null;

                                        listView1.Invoke(() =>
                                        {
                                            group = new ListViewGroup($"{className} - Instance #{index}");
                                            int id = listView1.Groups.Add(group) + 1;

                                            LVGROUP lvg = new LVGROUP();
                                            lvg.cbSize = (uint)Marshal.SizeOf<LVGROUP>();
                                            lvg.mask = LVGF_STATE;
                                            lvg.stateMask = LVGS_COLLAPSIBLE;
                                            lvg.state = LVGS_COLLAPSIBLE;

                                            nint hStruct = Marshal.AllocHGlobal(Marshal.SizeOf<LVGROUP>());
                                            Marshal.StructureToPtr(lvg, hStruct, false);

                                            NativeHelper.SendMessageW(listView1.Handle, LVM_SETGROUPINFO, (UIntPtr)id, hStruct);
                                            Marshal.FreeHGlobal(hStruct);

                                            //Items.Add(group);
                                        });

                                        foreach (var prop in instance.Properties)
                                        {
                                            if (_wmiTokenSrc.Token.IsCancellationRequested) break;

                                            listView1.Invoke(() =>
                                            {
                                                var item = new ListViewItem
                                                {
                                                    Text = prop.Name,
                                                    Tag = prop,
                                                    Group = group,
                                                    SubItems =
                                                    {
                                                        new ListViewItem.ListViewSubItem
                                                        {
                                                            Text = prop.Value?.ToString() ?? "<none>"
                                                        }
                                                    }
                                                };

                                                listView1.Items.Add(item);
                                                //Items.Add(item);
                                            });

                                            Invoke(() => progressBar1.Value++);
                                        }

                                        index++;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("ERROR: " + ex.Message);
                            }
                        }

                        Invoke(() => progressBar1.Value++);
                    }

                    _wmiTokenSrc.Dispose();
                }

                listView1.Invoke(() =>
                {
                    listView1.EndUpdate();
                    listView1.UseWaitCursor = false;

                    progressBar1.Visible = false;
                    label7.Visible = false;
                    button1.Visible = false;
                });
            }));
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

        private static string GetProcessorName()
        {
            using (var searcher = new ManagementObjectSearcher(
                "SELECT Name FROM Win32_Processor"))
            using (var results = searcher.Get())
            {
                foreach (ManagementObject processor in results)
                    return processor["Name"]?.ToString();
            }

            return null;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            LoadInfo();
        }

        private void InfoPage_Load(object sender, EventArgs e)
        {
            LoadInfo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _wmiTokenSrc.Cancel();
            button1.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //string query = textBox1.Text;
            //HashSet<object> found = new HashSet<object>();

            //foreach (var entry in Items)
            //    if (entry is ListViewItem lvi)
            //    {
            //        if (lvi.Text.Contains(query))
            //            found.Add(lvi);
            //    }
            //    else if (entry is ListViewGroup lvg)
            //    {
            //        if (lvg.Header.Contains(query))
            //            found.Add(lvg);
            //    }

            //listView1.BeginUpdate();

            //foreach (var item in Items)
            //    if (!found.Contains(item))
            //    {
            //        if (item is ListViewItem lvi)
            //            listView1.Items.Remove(lvi);
            //        else if (item is ListViewGroup lvg)
            //        {
            //            listView1.Groups.Remove(lvg);
            //            foreach (ListViewItem it in lvg.Items)
            //                listView1.Items.Remove(it);
            //        }
            //    }
            //    else
            //    {
            //        if (item is ListViewItem lvi && !listView1.Items.Contains(lvi))
            //            listView1.Items.Add(lvi);
            //        else if (item is ListViewGroup lvg && !listView1.Groups.Contains(lvg))
            //            listView1.Groups.Add(lvg);
            //    }

            //listView1.EndUpdate();

            var foundItem = listView1.FindItemWithText(textBox1.Text, true, 0, true);

            if (foundItem != null)
            {
                listView1.TopItem = foundItem;
            }
            else
            {
                foreach (ListViewGroup group in listView1.Groups)
                {
                    if (group.Header.Contains(textBox1.Text))
                    {
                        if (group.Items.Count > 0)
                            listView1.TopItem = group.Items[0];

                        break;
                    }
                }
            }
        }
    }
}
