using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskPane
{
    public class SamplesContainerNode : IPageContainerContract
    {
        public string Id => "PluginSamplesContainer";

        public string DisplayName => "Samples";

        public string Description => "System Manager Plugin Samples";

        public IPageNodeContract[] GetChildren() => new IPageNodeContract[]
        { new TaskPaneWithHiddenToolbarNode(), new SimpleTaskPaneWithShownToolbarNode() };

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
