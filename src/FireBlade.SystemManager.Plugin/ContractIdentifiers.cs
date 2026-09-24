using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireBlade.SystemManager.Plugin
{
    /// <summary>
    /// Represents the identifiers for the contracts used in the FireBlade System Manager plugin system.
    /// </summary>
    public static class ContractIdentifiers
    {
        /// <summary>
        /// Gets the identifier for the <see cref="IPlugin"/> contract.
        /// </summary>
        public const string PluginContractIdentifier = "FireBlade.SystemManager.Plugin.IPlugin";

        /// <summary>
        /// Gets the identifier for the <see cref="IIconContract"/> contract.
        /// </summary>
        public const string IconContractIdentifier = "FireBlade.SystemManager.Plugin.IIconContract";

        /// <summary>
        /// Gets the identifier for the <see cref="IPageContainerContract"/> contract.
        /// </summary>
        public const string PageContainerContractIdentifier = "FireBlade.SystemManager.Plugin.IPageContainerContract";

        /// <summary>
        /// Gets the identifier for the <see cref="IPageContract"/> contract.
        /// </summary>
        public const string PageContractIdentifier = "FireBlade.SystemManager.Plugin.IPageContract";

        /// <summary>
        /// Gets the identifier for the <see cref="IPageNodeContract"/> contract.
        /// </summary>
        public const string PageNodeContractIdentifier = "FireBlade.SystemManager.Plugin.IPageNodeContract";

        /// <summary>
        /// Gets the identifier for the <see cref="IPluginInfoContract"/> contract.
        /// </summary>
        public const string PluginInfoContractIdentifier = "FireBlade.SystemManager.Plugin.IPluginInfoContract";

        /// <summary>
        /// Gets the identifier for the <see cref="IPluginTreeContract"/> contract.
        /// </summary>
        public const string PluginTreeContractIdentifier = "FireBlade.SystemManager.Plugin.IPluginTreeContract";

        /// <summary>
        /// Gets the identifier for the <see cref="ISettingsContract"/> contract.
        /// </summary>
        public const string SettingsContractIdentifier = "FireBlade.SystemManager.Plugin.ISettingsContract";

        /// <summary>
        /// Gets the identifier for the <see cref="IAboutContract"/> contract.
        /// </summary>
        public const string AboutContractIdentifier = "FireBlade.SystemManager.Plugin.IAboutContract";

        /// <summary>
        /// Gets the identifier for the <see cref="IThemeContract"/> contract.
        /// </summary>
        public const string ThemeContractIdentifier = "FireBlade.SystemManager.Plugin.IThemeContract";
    }
}
