using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Browser
{
    public partial class BrowserPage : UserControl
    {
        public BrowserPage()
        {
            InitializeComponent();

            webBrowser1.CanGoBackChanged += WebBrowser_CanGoBackChanged;
            webBrowser1.CanGoForwardChanged += WebBrowser_CanGoForwardChanged;
        }

        private void WebBrowser_CanGoForwardChanged(object sender, EventArgs e)
        {
            toolStripButton2.Enabled = webBrowser1.CanGoForward;
            forwardToolStripMenuItem.Enabled = webBrowser1.CanGoForward;
        }

        private void WebBrowser_CanGoBackChanged(object sender, EventArgs e)
        {
            toolStripButton1.Enabled = webBrowser1.CanGoBack;
            backToolStripMenuItem.Enabled = webBrowser1.CanGoBack;
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            toolStripTextBox1.Text = webBrowser1.Url.ToString();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh(WebBrowserRefreshOption.Normal);
        }

        private void ifExpiredToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh(WebBrowserRefreshOption.IfExpired);
        }

        private void continueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh(WebBrowserRefreshOption.Completely);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(toolStripTextBox1.Text);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            webBrowser1.GoHome();
        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            webBrowser1.GoSearch();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoSearch();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintPreviewDialog();
        }

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPageSetupDialog();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowSaveAsDialog();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPropertiesDialog();
        }
    }
}
