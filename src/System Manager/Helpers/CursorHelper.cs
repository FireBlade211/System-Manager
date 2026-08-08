using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager.Helpers
{
    public static class CursorHelper
    {
        [DllImport("User32.dll")]
        private static extern IntPtr LoadCursorW(IntPtr hInstance, IntPtr lpCursorName);

        public static Cursor LoadCursor(CursorId id)
        {
            IntPtr hCursor = LoadCursorW(IntPtr.Zero, (IntPtr)id);

            if (hCursor != IntPtr.Zero)
            {
                // Note: We should not dispose system cursors, so we don't need to duplicate any shit
                var cur = new Cursor(hCursor);
                return cur;
            }

            return null;
        }
    }

    public enum CursorId
    {
        // TODO: Put more shit here
        LinkSelect = 32649
    }
}
