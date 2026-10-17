using FireBlade.SystemManager.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager.DefaultPlugin.Dialogs
{
    public partial class ServicePropertiesDialog : Form
    {
        public ServicePropertiesDialog()
        {
            InitializeComponent();
        }

        public ServicePropertiesDialog(ServiceController sc)
        {
            InitializeComponent();
            Shown += (s, e) => InitFromService(sc);
        }

        private void InitFromService(ServiceController sc)
        {
            Text = $"{sc.DisplayName} Properties";
            label1.Text = sc.DisplayName;
            label2.Text = $"Internal name: {sc.ServiceName}";
            textBox1.Text = sc.Description;
            label3.Text = $"Image Path: {sc.ImagePath}";

            var icon = StockIconHelper.LoadIcon(sc.ImagePath, 0, true);
            if (icon != null)
                pictureBox1.Image = icon.ToBitmap();
            else
                pictureBox1.Image = StockIconHelper.LoadIcon(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System),
                    "filemgmt.dll"), 0, true).ToBitmap();

            try
            {
                listView1.BeginUpdate();

                listView1.Items.Clear();
                listView1.Items.AddRange(sc.ServicesDependedOn.Select(ServiceToListItem).ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load dependencies: {ex.Message}", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                listView1.EndUpdate();
            }

            try
            {
                listView2.BeginUpdate();

                listView2.Items.Clear();
                listView2.Items.AddRange(sc.DependentServices.Select(ServiceToListItem).ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load dependents: {ex.Message}", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                listView2.EndUpdate();
            }
        }

        private ListViewItem ServiceToListItem(ServiceController x) => new ListViewItem
        {
            Text = x.DisplayName,
            Tag = x,
            SubItems =
            {
                new ListViewItem.ListViewSubItem
                {
                    Text = x.ServiceName
                },
                new ListViewItem.ListViewSubItem
                {
                    Text = x.Status switch
                    {
                        ServiceControllerStatus.ContinuePending => "Continue in Progress",
                        ServiceControllerStatus.PausePending => "Pausing in Progress",
                        ServiceControllerStatus.StopPending => "Stopping in Progress",
                        ServiceControllerStatus.StartPending => "Starting in Progress",
                        _ => x.Status.ToString()
                    }
                },
                new ListViewItem.ListViewSubItem
                {
                    Text = (int)x.StartType != -1 ? x.StartType.ToString() : "Unknown"
                }
            }
        };

        private void button2_Click(object sender, EventArgs e)
        {
            var item = listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault();
            if (item != null && item.Tag is ServiceController sc)
            {
                InitFromService(sc);
                tabControl1.SelectedIndex = 0;
            }
        }

        public event EventHandler<string> OnLocateService;

        private void button3_Click(object sender, EventArgs e)
        {
            var item = listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault();
            if (item != null && item.Tag is ServiceController sc)
            {
                Close();
                OnLocateService?.Invoke(this, sc.ServiceName);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var item = listView2.SelectedItems.Cast<ListViewItem>().FirstOrDefault();
            if (item != null && item.Tag is ServiceController sc)
            {
                InitFromService(sc);
                tabControl1.SelectedIndex = 0;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var item = listView2.SelectedItems.Cast<ListViewItem>().FirstOrDefault();
            if (item != null && item.Tag is ServiceController sc)
            {
                Close();
                OnLocateService?.Invoke(this, sc.ServiceName);
            }
        }
    }
}
