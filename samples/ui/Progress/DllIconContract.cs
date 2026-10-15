using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireBlade.SystemManager.Helpers;

namespace Progress
{
    public class DllIconContract : IIconContract
    {
        public IContract QueryContract(string contractIdentifier) => null;

        public bool TryGetIcon(bool large, out Icon icon)
        {
            icon = StockIconHelper.LoadIcon(Path.Combine(Environment.SystemDirectory, "imageres.dll"), -67, large);
            return true;
        }
    }
}
