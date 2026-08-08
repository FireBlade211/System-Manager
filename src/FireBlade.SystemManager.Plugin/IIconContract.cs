using System.Drawing;

namespace FireBlade.SystemManager.Plugin
{
    /// <summary>
    /// Represents a contract used for retrieving icons.
    /// </summary>
    public interface IIconContract : IContract
    {
        /// <summary>
        /// Attempts to retrieve an icon for the object this contract was queried from at the specified size.
        /// </summary>
        /// <param name="large"><see langword="true"/> to retrieve the large icon; <see langword="false"/> to
        /// retrieve the small icon.</param>
        /// <param name="icon">When this function returns, receives the icon retrieved.</param>
        /// <returns><see langword="true"/> if the icon was retrieved successfully; otherwise, <see langword="false"/>.</returns>
        bool TryGetIcon(bool large, out Icon icon);
    }
}
