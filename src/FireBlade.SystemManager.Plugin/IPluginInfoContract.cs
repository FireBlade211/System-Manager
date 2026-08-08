using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireBlade.SystemManager.Plugin
{
    /// <summary>
    /// Represents a contract for retrieving display information about a System Manager plugin.
    /// </summary>
    public interface IPluginInfoContract : IContract
    {
        /// <summary>
        /// Gets the display name of the plugin.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets the description of this plugin.
        /// </summary>
        string Description { get; }

        // note: supports IIconContract
    }
}
