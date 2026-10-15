using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class HelloWorldInfo : IPluginInfoContract
    {
        public string Name => "Hello World Plugin Sample";

        public string Description => "This sample showcases a simple page that says Hello, World.";

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
