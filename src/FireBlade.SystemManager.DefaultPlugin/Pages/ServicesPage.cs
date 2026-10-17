using FireBlade.SystemManager.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireBlade.SystemManager.Helpers.Controls;
using FireBlade.SystemManager.DefaultPlugin.Dialogs;

namespace FireBlade.SystemManager.DefaultPlugin.Pages
{
    public partial class ServicesPage : UserControl
    {
        public BlueColorTable _table;

        public ServicesPage()
        {
            InitializeComponent();
            Load += ServicesPage_Load;

            contextMenuStrip1.Renderer = new ToolStripProfessionalRenderer(_table = new BlueColorTable());
        }

        private const uint ECM_FIRST = 0x1500;
        private const uint EM_SETCUEBANNER = (ECM_FIRST + 1);

        private HashSet<ListViewItem> _items = [];

        private void ServicesPage_Load(object sender, EventArgs e)
        {
            IntPtr ptr = Marshal.StringToHGlobalUni("Search services...");

            NativeHelper.SendMessageW(searchBox.Handle, EM_SETCUEBANNER, (UIntPtr)0u, ptr);

            Marshal.FreeHGlobal(ptr);

            listView1.BeginUpdate();

            var services = ServiceController.GetServices();
            foreach (var sc in services)
            {
                var start = sc.StartType;

                listView1.Items.Add(new ListViewItem
                {
                    Text = sc.DisplayName,
                    Tag = sc,
                    SubItems =
                    {
                        new ListViewItem.ListViewSubItem
                        {
                            Text = sc.ServiceName
                        },
                        new ListViewItem.ListViewSubItem
                        {
                            Text = sc.Status switch
                            {
                                ServiceControllerStatus.ContinuePending => "Continue in Progress",
                                ServiceControllerStatus.PausePending => "Pausing in Progress",
                                ServiceControllerStatus.StopPending => "Stopping in Progress",
                                ServiceControllerStatus.StartPending => "Starting in Progress",
                                _ => sc.Status.ToString()
                            }
                        },
                        new ListViewItem.ListViewSubItem
                        {
                            Text = (int)start != -1 ? start.ToString() : "Unknown"
                        }
                    }
                });
            }

            listView1.EndUpdate();

            propertiesToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.Return;
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            ListViewItem foundItem =
                listView1.FindItemWithText(searchBox.Text, true, 0, true);

            if (foundItem != null)
                listView1.TopItem = foundItem;
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                ShowLvContextMenu(true);
        }

        private void ShowLvContextMenu(bool mouse)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                var item = listView1.SelectedItems.Cast<ListViewItem>().First();

                contextMenuStrip1.Show(listView1, mouse ? listView1.PointToClient(Cursor.Position) : item.Position);
            }
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Apps)
                ShowLvContextMenu(false);
        }

        private const uint SC_MANAGER_CONNECT = 0x0001;
        private const uint SERVICE_CONTROL_PAUSE = 0x00000002;
        private const uint SERVICE_CONTROL_CONTINUE = 0x00000003;
        private const uint SERVICE_STOP = 0x0020;
        private const uint SERVICE_START = 0x0010;
        private const uint SERVICE_QUERY_STATUS = 0x0004;

        private const uint SERVICE_CONTROL_STOP = 0x00000001;
        private const uint SERVICE_PAUSE_CONTINUE = 0x0040;

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr OpenSCManager(
            string lpMachineName,
            string lpDatabaseName,
            uint dwDesiredAccess);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr OpenService(
            IntPtr hSCManager,
            string lpServiceName,
            uint dwDesiredAccess);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool ControlService(
            IntPtr hService,
            uint dwControl,
            out SERVICE_STATUS lpServiceStatus);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CloseServiceHandle(IntPtr hSCObject);

        [StructLayout(LayoutKind.Sequential)]
        private struct SERVICE_STATUS
        {
            public uint ServiceType;
            public uint CurrentState;
            public uint ControlsAccepted;
            public uint Win32ExitCode;
            public uint ServiceSpecificExitCode;
            public uint CheckPoint;
            public uint WaitHint;
        }

        [DllImport("Advapi32.dll", SetLastError = true)]
        private static extern bool StartServiceW(nint hService, uint dwNumServiceArgs, IntPtr lpServiceArgVectors);

        private void RefreshItem(ListViewItem lvi, ServiceController sc)
        {
            lvi.Text = sc.DisplayName;

            lvi.SubItems[1] = new ListViewItem.ListViewSubItem(lvi, sc.ServiceName);
            lvi.SubItems[2] = new ListViewItem.ListViewSubItem(lvi, sc.Status switch
            {
                ServiceControllerStatus.ContinuePending => "Continue in Progress",
                ServiceControllerStatus.PausePending => "Pausing in Progress",
                ServiceControllerStatus.StopPending => "Stopping in Progress",
                ServiceControllerStatus.StartPending => "Starting in Progress",
                _ => sc.Status.ToString()
            });

            var start = sc.StartType;
            lvi.SubItems[3] = new ListViewItem.ListViewSubItem(lvi, (int)start != -1 ? start.ToString() : "Unknown");
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sc = listView1.SelectedItems.Cast<ListViewItem>().First().Tag as ServiceController;

            var dlg = SimpleProgressDialog.CreateDialog("System Manager is starting the service...", "Starting service", true);
            dlg.AllowCancel = false;

            Task.Run(() =>
            {
                var scm = OpenSCManager(
                    null,
                    null,
                    SC_MANAGER_CONNECT);

                if (scm == IntPtr.Zero)
                    throw new Win32Exception();

                try
                {
                    var service = OpenService(
                        scm,
                        sc.ServiceName,
                        SERVICE_START | SERVICE_QUERY_STATUS);

                    if (service == IntPtr.Zero)
                        throw new Win32Exception();

                    try
                    {
                        if (!StartServiceW(service, 0u, IntPtr.Zero))
                        {
                            throw new Win32Exception();
                        }
                    }
                    finally
                    {
                        CloseServiceHandle(service);
                    }
                }
                catch (Exception ex)
                {
                    Invoke(() => MessageBox.Show($"An error occured: {ex.Message}", null, MessageBoxButtons.OK, MessageBoxIcon.Error));
                }
                finally
                {
                    CloseServiceHandle(scm);
                    dlg.Invoke(dlg.Close);

                    sc.Refresh();
                    RefreshItem(listView1.SelectedItems.Cast<ListViewItem>().First(), sc);
                    UpdateMenuItems();
                }
            });

            dlg.ShowDialog(this);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Cast<ListViewItem>().Any())
                UpdateMenuItems();
            else
            {
                startToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = false;
                pauseToolStripMenuItem.Enabled = false;
                resumeToolStripMenuItem.Enabled = false;
                restartToolStripMenuItem.Enabled = false;

                toolStripMenuItem8.Enabled = false;
                toolStripMenuItem9.Enabled = false;
                toolStripMenuItem10.Enabled = false;
                toolStripMenuItem11.Enabled = false;
                toolStripMenuItem12.Enabled = false;

                toolStripMenuItem2.Enabled = false;
                toolStripMenuItem3.Enabled = false;
                toolStripMenuItem4.Enabled = false;
                toolStripMenuItem5.Enabled = false;
                toolStripMenuItem6.Enabled = false;
            }

            propertiesToolStripMenuItem.Enabled = listView1.SelectedItems.Count > 0;
            toolStripButton1.Enabled = listView1.SelectedItems.Count > 0;
            toolStripMenuItem7.Enabled = listView1.SelectedItems.Count > 0;
        }

        private void UpdateMenuItems()
        {
            ServiceControllerStatus state = listView1.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.Tag
                is ServiceController sc ? sc.Status : (ServiceControllerStatus)(-1);

            startToolStripMenuItem.Enabled = state != ServiceControllerStatus.Running;
            stopToolStripMenuItem.Enabled = state != ServiceControllerStatus.Stopped;
            pauseToolStripMenuItem.Enabled = state != ServiceControllerStatus.Paused;
            resumeToolStripMenuItem.Enabled = state == ServiceControllerStatus.Paused;
            restartToolStripMenuItem.Enabled = state == ServiceControllerStatus.Running;

            toolStripMenuItem8.Enabled = state != ServiceControllerStatus.Running;
            toolStripMenuItem9.Enabled = state != ServiceControllerStatus.Stopped;
            toolStripMenuItem10.Enabled = state != ServiceControllerStatus.Paused;
            toolStripMenuItem11.Enabled = state == ServiceControllerStatus.Paused;
            toolStripMenuItem12.Enabled = state == ServiceControllerStatus.Running;

            toolStripMenuItem2.Enabled = state != ServiceControllerStatus.Running;
            toolStripMenuItem3.Enabled = state != ServiceControllerStatus.Stopped;
            toolStripMenuItem4.Enabled = state != ServiceControllerStatus.Paused;
            toolStripMenuItem5.Enabled = state == ServiceControllerStatus.Paused;
            toolStripMenuItem6.Enabled = state == ServiceControllerStatus.Running;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sc = listView1.SelectedItems.Cast<ListViewItem>().First().Tag as ServiceController;

            if (!sc.CanStop)
            {
                MessageBox.Show("This service cannot be stopped.", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dlg = SimpleProgressDialog.CreateDialog("System Manager is stopping the service...", "Stopping service", true);
            dlg.AllowCancel = false;

            Task.Run(() =>
            {
                var scm = OpenSCManager(
                    null,
                    null,
                    SC_MANAGER_CONNECT);

                if (scm == IntPtr.Zero)
                    throw new Win32Exception();

                try
                {
                    var service = OpenService(
                        scm,
                        sc.ServiceName,
                        SERVICE_STOP | SERVICE_QUERY_STATUS);

                    if (service == IntPtr.Zero)
                        throw new Win32Exception();

                    try
                    {
                        if (!ControlService(
                            service,
                            SERVICE_CONTROL_STOP,
                            out _))
                        {
                            throw new Win32Exception();
                        }
                    }
                    finally
                    {
                        CloseServiceHandle(service);

                        sc.Refresh();
                        RefreshItem(listView1.SelectedItems.Cast<ListViewItem>().First(), sc);
                    }
                }
                catch (Exception ex)
                {
                    Invoke(() => MessageBox.Show($"An error occured: {ex.Message}", null, MessageBoxButtons.OK, MessageBoxIcon.Error));
                }
                finally
                {
                    CloseServiceHandle(scm);
                    dlg.Invoke(dlg.Close);

                    UpdateMenuItems();
                }
            });

            dlg.ShowDialog(this);
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sc = listView1.SelectedItems.Cast<ListViewItem>().First().Tag as ServiceController;

            if (!sc.CanPauseAndContinue)
            {
                MessageBox.Show("This service cannot be paused.", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dlg = SimpleProgressDialog.CreateDialog("System Manager is pausing the service...", "Pausing service", true);
            dlg.AllowCancel = false;

            Task.Run(() =>
            {
                var scm = OpenSCManager(
                    null,
                    null,
                    SC_MANAGER_CONNECT);

                if (scm == IntPtr.Zero)
                    throw new Win32Exception();

                try
                {
                    var service = OpenService(
                        scm,
                        sc.ServiceName,
                        SERVICE_PAUSE_CONTINUE | SERVICE_QUERY_STATUS);

                    if (service == IntPtr.Zero)
                        throw new Win32Exception();

                    try
                    {
                        if (!ControlService(service, SERVICE_CONTROL_PAUSE, out _))
                        {
                            throw new Win32Exception();
                        }
                    }
                    finally
                    {
                        CloseServiceHandle(service);
                    }
                }
                catch (Exception ex)
                {
                    Invoke(() => MessageBox.Show($"An error occured: {ex.Message}", null, MessageBoxButtons.OK, MessageBoxIcon.Error));
                }
                finally
                {
                    CloseServiceHandle(scm);
                    dlg.Invoke(dlg.Close);

                    sc.Refresh();
                    RefreshItem(listView1.SelectedItems.Cast<ListViewItem>().First(), sc);
                    UpdateMenuItems();
                }
            });

            dlg.ShowDialog(this);
        }

        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sc = listView1.SelectedItems.Cast<ListViewItem>().First().Tag as ServiceController;

            if (!sc.CanPauseAndContinue)
            {
                MessageBox.Show("This service cannot be resumed.", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dlg = SimpleProgressDialog.CreateDialog("System Manager is resuming the service...", "Resume service", true);
            dlg.AllowCancel = false;

            Task.Run(() =>
            {
                var scm = OpenSCManager(
                    null,
                    null,
                    SC_MANAGER_CONNECT);

                if (scm == IntPtr.Zero)
                    throw new Win32Exception();

                try
                {
                    var service = OpenService(
                        scm,
                        sc.ServiceName,
                        SERVICE_PAUSE_CONTINUE | SERVICE_QUERY_STATUS);

                    if (service == IntPtr.Zero)
                        throw new Win32Exception();

                    try
                    {
                        if (!ControlService(service, SERVICE_CONTROL_CONTINUE, out _))
                        {
                            throw new Win32Exception();
                        }
                    }
                    finally
                    {
                        CloseServiceHandle(service);
                    }
                }
                catch (Exception ex)
                {
                    Invoke(() => MessageBox.Show($"An error occured: {ex.Message}", null, MessageBoxButtons.OK, MessageBoxIcon.Error));
                }
                finally
                {
                    CloseServiceHandle(scm);
                    dlg.Invoke(dlg.Close);

                    sc.Refresh();
                    RefreshItem(listView1.SelectedItems.Cast<ListViewItem>().First(), sc);
                    UpdateMenuItems();
                }
            });

            dlg.ShowDialog(this);
        }

        [DllImport("Advapi32.dll")]
        private static extern bool QueryServiceStatusEx(
          nint hService,
          uint InfoLevel,
          ref SERVICE_STATUS_PROCESS lpBuffer,
          uint cbBufSize,
          ref uint pcbBytesNeeded);

        private struct SERVICE_STATUS_PROCESS
        {
            public uint dwServiceType;
            public uint dwCurrentState;
            public uint dwControlsAccepted;
            public uint dwWin32ExitCode;
            public uint dwServiceSpecificExitCode;
            public uint dwCheckPoint;
            public uint dwWaitHint;
            public uint dwProcessId;
            public uint dwServiceFlags;
        }

        private const int SERVICE_STOPPED = 0x00000001;
        private const int SERVICE_RUNNING = 0x00000004;

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sc = listView1.SelectedItems.Cast<ListViewItem>().First().Tag as ServiceController;

            if (!sc.CanStop)
            {
                MessageBox.Show("This service cannot be stopped or restarted.", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dlg = SimpleProgressDialog.CreateDialog("System Manager is stopping the service...", "Restart service", false);
            dlg.AllowCancel = false;

            Task.Run(() =>
            {
                var scm = OpenSCManager(
                    null,
                    null,
                    SC_MANAGER_CONNECT);

                if (scm == IntPtr.Zero)
                    throw new Win32Exception();

                try
                {
                    var service = OpenService(
                        scm,
                        sc.ServiceName,
                        SERVICE_STOP | SERVICE_START | SERVICE_QUERY_STATUS);

                    if (service == IntPtr.Zero)
                        throw new Win32Exception();

                    try
                    {
                        if (!ControlService(
                            service,
                            SERVICE_CONTROL_STOP,
                            out _))
                        {
                            throw new Win32Exception();
                        }

                        SERVICE_STATUS_PROCESS status = new SERVICE_STATUS_PROCESS();
                        uint bytes = 0;

                        while (status.dwCurrentState != SERVICE_STOPPED)
                        {
                            Thread.Sleep(500);

                            if (!QueryServiceStatusEx(service, 0, ref status, (uint)Marshal.SizeOf<SERVICE_STATUS_PROCESS>(), ref bytes))
                                throw new Win32Exception();
                        }

                        dlg.Invoke(() =>
                        {
                            dlg.Progress = 0.5;
                            dlg.Message = "System Manager is restarting the service...";
                        });

                        if (!StartServiceW(service, 0, IntPtr.Zero))
                        {
                            throw new Win32Exception();
                        }

                        while (status.dwCurrentState != SERVICE_RUNNING)
                        {
                            Thread.Sleep(500);

                            if (!QueryServiceStatusEx(service, 0, ref status, (uint)Marshal.SizeOf<SERVICE_STATUS_PROCESS>(), ref bytes))
                                throw new Win32Exception();
                        }

                        dlg.Invoke(() => dlg.Progress = 1);
                    }
                    finally
                    {
                        CloseServiceHandle(service);

                        sc.Refresh();
                        RefreshItem(listView1.SelectedItems.Cast<ListViewItem>().First(), sc);
                        UpdateMenuItems();
                    }
                }
                catch (Exception ex)
                {
                    Invoke(() => MessageBox.Show($"An error occured: {ex.Message}", null, MessageBoxButtons.OK, MessageBoxIcon.Error));
                }
                finally
                {
                    CloseServiceHandle(scm);
                    dlg.Invoke(dlg.Close);
                }
            });

            dlg.ShowDialog(this);
        }

        private void taskPane1_LinkClicked(object sender, TaskPaneLinkClickedEventArgs e)
        {
            MessageBox.Show($"Link clicked: {e.Link.Text}");
        }

        private void taskPane1_GroupExpandedChanged(object sender, TaskPaneGroupExpandedChangedEventArgs e)
        {
            MessageBox.Show($"Group: {e.Group.Title} expanded changed to: {e.Group.Expanded}");
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sc = listView1.SelectedItems.Cast<ListViewItem>().First().Tag as ServiceController;
            if (sc != null)
            {
                var dlg = new ServicePropertiesDialog(sc);
                dlg.OnLocateService += (s, e) =>
                {
                    var it = listView1.Items.Cast<ListViewItem>().FirstOrDefault(x =>
                    {
                        if (x.Tag is ServiceController sc)
                            return sc.ServiceName.Equals(e, StringComparison.Ordinal);

                        return false;
                    });

                    if (it != null)
                    {
                        listView1.SelectedItems.Clear();

                        it.Selected = true;
                        it.EnsureVisible();
                    }
                };

                dlg.ShowDialog();
            }
        }
    }
}
