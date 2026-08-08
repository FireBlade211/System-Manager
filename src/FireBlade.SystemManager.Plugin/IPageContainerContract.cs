using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager.Plugin
{
    /// <summary>
    /// Represents a contract used for container nodes.
    /// </summary>
    public interface IPageContainerContract : IPageNodeContract
    {
        /// <summary>
        /// Retrieves the children of this container node.
        /// </summary>
        /// <returns>An array of children contained inside this container node.</returns>
        IPageNodeContract[] GetChildren();
    }
}
