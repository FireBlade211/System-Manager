using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progress
{
    public class ProgressSamplePluginInfo : IPluginInfoContract
    {
        public string Name => "PROGRESS Plugin Sample";

        public string Description => "This sample demonstrates the SimpleProgressDialog helper API.";

        public IContract QueryContract(string contractIdentifier)
        {
            switch (contractIdentifier)
            {
                case ContractIdentifiers.IconContractIdentifier:
                    return new DllIconContract();
            }

            return null;
        }
    }
}
