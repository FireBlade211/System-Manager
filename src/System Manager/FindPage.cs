using FireBlade.SystemManager.Helpers;
using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace FireBlade.SystemManager
{
    public partial class FindPage : UserControl
    {
        private Form1 _form;
        private SearchListView _lv;

        public FindPage()
        {
            InitializeComponent();

            elementHost1.Child = _lv = new SearchListView();
            _lv.OnItemClick += Lv_OnItemClick;
        }

        private void Lv_OnItemClick(object sender, SearchEntryInfo e)
        {
            _form.treeView1.SelectedNode = e.Node;
        }

        public FindPage(Form1 form)
        {
            InitializeComponent();
            _form = form;

            elementHost1.Child = _lv = new SearchListView();
            _lv.OnItemClick += Lv_OnItemClick;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<TreeNode> found = new List<TreeNode>();
            FindNodesRecursive(_form.treeView1.Nodes, textBox1.Text, found);

            List<SearchEntryInfo> results = new List<SearchEntryInfo>();

            _lv.Results.Clear();

            foreach (var node in found)
            {
                var info = new SearchEntryInfo();
                info.Name = node.Text;
                info.Description = node.ToolTipText;
                info.Node = node;

                _lv.Results.Add(info);
                results.Add(info);
            }

            Parallel.ForEach(
                results,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = 4
                },
                LoadIcon);
        }

        private void LoadIcon(SearchEntryInfo entry)
        {
            Icon icon = null;

            if (entry.Node.Tag is IPageContract page)
            {
                if (page.QueryContract(ContractIdentifiers.IconContractIdentifier) is IIconContract contr)
                    contr.TryGetIcon(true, out icon); // large icon
            }
            else if (entry.Node.Tag is IPageContainerContract cont)
            {
                if (cont.QueryContract(ContractIdentifiers.IconContractIdentifier) is IIconContract contr)
                    contr.TryGetIcon(true, out icon);
            }
            else
            {
                // hardcoded for now until we store a dictionary of all the tree node icons

                icon = StockIconHelper.LoadIcon(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "ieframe.dll"),
                    -100,
                    true);
            }

            if (icon != null)
            {
                using (Icon ico = icon)
                {
                    var src = Imaging.CreateBitmapSourceFromHIcon(
                        icon.Handle,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());

                    src.Freeze();

                    entry.Dispatcher.BeginInvoke(() => entry.Source = src);
                }
            }
        }

        private void FindNodesRecursive(TreeNodeCollection nodes, string query, List<TreeNode> list)
        {
            var lower = query.ToLowerInvariant();

            foreach (var node in nodes)
            {
                if (node is TreeNode tn)
                {
                    if (tn.Text.ToLowerInvariant().Contains(lower) || tn.ToolTipText.ToLowerInvariant().Contains(lower))
                        list.Add(tn);

                    FindNodesRecursive(tn.Nodes, lower, list);
                }
            }
        }
    }
}
