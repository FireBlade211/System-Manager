using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireBlade.SystemManager.Plugin
{
    public enum ColorScheme
    {
        System,
        BlueLuna,
        OliveLuna,
        SilverLuna,
        NormalColor = BlueLuna,
        HomeStead = OliveLuna,
        Metallic = SilverLuna,
        Blue = BlueLuna,
        Olive = OliveLuna,
        Silver = SilverLuna,
        Royale
    }

    /// <summary>
    /// A contract that allows a plugin to retrieve theme information.
    /// </summary>
    public interface IThemeContract : IContract
    {
        /// <summary>
        /// Fired whenever the System Manager theme changes.
        /// </summary>
        /// <param name="scheme">The user-selected color scheme.</param>
        void OnColorSchemeChanged(ColorScheme scheme);
    }
}
