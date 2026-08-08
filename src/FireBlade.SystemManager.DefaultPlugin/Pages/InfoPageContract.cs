using FireBlade.SystemManager.Helpers;
using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager.DefaultPlugin.Pages
{
    public class InfoPageContract : IPageContract
    {
        public string Id => "SystemInfo";

        public string DisplayName => "System Information";

        public string Description => "View information about your system.";

        public UserControl CreateContent() => new InfoPage();

        public IContract QueryContract(string contractIdentifier)
        {
            switch (contractIdentifier)
            {
                case ContractIdentifiers.IconContractIdentifier:
                    return new InfoIconContract();
            }

            return null;
        }
    }

    public class InfoIconContract : IIconContract
    {
        public IContract QueryContract(string contractIdentifier) => null;

        public bool TryGetIcon(bool large, out Icon icon)
        {
            icon = StockIconHelper.GetIcon(StockIcon.DesktopPC, large ? StockIconOptions.Default : StockIconOptions.SmallIcon);
            return true;
        }
    }
}
