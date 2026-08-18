using FireBlade.SystemManager.Helpers;
using FireBlade.SystemManager.Plugin;
using FireBlade.SystemManager.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager
{
    public partial class PluginGalleryForm : Form
    {
        public PluginGalleryForm()
        {
            InitializeComponent();
        }

        private string _idToSelect;

        public void SetItemToSelect(string id)
        {
            _idToSelect = id;
        }

        private async void LoadPlugins()
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(Settings.Default.GitHubToken))
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                            SettingsDlg.UnprotectToken(Settings.Default.GitHubToken));

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var ver = Assembly.GetCallingAssembly().GetName().Version;

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.object+json"));
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("System-Manager",
                        $"{ver.Major}.{ver.Minor}"));

                    var response = await client.GetAsync("https://api.github.com/repos/FireBlade211/System-Manager-Plugins/contents/plugins");
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    var root = JsonConvert.DeserializeObject<RepositoryItem>(json);

                    List<PluginRepoInfo> infos = new List<PluginRepoInfo>();

                    foreach (var entry in root.Entries)
                    {
                        string ext = Path.GetExtension(entry.Name);

                        if (ext.Equals(".json", StringComparison.OrdinalIgnoreCase))
                        {
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.raw+json"));

                            response = await client.GetAsync(entry.Url);

                            if (response.IsSuccessStatusCode)
                            {
                                json = await response.Content.ReadAsStringAsync();
                                var info = JsonConvert.DeserializeObject<PluginJsonInfo>(json);

                                var i = new PluginRepoInfo
                                {
                                    Info = info,
                                    Item = entry
                                };

                                var lvi = new ListViewItem
                                {
                                    Text = info.Name,
                                    ToolTipText = info.Description,
                                    SubItems =
                                    {
                                        new ListViewItem.ListViewSubItem
                                        {
                                            Text = info.Version
                                        },
                                        new ListViewItem.ListViewSubItem
                                        {
                                            Text = info.Author
                                        }
                                    },
                                    Tag = i
                                };

                                i.ListItem = lvi;

                                infos.Add(i);

                                listView1.Items.Add(lvi);
                            }
                        }
                    }

                    foreach (var entry in root.Entries)
                    {
                        if (!Path.GetExtension(entry.Name).Equals(
                            ".dll", StringComparison.OrdinalIgnoreCase))
                            continue;

                        string manifestName = entry.Name + ".json";

                        PluginRepoInfo info = infos.FirstOrDefault(x =>
                            x.Item.Name.Equals(manifestName, StringComparison.OrdinalIgnoreCase));

                        info?.DllItem = entry;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"An error has occured: {ex.Message}.\n\nWe are sorry for the inconvienience.", null,
                //    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            listView1.EndUpdate();

            if (_idToSelect != null)
            {
                foreach (var lvi in listView1.Items.Cast<ListViewItem>())
                {
                    if (lvi.Tag is PluginRepoInfo info)
                    {
                        info.ListItem.Selected = true;
                        info.ListItem.EnsureVisible();

                        listView1.Focus();
                    }
                }
            }
        }

        private static bool TryParseLfsPointer(
    string content,
    out string oid,
    out long size)
        {
            oid = null;
            size = 0;

            using (var reader = new StringReader(content))
            {
                string version = reader.ReadLine();

                if (!string.Equals(
                    version,
                    "version https://git-lfs.github.com/spec/v1",
                    StringComparison.Ordinal))
                    return false;

                string oidLine = reader.ReadLine();
                string sizeLine = reader.ReadLine();

                if (oidLine == null || sizeLine == null ||
                    !oidLine.StartsWith("oid sha256:", StringComparison.Ordinal) ||
                    !sizeLine.StartsWith("size ", StringComparison.Ordinal))
                    return false;

                oid = oidLine.Substring("oid sha256:".Length);

                if (!long.TryParse(
                    sizeLine.Substring("size ".Length),
                    out size))
                {
                    oid = null;
                    return false;
                }

                return true;
            }
        }

        private void PluginGalleryForm_Load(object sender, EventArgs e)
        {
            LoadPlugins();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if ((listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault().Tag) is PluginRepoInfo info)
            {
                //string url =
                //    $"https://api.github.com/repos/FireBlade211/System-Manager-Plugins/git/blobs/{info.Item.SHA}";

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        if (!string.IsNullOrEmpty(Settings.Default.GitHubToken))
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                                SettingsDlg.UnprotectToken(Settings.Default.GitHubToken));

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        var ver = Assembly.GetCallingAssembly().GetName().Version;

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.object+json"));
                        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("System-Manager",
                            $"{ver.Major}.{ver.Minor}"));

                        //var response = await client.GetAsync(url);
                        //response.EnsureSuccessStatusCode();

                        //string json = await response.Content.ReadAsStringAsync();

                        //var blob = JsonConvert.DeserializeObject<GitBlob>(json);

                        //byte[] data = Convert.FromBase64String(blob.Content);
                        //string content = Encoding.UTF8.GetString(data);

                        //if (TryParseLfsPointer(content, out string oid, out long size))
                        //{

                        //}

                        string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins", info.DllItem.Name);
                        string tempPath = destinationPath + ".tmp";

                        if (File.Exists(destinationPath))
                        {
                            var kvp = PluginHelper.LoadedPlugins.FirstOrDefault(x => Path.GetFullPath(x.Key.Location).Equals(
                                Path.GetFullPath(destinationPath), StringComparison.OrdinalIgnoreCase));

                            if (!kvp.Equals(default(KeyValuePair<Assembly, IPlugin>)))
                            {
                                var parts = info.Info.Version.Split('.').ToList();

                                while (parts.Count < 4)
                                    parts.Add("0");

                                var installed = new Version(string.Join(".", parts));

                                if (kvp.Key.GetName().Version.Equals(installed))
                                {
                                    MessageBox.Show("The same version of this plugin is already installed.", null,
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    
                                    return;
                                }
                            }

                            destinationPath += ".upgrade";
                        }

                        using (var response = await client.GetAsync(
                            $"https://raw.githubusercontent.com/FireBlade211/System-Manager-Plugins/refs/heads/main/{info.DllItem.Path}",
                            HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCode();

                            long? totalBytes = response.Content.Headers.ContentLength;

                            progressBar1.Visible = true;

                            using (Stream input = await response.Content.ReadAsStreamAsync())
                            using (FileStream output = File.Create(tempPath))
                            {
                                byte[] buffer = new byte[81920];
                                long downloadedBytes = 0;
                                int bytesRead;

                                while ((bytesRead = await input.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                {
                                    await output.WriteAsync(buffer, 0, bytesRead);

                                    downloadedBytes += bytesRead;

                                    if (totalBytes.HasValue)
                                    {
                                        double percentage =
                                            (double)downloadedBytes / totalBytes.Value * 100.0;

                                        progressBar1.Value = (int)percentage;
                                    }
                                }
                            }

                            progressBar1.Visible = false;
                        }

                        File.Move(tempPath, destinationPath);

                        string message = "To finish installing this plugin, you need to restart System Manager." +
                             "\n\nDo you want to restart System Manager now?";

                        if (SettingsDlg.IsActive)
                            message = "To finish installing this plugin, you need to restart System Manager." +
                             "\nAny unsaved setting changes will be lost.\nDo you want to restart System Manager now?";

                        var result = MessageBox.Show(message, "Restart required",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            Process.Start(Application.ExecutablePath, string.Join(" ", Environment.CommandLine.Split(' ').Skip(1)));
                            Application.Exit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Invoke(() => progressBar1.State = WindowsFormsAero.ProgressBarState.Error);

                    MessageBox.Show($"An error has occured: {ex.Message}.\n\nWe are sorry for the inconvienience.", null,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    Invoke(() => progressBar1.Visible = false);
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = listView1.SelectedItems.Cast<ListViewItem>().Any();
        }
    }

    public class RepositoryItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("sha")]
        public string SHA { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("git_url")]
        public string GitUrl { get; set; }

        [JsonProperty("download_url")]
        public string DownloadUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("_links")]
        public RepositoryItemLinks Links { get; set; }

        [JsonProperty("entries")]
        public List<RepositoryItem> Entries { get; set; }
    }

    public class RepositoryItemLinks
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("git")]
        public string Git { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }
    }

    public class PluginJsonInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }
    }

    public class PluginRepoInfo
    {
        public PluginJsonInfo Info { get; set; }
        public RepositoryItem Item { get; set; }

        public RepositoryItem DllItem { get; set; }
        public ListViewItem ListItem { get; set; }
    }

    public class GitBlob
    {
        [JsonProperty("sha")]
        public string SHA { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("encoding")]
        public string Encoding { get; set; }
    }
}
