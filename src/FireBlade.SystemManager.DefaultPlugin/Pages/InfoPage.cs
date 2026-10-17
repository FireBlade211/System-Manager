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
            progressBar1.Visible = true;
            label44.Visible = true;

            DisposeObjects();
            label4.Text = Environment.MachineName;
            
            listBox1.BeginUpdate();
            listBox1.Items.Clear();

            Task.Run(() =>
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
                {
                    using (var results = searcher.Get())
                    {
                        foreach (var item in results.OfType<ManagementObject>())
                            Invoke(() => listBox1.Items.Add(new WmiObjectLbItem
                            {
                                Object = item,
                                Name = TryGetValue(item, "Name", $"Untitled Processor #{listBox1.Items.Count - 1}")
                            }));
                    }
                }

                Invoke(() =>
                {
                    progressBar1.Visible = false;
                    label44.Visible = false;

                    listBox1.EndUpdate();
                });

                Invoke(() =>
                {
                    listBox2.BeginUpdate();
                    listBox2.Items.Clear();
                });

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory"))
                {
                    using (var results = searcher.Get())
                    {
                        ulong bytes = 0;

                        foreach (var item in results.OfType<ManagementObject>())
                        {
                            bytes += (ulong)item["Capacity"];

                            Invoke(() => listBox2.Items.Add(new WmiObjectLbItem
                            {
                                Name = FormatMemoryName(item),
                                Object = item
                            }));
                        }

                        Invoke(() => label5.Text = FormatBytes(bytes));
                    }
                }

                Invoke(() => listBox2.EndUpdate());
            });
        }

        private void DisposeObjects()
        {
            foreach (var item in listBox1.Items.Cast<WmiObjectLbItem>())
                item.Object.Dispose();

            foreach (var item in listBox2.Items.Cast<WmiObjectLbItem>())
                item.Object.Dispose();
        }

        private string FormatMemoryName(ManagementObject item)
        {
            string manufacturer = TryGetValue<string>(item, "Manufacturer");
            string partNumber = TryGetValue<string>(item, "PartNumber");
            ulong capacity = TryGetValue<ulong>(item, "Capacity");

            string name = !string.IsNullOrWhiteSpace(partNumber)
                ? $"{manufacturer.Trim()} {partNumber.Trim()}"
                : !string.IsNullOrWhiteSpace(manufacturer)
                ? $"{manufacturer.Trim()} RAM"
                : TryGetValue(item, "Name", out string n)
                ? n.Trim()
                : "Physical Memory";

            name += $" ({FormatBytes(capacity)})";

            return name.Trim();
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
            => (obj[prop]?.ToString()) ?? fallback;

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

        private bool TryGetValue<T>(ManagementObject obj, string prop, out T val)
        {
            try
            {
                // we can't use the as keyword here because it only works with reference types
                var v = obj[prop];
                if (v is T o)
                {
                    val = o;
                    return true;
                }
            }
            catch
            {
                
            }

            val = default;
            return false;
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Cast<object>().FirstOrDefault() is WmiObjectLbItem l)
            {
                var item = l.Object;

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

                label21.Text = "Unknown";
                if (TryGetValue(item, "CurrentVoltage", out ushort currentVoltage))
                {
                    double? voltage = (currentVoltage & 0x80) != 0
                        ? (currentVoltage & 0x7F) / 10.0
                        : null;

                    if (voltage != null)
                        label21.Text = $"{voltage} V";
                }

                label22.Text = "Unknown";
                if (TryGetValue(item, "L2CacheSize", out uint l2size))
                    label22.Text = FormatBytes((ulong)l2size * 1024);

                label24.Text = "Unknown";
                if (TryGetValue(item, "L2CacheSpeed", out uint l2speed))
                    label24.Text = FormatClockSpeed(l2speed);

                label28.Text = "Unknown";
                if (TryGetValue(item, "L3CacheSize", out uint l3size))
                    label28.Text = FormatBytes((ulong)l3size * 1024);

                label26.Text = "Unknown";
                if (TryGetValue(item, "L3CacheSpeed", out uint l3speed))
                    label26.Text = FormatClockSpeed(l3speed);

                if (TryGetValue(item, "LoadPercentage", out ushort loadPercentage))
                    label30.Text = $"{loadPercentage}%";
                else
                    label30.Text = "Unknown";

                label32.Text = "Unknown";
                if (TryGetValue(item, "MaxClockSpeed", out uint max))
                    label32.Text = FormatClockSpeed(max);

                label34.Text = "Unknown";
                if (TryGetValue(item, "NumberOfCores", out uint cores))
                    label34.Text = cores.ToString();

                label36.Text = "Unknown";
                if (TryGetValue(item, "NumberOfEnabledCore", out uint enabledCores))
                    label36.Text = enabledCores.ToString();

                label38.Text = "Unknown";
                if (TryGetValue(item, "NumberOfLogicalProcessors", out uint logicalProc))
                    label38.Text = logicalProc.ToString();

                label40.Text = "Unknown";
                if (TryGetValue(item, "ThreadCount", out uint threads))
                    label40.Text = threads.ToString();

                label42.Text = TryGetValue<ushort>(item, "Family") switch
                {
                    3 => "Intel 8086",
                    4 => "Intel 80286",
                    5 => "Intel 80386",
                    6 => "Intel 80486",
                    7 => "Intel 8087",
                    8 => "Intel 80287",
                    9 => "Intel 80387",
                    10 => "Intel 80487",
                    11 => "Intel™ Pentium®",
                    12 => "Intel™ Pentium® Pro",
                    13 => "Intel™ Pentium® II",
                    14 => "Intel™ Pentium® with MMX",
                    15 => "Intel™ Celeron®",
                    16 => "Intel™ Pentium® II Xeon™",
                    17 => "Intel™ Pentium® III",
                    18 => "Motorola M1 Family",
                    19 => "Motorola M2 Family",
                    20 => "Intel™ Celeron® M Processor",
                    21 => "Intel™ Celeron® 4 HT Processor",
                    24 => "K5 Family",
                    25 => "K6 Family",
                    26 => "K6-2",
                    27 => "K6-3",
                    28 => "AMD Athlon™ Family",
                    29 => "AMD® Duron™",
                    30 => "AMD2900 Family",
                    31 => "K6-2+",
                    32 => "PowerPC",
                    33 => "PowerPC 601",
                    34 => "PowerPC 603",
                    35 => "PowerPC 603+",
                    36 => "PowerPC 604",
                    37 => "PowerPC 620",
                    38 => "PowerPC X704",
                    39 => "PowerPC 750",
                    40 => "Intel® Core™ Duo Processor",
                    41 => "Intel® Core™ Duo Mobile Processor",
                    42 => "Intel® Core™ Solo Mobile Processor",
                    43 => "Intel® Atom™ Processor",
                    48 => "Alpha Family",
                    49 => "Alpha 21064",
                    50 => "Alpha 21066",
                    51 => "Alpha 21164",
                    52 => "Alpha 21164 PC",
                    53 => "Alpha 21164a",
                    54 => "Alpha 21264",
                    55 => "Alpha 21364",
                    56 => "AMD Turion™ II Ultra Dual-Core Mobile M Processor Family",
                    57 => "AMD Turion™ II Dual-Core Mobile M Processor Family",
                    58 => "AMD Athlon™ II Dual-Core Mobile M Processor Family",
                    59 => "AMD Opteron™ 6100 Series Processor",
                    60 => "AMD Opteron™ 4100 Series Processor",
                    64 => "MIPS Family",
                    65 => "MIPS R4000",
                    66 => "MIPS R4200",
                    67 => "MIPS R4400",
                    68 => "MIPS R4600",
                    69 => "MIPS R10000",
                    80 => "SPARC Family",
                    81 => "SuperSPARC",
                    82 => "microSPARC II",
                    83 => "microSPARC IIep",
                    84 => "UltraSPARC",
                    85 => "UltraSPARC II",
                    86 => "UltraSPARC IIi",
                    87 => "UltraSPARC III",
                    88 => "UltraSPARC IIIi",
                    96 => "68040",
                    97 => "68xxx Family",
                    98 => "68000",
                    99 => "68010",
                    100 => "68020",
                    101 => "68030",
                    112 => "Hobbit Family",
                    120 => "Crusoe™ TM5000 Family",
                    121 => "Crusoe™ TM3000 Family",
                    122 => "Efficeon™ TM8000 Family",
                    128 => "Weitek",
                    130 => "Intel™ Itanium Processor",
                    131 => "AMD Athlon™ 64 Processor Family",
                    132 => "AMD Opteron™ Processor Family",
                    133 => "AMD Sempron™ Processor Family",
                    134 => "AMD Turion™ 64 Mobile Technology",
                    135 => "Dual-Core AMD Opteron™ Processor Family",
                    136 => "AMD Athlon™ 64 X2 Dual-Core Processor Family",
                    137 => "AMD Turion™ 64 X2 Mobile Technology",
                    138 => "Quad-Core AMD Opteron™ Processor Family",
                    139 => "Third-Generation AMD Opteron™ Processor Family",
                    140 => "AMD Phenom™ FX Quad-Core Processor Family",
                    141 => "AMD Phenom™ X4 Quad-Core Processor Family",
                    142 => "AMD Phenom™ X2 Dual-Core Processor Family",
                    143 => "AMD Athlon™ X2 Dual-Core Processor Family",
                    144 => "PA-RISC Family",
                    145 => "PA-RISC 8500",
                    146 => "PA-RISC 8000",
                    147 => "PA-RISC 7300LC",
                    148 => "PA-RISC 7200",
                    149 => "PA-RISC 7100LC",
                    150 => "PA-RISC 7100",
                    160 => "V30 Family",
                    161 => "Quad-Core Intel® Xeon® processor 3200 Series",
                    162 => "Dual-Core Intel® Xeon® processor 3000 Series",
                    163 => "Quad-Core Intel® Xeon® processor 5300 Series",
                    164 => "Dual-Core Intel® Xeon® processor 5100 Series",
                    165 => "Dual-Core Intel® Xeon® processor 5000 Series",
                    166 => "Dual-Core Intel® Xeon® processor LV",
                    167 => "Dual-Core Intel® Xeon® processor ULV",
                    168 => "Dual-Core Intel® Xeon® processor 7100 Series",
                    169 => "Quad-Core Intel® Xeon® processor 5400 Series",
                    170 => "Quad-Core Intel® Xeon® processor",
                    171 => "Dual-Core Intel® Xeon® processor 5200 Series",
                    172 => "Dual-Core Intel® Xeon® processor 7200 Series",
                    173 => "Quad-Core Intel® Xeon® processor 7300 Series",
                    174 => "Quad-Core Intel® Xeon® processor 7400 Series",
                    175 => "Multi-Core Intel® Xeon® processor 7400 Series",
                    176 => "Pentium® III Xeon™",
                    177 => "Pentium® III Processor with Intel® SpeedStep™ Technology",
                    178 => "Pentium® 4",
                    179 => "Intel® Xeon™",
                    180 => "AS400 Family",
                    181 => "Intel® Xeon™ processor MP",
                    182 => "AMD Athlon™ XP Family",
                    183 => "AMD Athlon™ MP Family",
                    184 => "Intel® Itanium® 2",
                    185 => "Intel® Pentium® M processor",
                    186 => "Intel® Celeron® D processor",
                    187 => "Intel® Pentium® D processor",
                    188 => "Intel® Pentium® Processor Extreme Edition",
                    189 => "Intel® Core™ Solo Processor",
                    190 => "K7",
                    191 => "Intel® Core™ 2 Duo Processor",
                    192 => "Intel® Core™ 2 Solo processor",
                    193 => "Intel® Core™ 2 Extreme processor",
                    194 => "Intel® Core™ 2 Quad processor",
                    195 => "Intel® Core™ 2 Extreme mobile processor",
                    196 => "Intel® Core™ 2 Duo mobile processor",
                    197 => "Intel® Core™ 2 Solo mobile processor",
                    198 => "Intel® Core™ i7 processor",
                    199 => "Dual-Core Intel® Celeron® Processor",
                    200 => "S/390 and zSeries Family",
                    201 => "ESA/390 G4",
                    202 => "ESA/390 G5",
                    203 => "ESA/390 G6",
                    204 => "z/Architecture base",
                    205 => "Intel® Core™ i5 processor",
                    206 => "Intel® Core™ i3 processor",
                    207 => "Intel® Core™ i9 processor",
                    210 => "VIA C7™-M Processor Family",
                    211 => "VIA C7™-D Processor Family",
                    212 => "VIA C7™ Processor Family",
                    213 => "VIA Eden™ Processor Family",
                    214 => "Multi-Core Intel® Xeon® processor",
                    215 => "Dual-Core Intel® Xeon® processor 3xxx Series",
                    216 => "Quad-Core Intel® Xeon® processor 3xxx Series",
                    217 => "VIA Nano™ Processor Family",
                    218 => "Dual-Core Intel® Xeon® processor 5xxx Series",
                    219 => "Quad-Core Intel® Xeon® processor 5xxx Series",
                    221 => "Dual-Core Intel® Xeon® processor 7xxx Series",
                    222 => "Quad-Core Intel® Xeon® processor 7xxx Series",
                    223 => "Multi-Core Intel® Xeon® processor 7xxx Series",
                    224 => "Multi-Core Intel® Xeon® processor 3400 Series",
                    230 => "Embedded AMD Opteron™ Quad-Core Processor Family",
                    231 => "AMD Phenom™ Triple-Core Processor Family",
                    232 => "AMD Turion™ Ultra Dual-Core Mobile Processor Family",
                    233 => "AMD Turion™ Dual-Core Mobile Processor Family",
                    234 => "AMD Athlon™ Dual-Core Processor Family",
                    235 => "AMD Sempron™ SI Processor Family",
                    236 => "AMD Phenom™ II Processor Family",
                    237 => "AMD Athlon™ II Processor Family",
                    238 => "Six-Core AMD Opteron™ Processor Family",
                    239 => "AMD Sempron™ M Processor Family",
                    250 => "i860",
                    251 => "i960",
                    254 => "Reserved (SMBIOS Extension)",
                    255 => "Reserved (Un-initialized Flash Content - Lo)",
                    260 => "SH-3",
                    261 => "SH-4",
                    280 => "ARM",
                    281 => "StrongARM",
                    300 => "6x86",
                    301 => "MediaGX",
                    302 => "MII",
                    320 => "WinChip",
                    350 => "DSP",
                    500 => "Video Processor",
                    65534 => "Reserved (For Future Special Purpose Assignment)",
                    65535 => "Reserved (Un-initialized Flash Content - Hi)",
                    1 => TryGetValue(item, "OtherFamilyDescription"), // back to the beginning (default is Unknown btw)
                    _ => "Unknown"
                };
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItems.Cast<object>().FirstOrDefault() is WmiObjectLbItem l)
            {
                var item = l.Object;

                label46.Text = FormatMemoryName(item);
                label48.Text = TryGetValue(item, "BankLabel");

                if (TryGetValue(item, "Capacity", out ulong capacity))
                    label50.Text = FormatBytes(capacity);
                else
                    label50.Text = "Unknown";

                label52.Text = TryGetValue(item, "Caption");
                label54.Text = TryGetValue(item, "Name");

                if (TryGetValue(item, "ConfiguredClockSpeed", out uint clockConf))
                    label56.Text = FormatClockSpeed(clockConf);

                if (TryGetValue(item, "ConfiguredVoltage", out uint millivolts) && millivolts != 0)
                    label58.Text = $"{millivolts / 1000.0:F2} V";
                else
                    label58.Text = "Unknown";

                label59.Text = TryGetValue(item, "Description");
                label61.Text = TryGetValue(item, "DeviceLocator");

                if (TryGetValue(item, "FormFactor", out ushort formFactor))
                    label63.Text = formFactor switch
                    {
                        2 => "SIP",
                        3 => "DIP",
                        4 => "ZIP",
                        5 => "SOJ",
                        6 => "Proprietary",
                        7 => "SIMM",
                        8 => "DIMM",
                        9 => "TSOP",
                        10 => "PGA",
                        11 => "RIMM",
                        12 => "SODIMM",
                        13 => "SRIMM",
                        14 => "SMD",
                        15 => "SSMP",
                        16 => "QFP",
                        17 => "TQFP",
                        18 => "SOIC",
                        19 => "LCC",
                        20 => "PLCC",
                        21 => "BGA",
                        22 => "FPBGA",
                        23 => "LGA",
                        _ => "Unknown"
                    };
                else
                    label63.Text = "Unknown";

                if (TryGetValue(item, "InstallDate", out string installDate)
                    && !string.IsNullOrEmpty(installDate))
                    label65.Text = ManagementDateTimeConverter.ToDateTime(installDate).ToString();
                else
                    label65.Text = "Unknown";

                readOnlyCheckBox1.Checked = false;
                readOnlyCheckBox2.Checked = false;
                readOnlyCheckBox3.Checked = false;
                readOnlyCheckBox4.Checked = false;

                if (TryGetValue(item, "HotSwappable", out bool swap))
                    readOnlyCheckBox1.Checked = swap;

                if (TryGetValue(item, "Removable", out bool removable))
                    readOnlyCheckBox2.Checked = removable;

                if (TryGetValue(item, "Replaceable", out bool repl))
                    readOnlyCheckBox3.Checked = repl;

                if (TryGetValue(item, "PoweredOn", out bool power))
                    readOnlyCheckBox4.Checked = power;
            }
        }
    }

    public class WmiObjectLbItem
    {
        public string Name { get; set; }
        public ManagementObject Object { get; set; }

        public override string ToString() => Name;
    }
}
