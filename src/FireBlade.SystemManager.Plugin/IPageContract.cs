using System;
using System.Windows.Forms;

namespace FireBlade.SystemManager.Plugin
{
    /// <summary>
    /// Represents a contract used for page nodes.
    /// </summary>
    public interface IPageContract : IPageNodeContract
    {
        /// <summary>
        /// Creates the content of this page.
        /// </summary>
        /// <returns>The content of the page.</returns>
        UserControl CreateContent();
    }
}
