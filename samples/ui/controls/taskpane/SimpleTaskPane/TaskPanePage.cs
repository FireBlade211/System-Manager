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

namespace SimpleTaskPane
{
    public partial class TaskPanePage : UserControl
    {
        public TaskPanePage()
        {
            InitializeComponent();
        }

        private void taskPane1_LinkClicked(object sender, TaskPaneLinkClickedEventArgs e)
        {
            label3.Text = "LinkClicked";
            label4.Text = e.Link.Text;

            label6.Text = "Link:";
        }

        private void taskPane1_GroupExpandedChanged(object sender, TaskPaneGroupExpandedChangedEventArgs e)
        {
            label3.Text = "GroupExpandedChanged";
            label4.Text = e.Group.Title;
            label6.Text = "Group:";
        }
    }
}
