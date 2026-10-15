using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Progress
{
    public class ProgressPageNode : IPageContract
    {
        public string Id => "ProgressSample";

        public string DisplayName => "Progress";

        public string Description => "Shows how to utilize the SimpleProgressDialog API.";

        public UserControl CreateContent() => new ProgressPage();

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
