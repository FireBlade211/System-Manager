using FireBlade.SystemManager.Helpers;
using FireBlade.SystemManager.Plugin;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FireBlade.SystemManager.DefaultPlugin.Pages
{
    public class ServicesPageContract : IPageContract
    {
        public string Id => "FireBlade.SystemManager.DefaultPlugin.Services";

        public string DisplayName => "Services";

        public string Description => "View, start, stop and manage Windows services.";

        public UserControl CreateContent() => new ServicesPage(); /* new UserControl
        {
            Controls =
            {
                new Label
                {
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = "Hello, World! This is the Services page that was received through a plugin!"
                }
            }
        }; */

        public IContract QueryContract(string contractIdentifier)
        {
            switch (contractIdentifier)
            {
                case ContractIdentifiers.IconContractIdentifier:
                    return new ServicesPageIconContract();
            }
            
            return null;
        }
    }

    public class ServicesPageIconContract : IIconContract
    {
        public IContract QueryContract(string contractIdentifier) => null;

        public bool TryGetIcon(bool large, out Icon icon)
        {
            var ico = StockIconHelper.LoadIcon(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "filemgmt.dll"),
                0, large);

            icon = ico;

            return ico != null;
        }
    }
}
