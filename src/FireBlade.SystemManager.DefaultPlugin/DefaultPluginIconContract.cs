using FireBlade.SystemManager.Plugin;
using FireBlade.SystemManager.Helpers;
using System.Drawing;

namespace FireBlade.SystemManager.DefaultPlugin
{
    public class DefaultPluginIconContract : IIconContract
    {
        public IContract QueryContract(string contractIdentifier) => null;

        public bool TryGetIcon(bool large, out Icon icon)
        {
            icon = StockIconHelper.GetIcon(StockIcon.Application, large ? StockIconOptions.Default : StockIconOptions.SmallIcon);
            return true;
        }
    }
}
