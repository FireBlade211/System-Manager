using FireBlade.SystemManager.Helpers;
using FireBlade.SystemManager.Plugin;
using FireBlade.SystemManager.Properties;
using Markdig;
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
using System.Windows.Forms;
using System.Xml.Linq;
using HAP = HtmlAgilityPack;

namespace FireBlade.SystemManager
{
    public partial class PluginGalleryForm : Form
    {
        private static readonly MarkdownPipeline _markdownPipeline = new MarkdownPipelineBuilder()
            .UseAbbreviations()
            .UseAutoLinks()
            .UsePipeTables()
            .UseTaskLists()
            .UseFootnotes()
            .UseEmphasisExtras()
            .UseDefinitionLists()
            .UseListExtras()
            .UseEmojiAndSmiley()
            .UseFigures()
            .UseGenericAttributes()
            .UseSmartyPants()
            .UseCitations()
            .UseCustomContainers()
            .UseMathematics()
            .UseMediaLinks()
            .UseNonAsciiNoEscape()
            .Build();

        public PluginGalleryForm()
        {
            InitializeComponent();

            DeserializeSources(PluginGallery.Default.PluginSources);
            comboBox1.SelectedIndex = 0;
        }

        private string _idToSelect;

        public void SetItemToSelect(string id)
        {
            _idToSelect = id;
        }

        public string RepositoryName
        {
            get
            {
                if (comboBox1.SelectedIndex != 0)
                    if (comboBox1.SelectedItem is PluginSourceItem src)
                        return src.Repository;

                return "FireBlade211/System-Manager-Plugins";
            }
        }

        private async void LoadPlugins()
        {
            button5.Enabled = false;

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

                    using var response = await client.GetAsync($"https://api.github.com/repos/{RepositoryName}/contents/plugins");
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

                            using var response2 = await client.GetAsync(entry.Url);

                            if (response2.IsSuccessStatusCode)
                            {
                                json = await response2.Content.ReadAsStringAsync();
                                var info = JsonConvert.DeserializeObject<PluginJsonInfo>(json);

                                var i = new PluginRepoInfo
                                {
                                    Info = info,
                                    Item = entry,
                                    ExtraInfo = new PluginExtraInfo
                                    {
                                        IconUrl = entry.DownloadUrl.Substring(
                                            0,
                                            entry.DownloadUrl.LastIndexOf('.', entry.DownloadUrl.LastIndexOf('.') - 1)
                                        ) + ".ico"
                                    }
                                };

                                var lvi = new ListViewItem
                                {
                                    Text = info.Name,
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

                    foreach (var entry in root.Entries)
                    {
                        if (!Path.GetExtension(entry.Name).Equals(
                            ".zip", StringComparison.OrdinalIgnoreCase))
                            continue;

                        string srcName = entry.Name;
                        string manifestName = Path.GetFileNameWithoutExtension(entry.Name) + ".dll.json";

                        PluginRepoInfo info = infos.FirstOrDefault(x =>
                            x.Item.Name.Equals(manifestName, StringComparison.OrdinalIgnoreCase));
                        
                        info?.ExtraInfo.SourceCodeUrl =
                            $"https://raw.githubusercontent.com/{RepositoryName}/refs/heads/main/plugins/{srcName}";
                    }

                    foreach (var entry in root.Entries)
                    {
                        if (!Path.GetExtension(entry.Name).Equals(
                            ".md", StringComparison.OrdinalIgnoreCase))
                            continue;

                        string readmeName = entry.Name;
                        string manifestName = Path.GetFileNameWithoutExtension(entry.Name) + ".dll.json";

                        PluginRepoInfo info = infos.FirstOrDefault(x =>
                            x.Item.Name.Equals(manifestName, StringComparison.OrdinalIgnoreCase));

                        string url = $"https://raw.githubusercontent.com/{RepositoryName}/refs/heads/main/plugins/{readmeName}";

                        using var r = await client.GetAsync(url);

                        if (r.IsSuccessStatusCode)
                            info.ExtraInfo.Readme = await r.Content.ReadAsStringAsync();
                    }

                    foreach (var entry in root.Entries)
                    {
                        if (!entry.Name.EndsWith(
                            ".license.txt", StringComparison.OrdinalIgnoreCase))
                            continue;

                        string licenseName = entry.Name;
                        string manifestName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(entry.Name)) + ".dll.json";

                        PluginRepoInfo info = infos.FirstOrDefault(x =>
                            x.Item.Name.Equals(manifestName, StringComparison.OrdinalIgnoreCase));

                        string url = $"https://raw.githubusercontent.com/{RepositoryName}/refs/heads/main/plugins/{licenseName}";

                        using var r = await client.GetAsync(url);

                        if (r.IsSuccessStatusCode)
                            info.ExtraInfo.License = await r.Content.ReadAsStringAsync();
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

                    foreach (var item in listView1.Items.Cast<ListViewItem>())
                    {
                        var info = (item.Tag as PluginRepoInfo).ExtraInfo;

                        using HttpResponseMessage resp = await client.GetAsync(info.IconUrl);

                        if (resp.IsSuccessStatusCode)
                        {
                            byte[] data = await resp.Content.ReadAsByteArrayAsync();
                            using MemoryStream ms = new MemoryStream(data);

                            Icon icon = new Icon(ms);
                            info.LoadedIcon = icon;

                            imageList1.Images.Add(icon);
                            imageList2.Images.Add(icon);

                            item.ImageIndex = imageList1.Images.Count - 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"An error has occured: {ex.Message}.\n\nWe are sorry for the inconvienience.", null,
                //    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            button5.Enabled = true;
        }

        private void PluginGalleryForm_Load(object sender, EventArgs e)
        {
            //LoadPlugins();
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
                            $"https://raw.githubusercontent.com/{RepositoryName}/refs/heads/main/{info.DllItem.Path}",
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
                            Application.Restart();
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

        private static string FormatFileSize(long bytes)
        {
            if (bytes < 1000)
                return $"{bytes} B";

            string[] units = { "KB", "MB", "GB", "TB", "PB", "EB" };

            double size = bytes;
            int unit = -1;

            do
            {
                size /= 1000;
                unit++;
            }
            while (size >= 1000 && unit < units.Length - 1);

            return $"{size:0.##} {units[unit]}";
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = listView1.SelectedItems.Cast<ListViewItem>().Any();
            webBrowser1.Navigating -= webBrowser1_Navigating;

            if ((listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.Tag) is PluginRepoInfo info)
            {
                label1.Text = info.Info.Name;
                label2.Text = info.Info.Description;
                toolTip1.SetToolTip(label2, info.Info.Description);

                label3.Text = $"ID: {Path.GetFileNameWithoutExtension(info.DllItem.Name)}";
                label4.Text = $"Size: {FormatFileSize(info.DllItem.Size)}";
                label5.Text = $"SHA-1 (Git blob): {info.DllItem.SHA}";
                label6.Text = $"Author: {info.Info.Author}";
                label7.Text = $"Version: {info.Info.Version}";

                textBox1.Text = string.IsNullOrEmpty(info.ExtraInfo.License) ? "No license found" : info.ExtraInfo.License;

                if (info.ExtraInfo.LoadedIcon != null)
                {
                    // Convert manually because ToBitmap sucks
                    //Bitmap bitmap = new Bitmap(32, 32);

                    //using (Graphics g = Graphics.FromImage(bitmap))
                    //{
                    //    g.DrawIcon(info.ExtraInfo.LoadedIcon, 0, 0);
                    //}

                    //pictureBox1.Image = bitmap;

                    pictureBox1.Image = imageList2.Images[info.ListItem.ImageIndex];
                }

                if (!string.IsNullOrEmpty(info.ExtraInfo.Readme))
                    webBrowser1.DocumentText = FixShittyMarkdownHtml(Markdown.ToHtml(info.ExtraInfo.Readme, _markdownPipeline));
            }
            else
            {
                label1.Text = "Name";
                label2.Text = "Description";
                label3.Text = "ID: ";
                label4.Text = "SIze: ";
                label5.Text = "SHA-1 (Git blob): ";
                label6.Text = "Author: ";
                label7.Text = "Version: ";
                textBox1.Text = string.Empty;

                pictureBox1.Image = null;
                webBrowser1.DocumentText = string.Empty;
            }

            webBrowser1.Navigating += webBrowser1_Navigating;
        }

        private string FixShittyMarkdownHtml(string html)
        {
            HAP.HtmlDocument doc = new HAP.HtmlDocument();
            doc.LoadHtml($@"
<html>
    <head>
    </head>
    <body>
        {html}
    </body>
</html>");

            HAP.HtmlNode head = doc.DocumentNode.SelectSingleNode("//head");

            if (head == null)
            {
                head = doc.CreateElement("head");
                doc.DocumentNode.AppendChild(head);
            }

            HAP.HtmlNode style = doc.CreateElement("style");
            style.InnerHtml = @"
                body {
                    font-family: Tahoma, Arial, sans-serif;
                    font-size: 9pt;
                }

                h1 {
                    font-size: 12pt;
                    font-family: Verdana, Arial, sans-serif;
                    font-weight: bold;
                }

                h2 {
                    font-size: 11pt;
                    font-family: Verdana;
                }

                a {
                   color: blue;
                }

                a:hover {
                    /* font-size: 10pt; */
                    font-weight: bold;
                    color: red;
                }

                code {
                    font-family: Consolas, monospace;
                }

                table {
                    border-collapse: collapse;
                    width: 100%;
                }

                th {
                    border: 1px solid gray;
                    padding: 2px;
                }

                td {
                    border: 1px solid gray;
                    padding: 4px;
                }
            ";

            head.AppendChild(style);

            HAP.HtmlNode body = doc.DocumentNode.SelectSingleNode("//body");

            if (body != null)
            {
                // Sanitize all images.
                foreach (HAP.HtmlNode img in body.SelectNodes(".//img") ?? Enumerable.Empty<HAP.HtmlNode>())
                {
                    string src = img.GetAttributeValue("src", null);

                    if (!IsAllowedImageUrl(src))
                        img.Remove();
                }
            }

            using (StringWriter writer = new StringWriter())
            {
                doc.Save(writer);
                return writer.ToString();
            }
        }

        private bool IsAllowedImageUrl(string src)
        {
            if (!Uri.TryCreate(src, UriKind.Absolute, out Uri uri))
                return false;

            if (uri.Scheme != Uri.UriSchemeHttps)
                return false;

            return uri.Host.Equals("raw.githubusercontent.com",
                StringComparison.OrdinalIgnoreCase)
                || uri.Host.Equals("imgur.com",
                    StringComparison.OrdinalIgnoreCase)
                || uri.Host.EndsWith(".imgur.com",
                    StringComparison.OrdinalIgnoreCase);
        }

        private bool _expanded = true;

        private void button3_Click(object sender, EventArgs e)
        {
            _expanded = !_expanded;
            button3.Text = _expanded ? "<< &Hide details" : "&Show details >>";

            _startSize = Size;
            _targetSize = _expanded ? new Size(948, 444) : new Size(615, 444);

            _animationStart = DateTime.UtcNow;
            _animationDuration = 100;

            timer1.Start();
        }

        private Size _startSize;
        private Size _targetSize;

        private DateTime _animationStart;
        private double _animationDuration;

        private void timer1_Tick(object sender, EventArgs e)
        {
            double t = (DateTime.UtcNow - _animationStart).TotalMilliseconds / _animationDuration;

            if (t >= 1.0)
            {
                Size = _targetSize;
                timer1.Stop();
                return;
            }

            int width = (int)(_startSize.Width +
                (_targetSize.Width - _startSize.Width) * t);

            int height = (int)(_startSize.Height +
                (_targetSize.Height - _startSize.Height) * t);

            Size = new Size(width, height);
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if ((listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.Tag) is PluginRepoInfo info)
            {
                if (string.IsNullOrEmpty(info.ExtraInfo.SourceCodeUrl))
                {
                    MessageBox.Show("The source code for this plugin is not available.\nThe creator of this plugin hasn't provided source code.",
                        "Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (sourceCodeFileDlg.ShowDialog() == DialogResult.OK)
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

                        button4.Enabled = false;

                        using var response = await client.GetAsync(info.ExtraInfo.SourceCodeUrl, HttpCompletionOption.ResponseHeadersRead);

                        if (response.IsSuccessStatusCode)
                        {
                            long? totalBytes = response.Content.Headers.ContentLength;

                            progressBar2.Visible = true;

                            using (Stream input = await response.Content.ReadAsStreamAsync())
                            using (FileStream output = new FileStream(sourceCodeFileDlg.FileName, FileMode.Create, FileAccess.Write))
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

                                        progressBar2.Value = (int)percentage;
                                    }
                                }
                            }

                            progressBar2.Visible = false;
                            button4.Enabled = true;

                            var result = MessageBox.Show("The source code has been downloaded successfully.\nDo you want" +
                                " to open it now?", "Download", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);

                            if (result == DialogResult.Yes)
                                Process.Start(Path.Combine(KF(Environment.SpecialFolder.System), "EXPLORER.exe"),
                                    $"/select,\"{sourceCodeFileDlg.FileName}\"");
                        }
                        else
                        {
                            MessageBox.Show($"Failed to download the source code.\n\n{((uint)response.StatusCode)}: {response.ReasonPhrase}",
                                null, MessageBoxButtons.OK, MessageBoxIcon.Error);

                            progressBar2.Visible = false;
                            button4.Enabled = true;

                        }
                    }
                }
            }
        }

        private string KF(Environment.SpecialFolder fldr) => Environment.GetFolderPath(fldr);

        private void button5_Click(object sender, EventArgs e)
        {
            LoadPlugins();
        }

        private static readonly HashSet<string> _allowedNavigationSchemes =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "mailto",
                "ms-calculator",
                "ms-calendar",
                "ms-clock",
                "ms-photos",
                "ms-settings",
                "ms-windows-store"
            };

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.Equals(new Uri("about:blank"))) return;

            e.Cancel = true;

            if (_allowedNavigationSchemes.Contains(e.Url.Scheme.ToLowerInvariant()))
            {
                Process.Start(e.Url.ToString());
                return;
            }

            if (MessageBox.Show($"Are you sure you want to open the following link?\n\n{e.Url}", "Security warning",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                Process.Start(e.Url.ToString());
        }

        private int _lastSourceIdx = 0;
        private List<PluginSourceItem> _sources = new List<PluginSourceItem>();

        private string SerializeSources()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var src in _sources)
            {
                if (src.Temp) continue;

                // note: the user cannot set these to contain newlines
                sb.AppendLine(src.Name);
                sb.AppendLine(src.Repository);
            }

            return sb.ToString();
        }

        private void DeserializeSources(string data)
        {
            var lines = data
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToArray();

            for (int i = 0; i + 1 < lines.Length; i += 2)
            {
                var item = new PluginSourceItem
                {
                    Name = lines[i],
                    Repository = lines[i + 1],
                    Temp = false
                };

                _sources.Add(item);
                comboBox1.Items.Insert(comboBox1.Items.Count - 3, item);
            }
        }

        private bool _sourceSelLock = false;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.BeginUpdate();
            int sel = comboBox1.SelectedIndex;

            if (sel >= comboBox1.Items.Count - 2 - 1)
            {
                _sourceSelLock = true;

                comboBox1.SelectedIndex = _lastSourceIdx;
                comboBox1.EndUpdate();

                if (sel == comboBox1.Items.Count - 1 - 1)
                {
                    var dlg = new AddPluginSourceForm();
                    var result = dlg.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        var item = new PluginSourceItem
                        {
                            Name = dlg.textBox1.Text,
                            Repository = dlg.textBox2.Text,
                            Temp = dlg.checkBox1.Checked
                        };

                        comboBox1.Items.Insert(comboBox1.Items.Count - 3, item);
                        _sources.Add(item);

                        PluginGallery.Default.PluginSources = SerializeSources();
                        PluginGallery.Default.Save();
                    }
                }
                else if (sel == comboBox1.Items.Count - 1 - 0)
                {
                    var dlg = new EditPluginSourcesForm(_sources);
                    dlg.ShowDialog();

                    comboBox1.BeginUpdate();

                    comboBox1.Items.Clear();

                    comboBox1.Items.Add("Official Repository");
                    comboBox1.Items.Add("--------------------------------------");
                    comboBox1.Items.Add("Add new source...");
                    comboBox1.Items.Add("Edit sources...");

                    PluginGallery.Default.PluginSources = SerializeSources();
                    PluginGallery.Default.Save();

                    _sources.Clear();
                    DeserializeSources(PluginGallery.Default.PluginSources);

                    comboBox1.SelectedIndex = _lastSourceIdx;
                    comboBox1.EndUpdate();
                }

                _sourceSelLock = false;
                return;
            }
            else
                comboBox1.EndUpdate();

            _lastSourceIdx = sel;

            if (!_sourceSelLock)
                LoadPlugins();
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

        public PluginExtraInfo ExtraInfo { get; set; } = new PluginExtraInfo();
    }

    public class PluginExtraInfo
    {
        public string IconUrl { get; set; }
        public Icon LoadedIcon { get; set; }
        public string SourceCodeUrl { get; set; } = string.Empty;
        public string Readme { get; set; }
        public string License { get; set; }
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

    public class PluginSourceItem
    {
        public string Name { get; set; }
        public string Repository { get; set; }
        public bool Temp { get; set; }

        public override string ToString() => Name;
    }
}
