using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireBlade.SystemManager.DefaultPlugin
{
    public class DefaultPluginInfo : IPluginInfoContract
    {
        public string Name => "Built-in";

        public string Description => "This is the built-in plugin that comes with System Manager.";

        public IContract QueryContract(string contractIdentifier)
        {
            switch (contractIdentifier)
            {
                case ContractIdentifiers.IconContractIdentifier:
                    return new DefaultPluginIconContract();
            }

            return null;
        }
    }
}
