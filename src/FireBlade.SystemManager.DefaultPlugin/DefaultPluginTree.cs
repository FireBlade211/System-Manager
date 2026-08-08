using FireBlade.SystemManager.DefaultPlugin.Pages;
using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireBlade.SystemManager.DefaultPlugin
{
    public class DefaultPluginTree : IPluginTreeContract
    {
        public IPageNodeContract[] GetNodes(out string parentId)
        {
            parentId = "root";

            return new IPageNodeContract[] { new ServicesPageContract(), new InfoPageContract() };
        }

        public IContract QueryContract(string contractIdentifier) => null;
    }
}
