using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireBlade.SystemManager.Helpers.Controls;

namespace DynamicTaskPane
{
    public partial class SamplePage : UserControl
    {
        public SamplePage()
        {
            InitializeComponent();

            _deleteLink = new TaskPaneLink
            {
                Text = "Delete selected item",
                Tag = "del"
            };
        }

        private void AddItem() => listBox1.Items.Add($"Item {listBox1.Items.Count + 1}");

        private void RemoveSelected()
        {
            // Delete the item.
            listBox1.Items.Remove(listBox1.SelectedItem);

            // Rename all the items.
            for (int i = 0; i < listBox1.Items.Count; i++)
                listBox1.Items[i] = $"Item {i + 1}";
        }

        private void taskPane1_LinkClicked(object sender, TaskPaneLinkClickedEventArgs e)
        {
            string tag = e.Link.Tag?.ToString() ?? string.Empty;

            if (tag.Equals("add1", StringComparison.OrdinalIgnoreCase))
                AddItem();
            else if (tag.Equals("add5", StringComparison.OrdinalIgnoreCase))
                for (int i = 0; i < 5; i++)
                    AddItem();
            else if (tag.Equals("clear", StringComparison.OrdinalIgnoreCase))
                listBox1.Items.Clear();
            else if (tag.Equals("del", StringComparison.OrdinalIgnoreCase))
                RemoveSelected();
            else
                MessageBox.Show($"Link clicked: {e.Link.Text}", "Link Clicked", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private TaskPaneLink _deleteLink;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var grp = taskPane1.Groups.FirstOrDefault(x => x.Tag?.ToString().Equals("dyna", StringComparison.OrdinalIgnoreCase) == true);

            if (grp != null)
            {
                grp.Links.Clear();

                if (listBox1.SelectedItems.Count > 0)
                    grp.Links.Add(_deleteLink);
   
                if (listBox1.SelectedItems.Count == 1)
                {
                    int idx = listBox1.SelectedIndex;

                    for (int i = 0; i <= idx; i++)
                        grp.Links.Add(new TaskPaneLink
                        {
                            Text = $"Command {i + 1}"
                        });
                }
            }
        }
    }
}
