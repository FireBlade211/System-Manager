using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicTaskPane
{
    public class DynamicTaskPaneTreeContract : IPluginTreeContract
    {
        public IPageNodeContract[] GetNodes(out string parentId)
        {
            parentId = "root";

            return new[] { new SamplesContainerNode() };
        }

        public IContract QueryContract(string contractIdentifier) => null;
    }
}
