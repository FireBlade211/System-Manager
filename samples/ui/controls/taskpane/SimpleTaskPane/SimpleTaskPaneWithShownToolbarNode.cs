using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleTaskPane
{
    public class SimpleTaskPaneWithShownToolbarNode : IPageContract
    {
        public string Id => "SimpleTaskPaneWithShownToolbarSample";

        public string DisplayName => "Simple Task Pane with Visible Toolbar";

        public string Description => "Shows a simple task pane with a visible toolbar.";

        internal TaskPanePage _page;

        public UserControl CreateContent()
        {
            var p = _page = new TaskPanePage();
            p.taskPane1.HideToolbarWhenActive = false;

            return p;
        }

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
