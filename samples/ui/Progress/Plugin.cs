using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireBlade.SystemManager.Plugin;

namespace Progress
{
    public class ProgressSamplePlugin : IPlugin
    {
        public IContract QueryContract(string contractIdentifier)
        {
            switch (contractIdentifier)
            {
                case ContractIdentifiers.PluginInfoContractIdentifier:
                    return new ProgressSamplePluginInfo();
                case ContractIdentifiers.PluginTreeContractIdentifier:
                    return new ProgressSamplePluginTree();
            }

            return null;
        }
    }
}
