using FireBlade.SystemManager.Helpers;
using FireBlade.SystemManager.Plugin;
using FireBlade.SystemManager.Properties;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager
{
    public partial class Form1 : Form
    {
        public BlueColorTable _toolStripColorTable;

        public Form1()
        {
            InitializeComponent();

            imageListSmall.Images.Add(StockIconHelper.LoadIcon(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "ieframe.dll"),
                -100,
                false));

            imageListLarge.Images.Add(StockIconHelper.LoadIcon(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "ieframe.dll"),
                -100,
                true));

            pluginDocumentationToolStripMenuItem.Image = StockIconHelper.LoadIcon("Apphlpdm.dll", -2011, false).ToBitmap();

            //imageListSmall.Images.Add(Resources.Home16);
            //imageListLarge.Images.Add(Resources.Home32);

            _toolStripColorTable = new BlueColorTable();

            var renderer = new ToolStripProfessionalRenderer(_toolStripColorTable);
            renderer.RoundedEdges = true;

            toolStrip1.Renderer = renderer;
            menuStrip1.Renderer = renderer;

            LoadSettings();
        }

        private void treeView1_MouseMove(object sender, MouseEventArgs e)
        {
            TreeViewHitTestInfo hit = treeView1.HitTest(e.Location);

            treeView1.Cursor =
                hit.Node != null &&
                hit.Location is TreeViewHitTestLocations.Label &&
                hit.Node != treeView1.SelectedNode
                    ? CursorHelper.LoadCursor(CursorId.LinkSelect)
                    : Cursors.Default;
        }

        private const uint LVM_FIRST = 0x1000;
        private const uint LVM_SETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 54);
        private const uint LVM_SETHOVERTIME = (LVM_FIRST + 71);

        private const uint LVS_EX_ONECLICKACTIVATE = 0x00000040;
        private const uint LVS_EX_UNDERLINEHOT = 0x00000800;
        private const uint LVS_EX_LABELTIP = 0x00004000;
        private const uint LVS_EX_DOUBLEBUFFER = 0x00010000;
        private const uint LVS_EX_INFOTIP = 0x00000400;

        private IPageContract _currentPageContract = null;

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (Settings.Default.ShowPageNameInTitleBar)
                this.Text = $"[{e.Node.Text}] System Manager";

            var children = e.Node.Nodes.Cast<TreeNode>();

            ToolStripManager.RevertMerge(menuStrip1);
            ToolStripManager.RevertMerge(toolStrip1);

            toolStrip1.Items.Clear();

            if (children.Any())
            {
                _currentPageContract = null;

                var found = contentPanel.Controls.OfType<ListView>().FirstOrDefault();
                if (found != null)
                {
                    found.BeginUpdate();
                    found.Items.Clear();

                    foreach (var child in children)
                        found.Items.Add(new ListViewItem
                        {
                            Text = child.Text,
                            ImageIndex = child.ImageIndex,
                            Tag = child,
                            Group = found.Groups.Cast<ListViewGroup>().FirstOrDefault()
                        });

                    var grp = found.Groups.Cast<ListViewGroup>().FirstOrDefault();

                    if (grp != null)
                        grp.Header = e.Node.Text;
                    else
                        found.Groups.Add("listViewGroup1", e.Node.Text);

                    found.EndUpdate();
                }
                else
                {
                    // container node
                    var lv = new ListView
                    {
                        View = View.LargeIcon,
                        Dock = DockStyle.Fill,
                        Activation = ItemActivation.OneClick,
                        HotTracking = true,
                        LargeImageList = imageListLarge,
                        SmallImageList = imageListSmall,
                        HideSelection = false,
                        LabelWrap = true,
                        ShowItemToolTips = true,
                        ShowGroups = true,
                        Groups =
                        {
                            new ListViewGroup(e.Node.Text)
                        }
                    };

                    lv.BeginUpdate();

                    foreach (var child in children)
                        lv.Items.Add(new ListViewItem
                        {
                            Text = child.Text,
                            ImageIndex = child.ImageIndex,
                            Tag = child,
                            ToolTipText = child.ToolTipText,
                            Group = lv.Groups.Cast<ListViewGroup>().FirstOrDefault()
                        });

                    lv.EndUpdate();

                    lv.ItemActivate += Lv_ItemActivate;

                    contentPanel.Controls.Clear();
                    contentPanel.Controls.Add(lv);

                    NativeHelper.SendMessageW(lv.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, (UIntPtr)0u,
                        (IntPtr)(LVS_EX_DOUBLEBUFFER | LVS_EX_LABELTIP | LVS_EX_INFOTIP | LVS_EX_ONECLICKACTIVATE | LVS_EX_UNDERLINEHOT));

                    // prevent track selection
                    NativeHelper.SendMessageW(
                        lv.Handle,
                        LVM_SETHOVERTIME,
                        UIntPtr.Zero,
                        new IntPtr(int.MaxValue)); // note: we can't use uint.MaxValue - 1 because it overflows
                }
            }
            else
            {
                if (e.Node.Tag is IPageContract node)
                {
                    var content = node.CreateContent();
                    content.Dock = DockStyle.Fill;

                    _currentPageContract = node;

                    if (node.QueryContract(ContractIdentifiers.ThemeContractIdentifier) is IThemeContract contract)
                        contract.OnColorSchemeChanged(_toolStripColorTable.Scheme);

                    contentPanel.Controls.Clear();
                    contentPanel.Controls.Add(content);

                    var tb = content.Controls.OfType<ToolStrip>().FirstOrDefault();
                    if (tb != null)
                    {
                        content.Controls.Remove(tb);

                        //toolStrip1.Items.AddRange(tb.Items);
                        ToolStripManager.Merge(tb, toolStrip1);
                    }

                    var menu = content.Controls.OfType<MenuStrip>().FirstOrDefault();
                    if (menu != null)
                    {
                        content.Controls.Remove(menu);

                        ToolStripManager.Merge(menu, menuStrip1);
                    }
                }
            }

            if (Settings.Default.ShowPageIconInTaskBar)
            {
                int hr = ComMethods.CoInitializeEx(IntPtr.Zero, COINIT.APARTMENTTHREADED);
                if (hr >= 0)
                {
                    var taskbar = (ITaskbarList3)new TaskbarList();
                    Icon icon = null;

                    try
                    {
                        if (e.Node.Tag is IPageContract page)
                        {
                            if (page.QueryContract(ContractIdentifiers.IconContractIdentifier) is IIconContract contr)
                                contr.TryGetIcon(false, out icon); // small icon
                        }
                        else if (e.Node.Tag is IPageContainerContract cont)
                        {
                            if (cont.QueryContract(ContractIdentifiers.IconContractIdentifier) is IIconContract contr)
                                contr.TryGetIcon(false, out icon);
                        }
                        else
                        {
                            // hardcoded for now until we store a dictionary of all the tree node icons

                            icon = StockIconHelper.LoadIcon(
                                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "ieframe.dll"),
                                -100,
                                false);
                        }

                        if (icon != null)
                        {
                            taskbar.HrInit();
                            taskbar.SetOverlayIcon(Handle, icon.Handle, e.Node.Text);
                        }
                    }
                    finally
                    {
                        ComMethods.CoUninitialize();
                        icon.Dispose(); // the taskbar will copy the icon
                    }
                }
            }
        }

        private void Lv_ItemActivate(object sender, EventArgs e)
        {
            var lv = sender as ListView;
            if (lv != null)
            {
                var selectedItem = lv.SelectedItems[0];

                if (selectedItem.Tag is TreeNode node)
                    treeView1.SelectedNode = node;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            var rootNode = new TreeNode
            {
                Text = "Home",
                ImageIndex = 0,
                SelectedImageIndex = 0,
                Tag = "root",
                /*
                Nodes =
                {
                    new TreeNode
                    {
                        Text = "Subitem 1",
                        ImageIndex = 0,
                        SelectedImageIndex = 0
                    },
                    new TreeNode
                    {
                        Text = "Subitem 2",
                        ImageIndex = 0,
                        SelectedImageIndex = 0
                    },
                    new TreeNode
                    {
                        Text = "Subitem 3",
                        ImageIndex = 0,
                        SelectedImageIndex = 0,
                        Nodes =
                        {
                            new TreeNode
                            { 
                                Text = "Subitem 4",
                                ImageIndex = 0,
                                SelectedImageIndex = 0
                            }
                        }
                    },
                }
                */
            };

            treeView1.Nodes.Add(rootNode);

            PluginHelper.LoadPlugins(this);
            treeView1.SelectedNode = rootNode;

            _ = CheckUpdates(false);
        }

        private static Icon BitmapToIcon(Bitmap bitmap)
        {
            IntPtr hIcon = bitmap.GetHicon();

            try
            {
                using Icon temp = Icon.FromHandle(hIcon);
                return (Icon)temp.Clone();
            }
            finally
            {
                DestroyIcon(hIcon);
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        private void Form1_Activated(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
                goto nullNode;

            if (Settings.Default.ShowPageIconInTaskBar)
            {
                int hr = ComMethods.CoInitializeEx(IntPtr.Zero, COINIT.APARTMENTTHREADED);
                if (hr >= 0)
                {
                    var taskbar = (ITaskbarList3)new TaskbarList();
                    Icon icon = null;

                    try
                    {
                        if (treeView1.SelectedNode.Tag is IPageContract page)
                        {
                            if (page.QueryContract(ContractIdentifiers.IconContractIdentifier) is IIconContract contr)
                                contr.TryGetIcon(false, out icon); // small icon
                        }
                        else if (treeView1.SelectedNode.Tag is IPageContainerContract cont)
                        {
                            if (cont.QueryContract(ContractIdentifiers.IconContractIdentifier) is IIconContract contr)
                                contr.TryGetIcon(false, out icon);
                        }
                        else
                        {
                            // hardcoded for now until we store a dictionary of all the tree node icons

                            icon = StockIconHelper.LoadIcon(
                                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "ieframe.dll"),
                                -100,
                                false);
                        }

                        if (icon != null)
                        {
                            taskbar.HrInit();
                            taskbar.SetOverlayIcon(Handle, icon.Handle, treeView1.SelectedNode.Text);
                        }
                    }
                    finally
                    {
                        ComMethods.CoUninitialize();
                        icon.Dispose(); // the taskbar will copy the icon
                    }
                }
            }

            goto exit;

        nullNode:
            BeginInvoke(() => treeView1.SelectedNode = null);

        exit:
            return;
        }

        private void findPagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge(menuStrip1);
            ToolStripManager.RevertMerge(toolStrip1);

            var page = new FindPage(this);
            page.Dock = DockStyle.Fill;

            _currentPageContract = null;
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(page);

            treeView1.SelectedNode = null;

            if (Settings.Default.ShowPageIconInTaskBar)
            {
                int hr = ComMethods.CoInitializeEx(IntPtr.Zero, COINIT.APARTMENTTHREADED);
                if (hr >= 0)
                {
                    var taskbar = (ITaskbarList3)new TaskbarList();
                    Icon icon = BitmapToIcon(Resources.Find16);

                    try
                    {
                        if (icon != null)
                        {
                            taskbar.HrInit();
                            taskbar.SetOverlayIcon(Handle, icon.Handle, "Find Pages");
                        }
                    }
                    finally
                    {
                        ComMethods.CoUninitialize();
                        icon.Dispose(); // the taskbar will copy the icon
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddSuggestionsRecursive(TextBox box, TreeNodeCollection nodes)
        {
            foreach (var n in nodes.Cast<TreeNode>())
            {
                box.AutoCompleteCustomSource.Add(n.FullPath);
                box.AutoCompleteCustomSource.Add(n.FullPath.Replace('\\', '/'));

                AddSuggestionsRecursive(box, n.Nodes);
            }
        }

        private void goToPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new GoDialog();
            AddSuggestionsRecursive(dlg.textBox1, treeView1.Nodes);

            foreach (var node in treeView1.Nodes.Cast<TreeNode>().First().Nodes.Cast<TreeNode>()) // add root nodes
                dlg.textBox1.AutoCompleteCustomSource.Add(node.Text);

            var result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                string path = dlg.textBox1.Text;
                var split = path.Replace('\\', '/').Split('/');

                TreeNode current = treeView1.Nodes.Cast<TreeNode>().First();

                foreach (var item in split)
                {
                    if (item.Equals("Home", StringComparison.OrdinalIgnoreCase)) continue;

                    var found = current.Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Text.Equals(item, StringComparison.OrdinalIgnoreCase));
                    if (found != null)
                        current = found;
                    else
                        break;
                }

                treeView1.SelectedNode = current;
            }
        }

        private void systemManagerGitHubRepositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/FireBlade211/System-Manager");
        }

        private void systemManagerPluginRepositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/FireBlade211/System-Manager-Plugins");
        }

        private void aboutSystemManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new AboutDlg();
            dlg.ShowDialog();
        }

        private void LoadSettings()
        {
            if (!Settings.Default.ShowPageIconInTaskBar)
            {
                int hr = ComMethods.CoInitializeEx(IntPtr.Zero, COINIT.APARTMENTTHREADED);
                if (hr >= 0)
                {
                    var taskbar = (ITaskbarList3)new TaskbarList();

                    try
                    {
                        taskbar.HrInit();
                        taskbar.SetOverlayIcon(Handle, IntPtr.Zero, string.Empty);
                    }
                    finally
                    {
                        ComMethods.CoUninitialize();
                    }
                }
            }

            if (!Settings.Default.ShowPageNameInTitleBar)
                this.Text = "System Manager";

            _toolStripColorTable.Scheme = (ColorScheme)Settings.Default.ColorScheme;
            
            if (_currentPageContract?.QueryContract(ContractIdentifiers.ThemeContractIdentifier) is IThemeContract contract)
                contract.OnColorSchemeChanged(_toolStripColorTable.Scheme);
        }

        public void RedrawAllToolbars()
        {
            toolStrip1.Refresh();
            menuStrip1.Refresh();

            if (_currentPageContract?.QueryContract(ContractIdentifiers.ThemeContractIdentifier) is IThemeContract contract)
                contract.OnColorSchemeChanged(_toolStripColorTable.Scheme);
        }

        private void systemManagerSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SettingsDlg(this);
            dlg.ShowDialog();

            LoadSettings();
        }

        private void viewInstalledPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SettingsDlg(this);
            
            dlg.NavigateToPlugins();
            dlg.ShowDialog();

            LoadSettings();
        }

        private void getMorePluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new PluginGalleryForm();
            dlg.ShowDialog();
        }

        private void pluginDocumentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // For some reason by default this doesn't go to our home page even though it was specified in the project file
            // so we do it manually
            Help.ShowHelp(this, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "docs.chm"), HelpNavigator.Topic, "home.htm");
        }

        private async Task CheckUpdates(bool showProgress = true)
        {
            SimpleProgressDialog dlg = null;

            if (showProgress)
            {
                dlg = SimpleProgressDialog.CreateDialog("Checking for updates", "Check for Updates", true);
                dlg.AllowCancel = false;
                dlg.ShowDialog(this);
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(Settings.Default.GitHubToken))
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                            SettingsDlg.UnprotectToken(Settings.Default.GitHubToken));

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var ver = Assembly.GetCallingAssembly().GetName().Version;

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("System-Manager",
                        $"{ver.Major}.{ver.Minor}"));

                    var response = await client.GetAsync("https://api.github.com/repos/FireBlade211/System-Manager/releases/latest");
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();

                    var release = JsonConvert.DeserializeObject<JsonReleaseInfo>(json);
                    var latest = Version.Parse(release.TagName.TrimStart('v'));

                    // Check if it's a later release.
                    if (latest > ver)
                    {
                        var result = MessageBox.Show($"An update is available. ({release.TagName}).\n\nDo you want to download it now?",
                            "Update available", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                        if (result == DialogResult.OK)
                        {
                            // For now, just send them to the download page (we can implement auto-installation later)
                            Process.Start(release.HtmlUrl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while checking for updates:\n\n{ex.Message}", null, MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            dlg?.Close();
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
           _ = CheckUpdates();
        }
    }

    public class JsonReleaseInfo
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("assets_url")]
        public string AssetsUrl { get; set; }

        [JsonProperty("upload_url")]
        public string UploadUrl { get; set; }

        [JsonProperty("tarball_url")]
        public string TarballUrl { get; set; }

        [JsonProperty("zipball_url")]
        public string ZipballUrl { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("node_id")]
        public string NodeId { get; set; }

        [JsonProperty("tag_name")]
        public string TagName { get; set; }

        [JsonProperty("target_commitish")]
        public string TargetCommitish { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("draft")]
        public bool Draft { get; set; }

        [JsonProperty("prerelease")]
        public bool Prerelease { get; set; }

        [JsonProperty("immutable")]
        public bool Immutable { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("published_at")]
        public DateTime PublishedAt { get; set; }

        [JsonProperty("author")]
        public JsonReleaseAuthorInfo Author { get; set; }

        [JsonProperty("assets")]
        public JsonReleaseAssetInfo[] Assets { get; set; }
    }

    public class JsonReleaseAuthorInfo
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("node_id")]
        public string NodeId { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("gravatar_id")]
        public string GravatarId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("followers_url")]
        public string FollowersUrl { get; set; }

        [JsonProperty("following_url")]
        public string FollowingUrl { get; set; }

        [JsonProperty("gists_url")]
        public string GistsUrl { get; set; }

        [JsonProperty("starred_url")]
        public string StarredUrl { get; set; }

        [JsonProperty("subscriptions_url")]
        public string SubscriptionsUrl { get; set; }

        [JsonProperty("organizations_url")]
        public string OrganizationsUrl { get; set; }

        [JsonProperty("repos_url")]
        public string ReposUrl { get; set; }

        [JsonProperty("events_url")]
        public string EventsUrl { get; set; }

        [JsonProperty("received_events_url")]
        public string ReceivedEventsUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("site_admin")]
        public bool SiteAdmin { get; set; }
    }

    public class JsonReleaseAssetInfo
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("browser_download_url")]
        public string BrowserDownloadUrl { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("node_id")]
        public string NodeId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("digest")]
        public string Digest { get; set; }

        [JsonProperty("download_count")]
        public int DownloadCount { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("uploader")]
        public JsonReleaseAuthorInfo Uploader { get; set; }
    }
}
