using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicTaskPane
{
    public class DynamicTaskPaneInfo : IPluginInfoContract
    {
        public string Name => "Dynamic Task Pane Sample Plugin";

        public string Description => "Shows how to dynamically add commands to a TaskPane.";

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
