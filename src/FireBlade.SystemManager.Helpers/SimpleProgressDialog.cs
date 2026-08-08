using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager.Helpers
{
    /// <summary>
    /// Represents a simple progress dialog.
    /// </summary>
    public class SimpleProgressDialog
    {
        private SimpleProgressDialog()
        { }

        private Form _form;

        private Label _msgLabel;
        private ProgressBar _bar;
        private Button _cancelButton;

        // field-backed properties are amazing

        /// <summary>
        /// Gets or sets the message displayed above the progress bar.
        /// </summary>
        public string Message
        {
            get;
            set
            {
                field = value;
                _msgLabel?.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the title displayed on the dialog.
        /// </summary>
        public string Title
        {
            get;
            set
            {
                field = value;
                _form?.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the style of the progress bar.
        /// </summary>
        public ProgressBarStyle Style
        {
            get;
            set
            {
                field = value;
                _bar?.Style = value;
            }
        }

        public bool AllowCancel
        {
            get;
            set
            {
                if (_bound) throw new InvalidOperationException("The AllowCancel property cannot be set while the SimpleProgressDialog is bound" +
                    " to a BackgroundWorker.");

                if (_form != null && field != value)
                {
                    if (value)
                    {
                        _cancelButton = new Button();
                        _cancelButton.Text = "Cancel";
                        _cancelButton.Click += CancelButton_Click;
                        _cancelButton.Location = new Point(185, 80);
                        _form.Controls.Add(_cancelButton);

                        _form.Size = new Size(300, 132);
                    }
                    else if (_cancelButton != null)
                    {
                        _form.Controls.Remove(_cancelButton);
                        _cancelButton = null;

                        _form.Size = new Size(300, 100);
                    }
                }

                field = value;
            }
        }

        public double Progress
        {
            get;
            set
            {
                if (_bound) throw new InvalidOperationException("The Progress property cannot be set while the SimpleProgressDialog is bound" +
                    " to a BackgroundWorker.");

                field = value;

                _bar?.Value = (int)Math.Floor(value * 100);
            }
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        public void ShowDialog(IWin32Window parent = null)
        {
            _form = new Form();
            _form.FormClosing += Form_FormClosing;
            
            _form.Text = this.Title;
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.ShowIcon = false;
            _form.ShowInTaskbar = false;
            _form.MaximizeBox = false;
            _form.MinimizeBox = false;
            _form.ControlBox = AllowCancel;
            _form.StartPosition = FormStartPosition.CenterParent;

            _form.Size = new Size(300, 100);

            _msgLabel = new Label();
            _msgLabel.Text = Message;
            _msgLabel.Location = new Point(8, 8);
            _msgLabel.AutoSize = true;
            _form.Controls.Add(_msgLabel);

            _bar = new ProgressBar();
            _bar.Minimum = 0;
            _bar.Maximum = 100;
            _bar.Value = (int)Math.Floor(Progress * 100);
            _bar.Width = 264;
            _bar.Style = Style;
            _bar.Height = 20;
            _bar.Location = new Point(8, 36);
            _form.Controls.Add(_bar);

            if (AllowCancel)
            {
                _cancelButton = new Button();
                _cancelButton.Text = "Cancel";
                _cancelButton.Click += CancelButton_Click;
                _cancelButton.Location = new Point(200, 64);
                _form.Controls.Add(_cancelButton);

                _form.Size = new Size(300, 132);
            }

            if (parent != null)
                _form.ShowDialog(parent);
            else
                _form.ShowDialog();
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_closeLock) return;

            if (_bound)
            {
                _worker.CancelAsync();
            }
            else
            {
                var args = new CancelEventArgs();
                Cancel?.Invoke(this, args);

                if (!args.Cancel)
                    Close();
            }
        }

        /// <summary>
        /// Fires when the user cancels the dialog.
        /// </summary>
        /// <remarks>This will not fire if the dialog is bound to a <see cref="BackgroundWorker"/>. For more info, see
        /// <see cref="BindToBackgroundWorker(BackgroundWorker)"/>.</remarks>
        public event CancelEventHandler Cancel;

        private bool _closeLock = false;

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (_bound)
            {
                _worker.CancelAsync();
            }
            else
            {
                var args = new CancelEventArgs();
                Cancel?.Invoke(this, args);

                if (!args.Cancel)
                    Close();
            }
        }

        public void Close()
        {
            _closeLock = true;
            _form.Close();
            _closeLock = false;
        }

        public object Invoke(Delegate method) => _form?.Invoke(method);
        public object Invoke(Delegate method, params object[] args) => _form?.Invoke(method, args);

        private bool _bound;
        private BackgroundWorker _worker;

        /// <summary>
        /// Binds this <see cref="SimpleProgressDialog"/> instance to a <see cref="BackgroundWorker"/>,
        /// allowing both to share progress status.
        /// </summary>
        /// <param name="worker">The <see cref="BackgroundWorker"/> to bind to.</param>
        /// <remarks>This function can be called only once per instance. The dialog will automatically update to
        /// report the worker's progress, however you must manually call <see cref="ShowDialog"/> once the worker starts.</remarks>
        public void BindToBackgroundWorker(BackgroundWorker worker)
        {
            if (_bound) return;

            worker.ProgressChanged += (s, e) => Invoke(() => this.Progress = e.ProgressPercentage / 100.0);
            worker.RunWorkerCompleted += (s, e) => Invoke(() => Close());

            AllowCancel = worker.WorkerSupportsCancellation;

            _worker = worker;
            _bound = true;
        }

        /// <summary>
        /// Creates a new <see cref="SimpleProgressDialog"/>.
        /// </summary>
        /// <param name="msg">The message displayed on the dialog.</param>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="indeterminate"><see langword="true"/> to use an indeterminate progress bar; <see langword="false"/> to use
        /// a smooth progress bar. The default is <see langword="false"/>.</param>
        /// <returns>The created <see cref="SimpleProgressDialog"/>.</returns>
        public static SimpleProgressDialog CreateDialog(string msg, string title, bool indeterminate = false)
        {
            var dlg = new SimpleProgressDialog();

            dlg.Title   = title;
            dlg.Message = msg;
            dlg.Style   = indeterminate ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
            dlg.AllowCancel = true;

            return dlg;
        }
    }
}
