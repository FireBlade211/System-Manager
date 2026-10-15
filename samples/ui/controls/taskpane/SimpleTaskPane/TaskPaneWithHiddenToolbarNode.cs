using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleTaskPane
{
    public class TaskPaneWithHiddenToolbarNode : IPageContract
    {
        public string Id => "SimpleTaskPaneWithHiddenToolbarSample";

        public string DisplayName => "Simple Task Pane with Hidden Toolbar";

        public string Description => "Shows a simple task pane with a hidden toolbar.";

        internal TaskPanePage _page;

        public UserControl CreateContent() => _page = new TaskPanePage();

        public IContract QueryContract(string contractIdentifier)
        {
            switch (contractIdentifier)
            {
                case ContractIdentifiers.IconContractIdentifier:
                    return new DllIconContract();
                case ContractIdentifiers.ThemeContractIdentifier:
                    return new SimpleTaskPaneThemeContract(_page);
            }

            return null;
        }
    }
}
