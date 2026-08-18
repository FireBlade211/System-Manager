using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsAero.TaskDialog;

namespace FireBlade.SystemManager.Helpers
{
    public static class PluginHelper
    {
        public static Dictionary<Assembly, IPlugin> LoadedPlugins = new Dictionary<Assembly, IPlugin>();

        public static void LoadPlugins(Form1 mainForm)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
            if (!Directory.Exists(path)) return;

            // Remove incomplete plugins.
            foreach (string file in Directory.EnumerateFiles(path, "*.tmp"))
            {
                try
                {
                    File.Delete(file);
                }
                catch { }
            }

            // Update plugins.
            foreach (string file in Directory.EnumerateFiles(path, "*.dll.upgrade"))
            {
                try
                {
                    string dll = file.Substring(0, file.Length - ".upgrade".Length);

                    if (File.Exists(dll))
                        File.Delete(dll);

                    File.Move(file, dll);
                }
                catch { }
            }

            // Load plugins
            foreach (var dll in Directory.EnumerateFiles(path, "*.dll"))
            {
                try
                {
                    Assembly asm = Assembly.LoadFrom(dll);
                    if (asm != null)
                    {
                        var types = asm.GetTypes();
                        var contrPlugin = types.FirstOrDefault(x => typeof(IPlugin).IsAssignableFrom(x) &&
                            !x.IsInterface &&
                            !x.IsAbstract);

                        if (contrPlugin != null)
                        {
                            var plugin = (IPlugin)Activator.CreateInstance(contrPlugin);
                            LoadedPlugins.Add(asm, plugin);

                            if (plugin.QueryContract(ContractIdentifiers.PluginInfoContractIdentifier) is IPluginInfoContract info)
                            {
                                //MessageBox.Show($"{info.Name}\n\n{info.Description}", "Loaded plugin");
                                // TODO: Do shit with this
                            }

                            if (plugin.QueryContract(ContractIdentifiers.PluginTreeContractIdentifier) is IPluginTreeContract tree)
                            {
                                var nodes = tree.GetNodes(out string parentId);
                                foreach (var node in nodes)
                                {
                                    if (node.QueryContract(ContractIdentifiers.IconContractIdentifier) is IIconContract iconContract)
                                    {
                                        if (iconContract.TryGetIcon(false, out Icon smallIcon) && iconContract.TryGetIcon(true,
                                            out Icon largeIcon))
                                        {
                                            mainForm.imageListSmall.Images.Add(smallIcon);
                                            mainForm.imageListLarge.Images.Add(largeIcon);
                                        }
                                    }

                                    var tn = new TreeNode
                                    {
                                        Text = node.DisplayName,
                                        ToolTipText = node.Description,
                                        ImageIndex = mainForm.imageListSmall.Images.Count - 1,
                                        SelectedImageIndex = mainForm.imageListSmall.Images.Count - 1,
                                        Tag = node
                                    };

                                    var parent = FindParentRecursive(mainForm.treeView1.Nodes, parentId);
                                    var found = parent.Nodes.Cast<TreeNode>().FirstOrDefault(x =>
                                    (x.Tag as IPageContainerContract)?.Id.Equals(node.Id, StringComparison.Ordinal) == true);

                                    if (found != null && node is IPageContainerContract container)
                                        AddNodesRecursive(mainForm, found, container);
                                    else
                                    {
                                        if (node is IPageContainerContract c)
                                            AddNodesRecursive(mainForm, tn, c);

                                        parent.Nodes.Add(tn);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    var sb = new StringBuilder();

                    sb.AppendLine("The loader exceptions are as follows:");
                    sb.AppendLine();

                    foreach (var loaderEx in ex.LoaderExceptions)
                    {
                        sb.AppendLine($"- 0x{loaderEx.HResult:X}: {loaderEx.Message}");
                        sb.AppendLine();
                    }

                    var dlg = new TaskDialog("A plugin could not be loaded", "Failed to load plugin",
                         "There was an error loading the plugin:" +
                        $" <a href=\"open-loc\">{Path.GetFileName(dll)}</a>.\n\n0x{ex.HResult:X}: {ex.Message}");

                    dlg.AllowDialogCancellation = true;
                    dlg.CommonIcon = CommonIcon.SecurityError;
                    dlg.ExpandedInformation = sb.ToString();
                    dlg.EnableHyperlinks = true;

                    dlg.HyperlinkClick += (s, e) =>
                    {
                        if (e.Url.Equals("open-loc", StringComparison.OrdinalIgnoreCase))
                            Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "explorer.exe"),
                                "/select,\"" + dll + "\"");
                    };

                    dlg.Show(mainForm);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"There was an error loading the plugin: {Path.GetFileName(dll)}.\n\n0x{ex.HResult:X}: {ex.Message}",
                        null /* default to error */, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static TreeNode FindParentRecursive(TreeNodeCollection nodes, string tag)
        { 
            foreach (var node in nodes.Cast<TreeNode>())
            {
                if (node.Tag is string nodeId && nodeId.Equals(tag, StringComparison.Ordinal))
                    return node;

                var parent = FindParentRecursive(node.Nodes, tag);
                if (parent != null)
                    return parent;
            }

            return null;
        }

        private static void AddNodesRecursive(Form1 mainForm, TreeNode root, IPageContainerContract container, int depth = 0)
        {
            if (depth++ >= 256) return;

            foreach (var node in container.GetChildren())
            {
                var found = root.Nodes.Cast<TreeNode>().FirstOrDefault(x =>
                (x.Tag as IPageContainerContract)?.Id.Equals(node.Id, StringComparison.Ordinal) == true);

                if (found != null)
                {
                    if (node is IPageContainerContract co)
                        AddNodesRecursive(mainForm, found, co, depth);

                    continue;
                }

                if (node.QueryContract(ContractIdentifiers.IconContractIdentifier) is IIconContract icon)
                    if (icon.TryGetIcon(false, out Icon smallIcon) && icon.TryGetIcon(true, out Icon largeIcon))
                    {
                        mainForm.imageListSmall.Images.Add(smallIcon);
                        mainForm.imageListLarge.Images.Add(largeIcon);
                    }

                var n = new TreeNode
                {
                    Text = node.DisplayName,
                    ToolTipText = node.Description,
                    ImageIndex = mainForm.imageListSmall.Images.Count - 1,
                    SelectedImageIndex = mainForm.imageListSmall.Images.Count - 1,
                    Tag = node
                };

                root.Nodes.Add(n);

                if (node is IPageContainerContract c) AddNodesRecursive(mainForm, n, c, depth);
            }
        }
    }
}
