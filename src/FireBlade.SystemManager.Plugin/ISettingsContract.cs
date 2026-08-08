using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager.Plugin
{
    /// <summary>
    /// Represents a contract used to allow configuration of plugin settings.
    /// </summary>
    public interface ISettingsContract : IContract
    {
        /// <summary>
        /// Shows the settings dialog for the plugin.
        /// </summary>
        /// <param name="owner">The owner of the dialog.</param>
        void ShowDialog(IWin32Window owner);
    }
}
