using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskPane
{
    public class SimpleTaskPanePlugin : IPlugin
    {
        public IContract QueryContract(string contractIdentifier)
        {
            switch (contractIdentifier)
            {
                case ContractIdentifiers.PluginInfoContractIdentifier:
                    return new SimpleTaskPaneInfo();
                case ContractIdentifiers.PluginTreeContractIdentifier:
                    return new SimpleTaskPaneTree();
            }

            return null;
        }
    }
}
