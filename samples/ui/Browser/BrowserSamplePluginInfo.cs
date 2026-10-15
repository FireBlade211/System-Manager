using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser
{
    public class BrowserSamplePluginInfo : IPluginInfoContract
    {
        public string Name => "BROWSER Plugin Sample";

        public string Description => "Showcases menu and toolbar merging with a WinForms WebBrowser control.";

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
