using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class HelloWorldPlugin : IPlugin
    {
        public IContract QueryContract(string contractIdentifier)
        {
            switch (contractIdentifier)
            {
                case ContractIdentifiers.PluginInfoContractIdentifier:
                    return new HelloWorldInfo();
                case ContractIdentifiers.PluginTreeContractIdentifier:
                    return new HelloWorldTree();
            }

            return null;
        }
    }
}
