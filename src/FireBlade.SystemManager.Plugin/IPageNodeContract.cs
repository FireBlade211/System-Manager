using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireBlade.SystemManager.Plugin
{
    /// <summary>
    /// Represents the base contract for tree nodes.
    /// </summary>
    public interface IPageNodeContract : IContract
    {
        /// <summary>
        /// Gets the unique ID of this page node.
        /// </summary>
        /// <remarks>Container nodes that share the same ID will be merged. This ID can be used to identify a node.</remarks>
        string Id { get; }
        /// <summary>
        /// Gets the display name of this page node.
        /// </summary>
        string DisplayName { get; }
        /// <summary>
        /// Gets the description of this page node.
        /// </summary>
        string Description { get; }
    }
}
