using FireBlade.SystemManager.Helpers;
using FireBlade.SystemManager.Plugin;
using FireBlade.SystemManager.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadSettings();

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

            var renderer = new ToolStripProfessionalRenderer(new BlueColorTable());
            renderer.RoundedEdges = true;

            toolStrip1.Renderer = renderer;
            menuStrip1.Renderer = renderer;
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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (Settings.Default.ShowPageNameInTitleBar)
                this.Text = $"[{e.Node.Text}] System Manager";

            var children = e.Node.Nodes.Cast<TreeNode>();

            ToolStripManager.RevertMerge(menuStrip1);
            toolStrip1.Items.Clear();

            if (children.Any())
            {
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

                    contentPanel.Controls.Clear();
                    contentPanel.Controls.Add(content);

                    var tb = content.Controls.OfType<ToolStrip>().FirstOrDefault();
                    if (tb != null)
                    {
                        content.Controls.Remove(tb);

                        toolStrip1.Items.AddRange(tb.Items);
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
            var page = new FindPage(this);
            page.Dock = DockStyle.Fill;

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
        }

        private void systemManagerSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SettingsDlg();
            dlg.ShowDialog();

            LoadSettings();
        }

        private void viewInstalledPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SettingsDlg();
            
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
    }
}
