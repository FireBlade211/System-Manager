using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamicTaskPane
{
    public class DynamicTaskPanePageContract : IPageContract
    {
        public string Id => "DynamicTaskPaneSample";

        public string DisplayName => "Dynamic Task Pane";

        public string Description => "Shows how to dynamically add commands to a TaskPane.";

        private SamplePage _page;

        public UserControl CreateContent() => _page = new SamplePage();

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
