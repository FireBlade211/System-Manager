using FireBlade.SystemManager.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Progress
{
    public partial class ProgressPage : UserControl
    {
        public ProgressPage()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 1;
            comboBox2.SelectedIndex = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create the dialog.
            var dlg = SimpleProgressDialog.CreateDialog(textBox2.Text, textBox1.Text, false);

            dlg.Style = (ProgressBarStyle)comboBox2.SelectedIndex;
            dlg.AllowCancel = checkBox1.Checked;

            // Bind to a BackgroundWorker.
            if (checkBox2.Checked)
            {
                backgroundWorker1.WorkerSupportsCancellation = checkBox1.Checked;
                dlg.BindToBackgroundWorker(backgroundWorker1);

                backgroundWorker1.RunWorkerAsync();
            }

            bool cancel = false;
            dlg.Cancel += (s, ee) => cancel = true;

            if (!checkBox2.Checked)
                Task.Run(() =>
                {
                    int interval = (int)Math.Min(int.MaxValue, Math.Max(int.MinValue, InvokeGet(() => numericUpDown1.Value)));

                    int sel = InvokeGet(() => comboBox1.SelectedIndex);

                    if (sel == 1)
                        interval *= 1000; // seconds -> ms
                    else if (sel == 2)
                        interval *= 1000 * 60; // minutes -> ms ( * 60000)

                    if (InvokeGet(() => radioButton2.Checked)) // Duration
                        interval /= 100;

                    for (int i = 0; i < 100; i++)
                    {
                        if (cancel)
                            break;

                        double progress = (i + 1) / 100.0;

                        if (dlg.Style != ProgressBarStyle.Marquee)
                            dlg.Invoke((Action)(() => dlg.Progress = progress));

                        if (i >= 99)
                        {
                            dlg.Invoke((Action)(() => dlg.Close()));
                            break;
                        }

                        Thread.Sleep(interval);
                    }
                });

            dlg.ShowDialog(this);
        }

        private T InvokeGet<T>(Control c, Func<T> d) => (T)(c.Invoke(d));
        private T InvokeGet<T>(Func<T> d) => InvokeGet<T>(this, d);

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int interval = (int)Math.Min(int.MaxValue, Math.Max(int.MinValue, InvokeGet(() => numericUpDown1.Value)));
            int progress = 0;

            int sel = InvokeGet(() => comboBox1.SelectedIndex);

            if (sel == 1)
                interval *= 1000; // seconds -> ms
            else if (sel == 2)
                interval *= 1000 * 60; // minutes -> ms ( * 60000)

            if (InvokeGet(() => radioButton2.Checked)) // Duration
                interval /= 100;

            for (int i = 0; i < 100; i++)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                progress++;

                backgroundWorker1.ReportProgress(progress);
                Thread.Sleep(interval);
            }
        }
    }
}
