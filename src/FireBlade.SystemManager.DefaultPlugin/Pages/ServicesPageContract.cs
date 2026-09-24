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

        public ServicesPage _page;

        public UserControl CreateContent() => _page = new ServicesPage();

        public IContract QueryContract(string contractIdentifier)
        {
            switch (contractIdentifier)
            {
                case ContractIdentifiers.IconContractIdentifier:
                    return new ServicesPageIconContract();
                case ContractIdentifiers.ThemeContractIdentifier:
                    return new ServicesPageThemeContract(this);
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

    public class ServicesPageThemeContract : IThemeContract
    {
        public ServicesPageContract _contract;

        public ServicesPageThemeContract(ServicesPageContract c)
        {
            _contract = c;
        }

        public void OnColorSchemeChanged(ColorScheme scheme)
        {
            _contract._page._table.Scheme = scheme;
        }

        public IContract QueryContract(string contractIdentifier) => null;
    }
}
