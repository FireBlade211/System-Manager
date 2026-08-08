using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireBlade.SystemManager.DefaultPlugin
{
    public class DefaultPlugin : IPlugin
    {
        public IContract QueryContract(string contractIdentifier)
        {
            switch (contractIdentifier)
            {
                case ContractIdentifiers.PluginInfoContractIdentifier:
                    return new DefaultPluginInfo();
                case ContractIdentifiers.PluginTreeContractIdentifier:
                    return new DefaultPluginTree();
                default:
                    return null;
            }
        }
    }
}
