using FireBlade.SystemManager.Helpers;
using FireBlade.SystemManager.Plugin;
using FireBlade.SystemManager.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager
{
    public partial class SettingsDlg : Form
    {
        public static bool IsActive { get; set; }

        public SettingsDlg()
        {
            InitializeComponent();
            IsActive = true;

            if (!string.IsNullOrEmpty(Settings.Default.GitHubToken))
                textBox2.Text = UnprotectToken(Settings.Default.GitHubToken);
        }

        public void NavigateToPlugins()
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void SettingsDlg_Load(object sender, EventArgs e)
        {
            LoadPluginList();
        }

        private void LoadPluginList()
        {
            listView1.BeginUpdate();
            listView1.Clear();

            foreach (var kvp in PluginHelper.LoadedPlugins)
            {
                if (kvp.Value.QueryContract(ContractIdentifiers.PluginInfoContractIdentifier) is IPluginInfoContract info)
                {
                    if (info.QueryContract(ContractIdentifiers.IconContractIdentifier) is IIconContract icon)
                    {
                        if (icon.TryGetIcon(false, out Icon i))
                            imageList1.Images.Add(i);
                    }

                    var item = new ListViewItem
                    {
                        Text = info.Name,
                        ToolTipText = info.Description,
                        ImageIndex = imageList1.Images.Count - 1,
                        Tag = kvp
                    };

                    listView1.Items.Add(item);
                }
            }

            listView1.EndUpdate();
            label1.Text = $"There are {PluginHelper.LoadedPlugins.Count} installed plugins.";
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.Tag is KeyValuePair<Assembly, IPlugin> kvp)
            {
#if DEBUG
                button3.Enabled = true;
#else
                // TEMP: Check the plugin ID for now until there's a better way of identifying if this is the right plugin
                button3.Enabled = !Path.GetFileName(kvp.Key.Location).Equals("FireBlade.SystemManager.DefaultPlugin.dll",
                    StringComparison.OrdinalIgnoreCase);
#endif

                button4.Enabled = kvp.Value.QueryContract(ContractIdentifiers.AboutContractIdentifier) is IAboutContract;
                button5.Enabled = kvp.Value.QueryContract(ContractIdentifiers.SettingsContractIdentifier) is ISettingsContract;
                button6.Enabled = !Path.GetFileName(kvp.Key.Location).Equals("FireBlade.SystemManager.DefaultPlugin.dll",
                    StringComparison.OrdinalIgnoreCase);

                button7.Enabled = true;

                if (kvp.Value.QueryContract(ContractIdentifiers.PluginInfoContractIdentifier) is IPluginInfoContract info)
                {
                    textBox1.Text = info.Description;

                    var name = kvp.Key.GetName();
                    string copyright =
                        kvp.Key.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;

                    if (!Path.GetFileName(kvp.Key.Location).Equals("FireBlade.SystemManager.DefaultPlugin.dll",
                        StringComparison.OrdinalIgnoreCase))
                        label2.Text = $"{info.Name} v{VersionToString(name.Version)} | {copyright}";
                    else
                        label2.Text = copyright;
                }
            }
            else
            {
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;

                textBox1.Text = string.Empty;
                label2.Text = string.Empty;
            }
        }

        private string VersionToString(Version ver)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(ver.Major);

            if (ver.Minor != 0)
                sb.Append($".{ver.Minor}");

            if (ver.Build != 0)
                sb.Append($".{ver.Build}");

            if (ver.Revision != 0)
                sb.Append($".{ver.Revision}");

            return sb.ToString();
        }

        private bool _saving = false;

        private void button1_Click(object sender, EventArgs e)
        {
            _saving = true;

            Settings.Default.GitHubToken = ProtectToken(textBox2.Text);

            Settings.Default.Save();
            Close();
        }

        private static string ProtectToken(string token)
        {
            byte[] data = Encoding.UTF8.GetBytes(token);

            byte[] encrypted = ProtectedData.Protect(
                data,
                null,
                DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encrypted);
        }

        public static string UnprotectToken(string encrypted)
        {
            byte[] data = Convert.FromBase64String(encrypted);

            byte[] decrypted = ProtectedData.Unprotect(
                data,
                null,
                DataProtectionScope.CurrentUser);

            return Encoding.UTF8.GetString(decrypted);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _saving = false;

            Close();
        }

        private void SettingsDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsActive = false;

            if (_saving) return;

            Settings.Default.Reload();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.Tag is KeyValuePair<Assembly, IPlugin> kvp)
            {
                Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "EXPLORER.exe"),
                    "/select,\"" + kvp.Key.Location + "\"");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://docs.github.com/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var dlg = new PluginGalleryForm();
            dlg.ShowDialog();

            LoadPluginList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.Tag is KeyValuePair<Assembly, IPlugin> kvp)
            {
                File.Move(kvp.Key.Location, kvp.Key.Location + ".tmp");

                var result = MessageBox.Show("To finish removing this plugin, you need to restart System Manager." +
 "\nAny unsaved setting changes will be lost.\nDo you want to restart System Manager now?", "Restart required",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Process.Start(Application.ExecutablePath, string.Join(" ", Environment.CommandLine.Split(' ').Skip(1)));
                    Application.Exit();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.Tag is KeyValuePair<Assembly, IPlugin> kvp)
            {
                var dlg = new PluginGalleryForm();
                dlg.SetItemToSelect(Path.GetFileNameWithoutExtension(kvp.Key.Location));

                dlg.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.Tag is KeyValuePair<Assembly, IPlugin> kvp)
            {
                if (kvp.Value.QueryContract(ContractIdentifiers.AboutContractIdentifier) is IAboutContract contract)
                    contract.ShowDialog(this);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.Tag is KeyValuePair<Assembly, IPlugin> kvp)
            {
                if (kvp.Value.QueryContract(ContractIdentifiers.SettingsContractIdentifier) is ISettingsContract contract)
                    contract.ShowDialog(this);
            }
        }
    }
}
