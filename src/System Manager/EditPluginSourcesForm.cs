using FireBlade.SystemManager.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager
{
    public partial class EditPluginSourcesForm : Form
    {
        private List<PluginSourceItem> _sources;

        public EditPluginSourcesForm(List<PluginSourceItem> sources)
        {
            InitializeComponent();
            listView1.BeginUpdate();

            foreach (var src in sources)
                listView1.Items.Add(new ListViewItem
                {
                    Text = src.Name,
                    ToolTipText = src.Repository,
                    Tag = src
                });

            if (sources.Any())
                listView1.SelectedIndices.Add(0);
            
            listView1.EndUpdate();
            _sources = sources;
        }

        private bool _labelEditing = false;

        private const uint LVM_FIRST = 0x1000;
        private const uint LVM_CANCELEDITLABEL = (LVM_FIRST + 179);

        private void listView1_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            _labelEditing = true;
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            _labelEditing = false;

            var item = listView1.Items[e.Item];
            if (item?.Tag is PluginSourceItem src)
                src.Name = e.Label;
        }

        private void listView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                var item = listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault();
                if (item != null)
                {
                    if (!_labelEditing)
                        item.BeginEdit();
                    else
                        NativeHelper.SendMessageW(listView1.Handle, LVM_CANCELEDITLABEL, UIntPtr.Zero, IntPtr.Zero);
                }
            }
            else if (!_labelEditing && e.KeyCode is Keys.Back or Keys.Delete)
            {
                DeleteSelected();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.BeginEdit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeleteSelected();
        }

        private void DeleteSelected()
        {
            var item = listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault();
            if (item != null)
            {
                var result = !checkBox1.Checked ? DialogResult.Yes
                    : MessageBox.Show($"Are you sure you want to delete the source {item.Text} ({item.ToolTipText})?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    listView1.Items.Remove(item);

                    if (item.Tag is PluginSourceItem src)
                        _sources.Remove(src);
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = listView1.SelectedItems.Count > 0;
            button2.Enabled = listView1.SelectedItems.Count > 0;
        }
    }
}
