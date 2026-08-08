using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireBlade.SystemManager.Plugin
{
    /// <summary>
    /// Represents a contract used for retrieving the tree of a System Manager plugin.
    /// </summary>
    public interface IPluginTreeContract : IContract
    {
        /// <summary>
        /// Gets the nodes for this plugin tree.
        /// </summary>
        /// <param name="parentId">The ID of the parent node.</param>
        /// <returns>An array of page node contracts.</returns>
        IPageNodeContract[] GetNodes(out string parentId);
    }
}
