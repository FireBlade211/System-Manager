using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FireBlade.SystemManager.Helpers
{
    public static class NativeHelper
    {
        [DllImport("User32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam);
    }
}
