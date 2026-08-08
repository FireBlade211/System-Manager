using System.Diagnostics;
using System.Windows.Forms;

namespace FireBlade.SystemManager
{
    public partial class AboutDlg : Form
    {
        public AboutDlg()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://learn.microsoft.com/visualstudio/designers/the-visual-studio-image-library?view=visualstudio");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/markjames/famfamfam-silk-icons");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://dotnet.microsoft.com/download/dotnet-framework/net452");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Yes, this is the .NET Framework 4 WinForms documentation. No I'm not changing it because this app uses .NET Framework
            // 4.5.
            Process.Start("https://learn.microsoft.com/previous-versions/visualstudio/visual-studio-2010/ms229601(v=vs.100)");
        }
    }
}
