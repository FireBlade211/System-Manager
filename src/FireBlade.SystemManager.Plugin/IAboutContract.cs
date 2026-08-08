using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager.Plugin
{
    /// <summary>
    /// Represents a contract used to allow a plugin to show about information.
    /// </summary>
    public interface IAboutContract : IContract
    {
        /// <summary>
        /// Shows the about dialog for the plugin.
        /// </summary>
        /// <param name="owner">The owner of the dialog.</param>
        void ShowDialog(IWin32Window owner);
    }
}
