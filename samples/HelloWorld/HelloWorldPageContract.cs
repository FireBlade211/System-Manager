using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloWorld
{
    public class HelloWorldPageContract : IPageContract
    {
        public string Id => "HelloWorldSample";

        public string DisplayName => "Hello World Sample";

        public string Description => "Hello, World!";

        public UserControl CreateContent() => new HelloWorldPage();

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
