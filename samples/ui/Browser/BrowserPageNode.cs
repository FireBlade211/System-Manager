using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Browser
{
    public class BrowserPageNode : IPageContract
    {
        public string Id => "BrowserSample";

        public string DisplayName => "Browser Sample";

        public string Description => "Shows menu and toolbar merging with a WinForms WebBrowser control.";

        public UserControl CreateContent() => new BrowserPage();

        public IContract QueryContract(string contractIdentifier)
        {
            switch (contractIdentifier)
            {
                case ContractIdentifiers.IconContractIdentifier:
                    return new DllIconContract();
            }

            return null;
        }
    }
}
