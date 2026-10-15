using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser
{
    public class BrowserSamplePlugin : IPlugin
    {
        public IContract QueryContract(string contractIdentifier)
        {
            switch (contractIdentifier)
            {
                case ContractIdentifiers.PluginInfoContractIdentifier:
                    return new BrowserSamplePluginInfo();
                case ContractIdentifiers.PluginTreeContractIdentifier:
                    return new BrowserSamplePluginTree();
            }

            return null;
        }
    }
}
