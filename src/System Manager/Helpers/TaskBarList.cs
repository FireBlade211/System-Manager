using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FireBlade.SystemManager.Helpers
{
    public static class ComMethods
    {
        [DllImport("ole32.dll")]
        public static extern int CoInitializeEx(
    IntPtr pvReserved,
    COINIT dwCoInit);

        [DllImport("ole32.dll")]
        public static extern void CoUninitialize();
    }

    [Flags]
    public enum COINIT : uint
    {
        MULTITHREADED = 0x0,
        APARTMENTTHREADED = 0x2,
        DISABLE_OLE1DDE = 0x4,
        SPEED_OVER_MEMORY = 0x8
    }

    public enum TBPFLAG
    {
        TBPF_NOPROGRESS = 0x00000000,
        TBPF_INDETERMINATE = 0x00000001,
        TBPF_NORMAL = 0x00000002,
        TBPF_ERROR = 0x00000004,
        TBPF_PAUSED = 0x00000008
    }

    [ComImport]
    [Guid("EA1AFB91-9E28-4B86-90E9-9E9F8A5EEFAF")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITaskbarList3
    {
        void HrInit();

        void AddTab(IntPtr hwnd);

        void DeleteTab(IntPtr hwnd);

        void ActivateTab(IntPtr hwnd);

        void SetActiveAlt(IntPtr hwnd);

        void MarkFullscreenWindow(
    IntPtr hwnd,
    [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

        void SetProgressValue(
            IntPtr hwnd,
            ulong ullCompleted,
            ulong ullTotal);

        void SetProgressState(
            IntPtr hwnd,
            TBPFLAG tbpFlags);

        void RegisterTab(
            IntPtr hwndTab,
            IntPtr hwndMDI);

        void UnregisterTab(
            IntPtr hwndTab);

        void SetTabOrder(
            IntPtr hwndTab,
            IntPtr hwndInsertBefore);

        void SetTabActive(
            IntPtr hwndTab,
            IntPtr hwndMDI,
            uint dwReserved);

        void ThumbBarAddButtons(
            IntPtr hwnd,
            uint cButtons,
            [In] THUMBBUTTON[] pButtons);

        void ThumbBarUpdateButtons(
            IntPtr hwnd,
            uint cButtons,
            [In] THUMBBUTTON[] pButtons);

        void ThumbBarSetImageList(
            IntPtr hwnd,
            IntPtr himl);

        void SetOverlayIcon(
            IntPtr hwnd,
            IntPtr hIcon,
            [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);

        void SetThumbnailTooltip(
            IntPtr hwnd,
            [MarshalAs(UnmanagedType.LPWStr)] string pszTip);

        void SetThumbnailClip(
            IntPtr hwnd,
            ref RECT prcClip);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct THUMBBUTTON
    {
        public THUMBBUTTONMASK dwMask;
        public uint iId;
        public uint iBitmap;
        public IntPtr hIcon;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szTip;

        public THUMBBUTTONFLAGS dwFlags;
    }

    [Flags]
    public enum THUMBBUTTONMASK : uint
    {
        BITMAP = 0x1,
        ICON = 0x2,
        TOOLTIP = 0x4,
        FLAGS = 0x8
    }

    [Flags]
    public enum THUMBBUTTONFLAGS : uint
    {
        ENABLED = 0x0000,
        DISABLED = 0x0001,
        DISMISSONCLICK = 0x0002,
        NOBACKGROUND = 0x0004,
        HIDDEN = 0x0008,
        NONINTERACTIVE = 0x0010
    }

    [ComImport]
    [Guid("56FDF344-FD6D-11D0-958A-006097C9A090")]
    public class TaskbarList
    {
    }
}
