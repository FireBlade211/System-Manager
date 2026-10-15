using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskPane
{
    public class SimpleTaskPaneInfo : IPluginInfoContract
    {
        public string Name => "Simple Task Pane Sample Plugin";

        public string Description => "Shows how to use a simple TaskPane control on a plugin page.";

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
