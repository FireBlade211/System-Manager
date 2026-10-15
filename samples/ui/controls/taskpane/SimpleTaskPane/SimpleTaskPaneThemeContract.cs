using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskPane
{
    public class SimpleTaskPaneThemeContract : IThemeContract
    {
        private TaskPanePage _page;

        public SimpleTaskPaneThemeContract(TaskPanePage page)
        {
            _page = page;
        }

        public void OnColorSchemeChanged(ColorScheme scheme)
        {
            _page.taskPane1.ColorScheme = scheme;
        }

        public IContract QueryContract(string contractIdentifier) => null;
    }
}
