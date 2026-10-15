using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireBlade.SystemManager.Plugin;

namespace DynamicTaskPane
{
    public class DynamicTaskPanePlugin : IPlugin
    {
        public IContract QueryContract(string contractIdentifier)
        {
            switch (contractIdentifier)
            {
                case ContractIdentifiers.PluginInfoContractIdentifier:
                    return new DynamicTaskPaneInfo();
                case ContractIdentifiers.PluginTreeContractIdentifier:
                    return new DynamicTaskPaneTreeContract();
            }

            return null;
        }
    }
}
