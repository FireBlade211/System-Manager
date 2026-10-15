using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace FireBlade.SystemManager.Helpers
{
    [Description("Represents a control that displays a heading.")]
    [ToolboxBitmap(typeof(HeadingControl), "FireBlade.SystemManager.Helpers.textbox.bmp")] // doesn't work
    [Obsolete("This control is obsolete; use a Label control instead. See Wording and Typography page in the" +
        " User Experience Guidelines section of the plugin documentation for more info.")]
    public class HeadingControl : Control
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr OpenThemeData(
            IntPtr hwnd,
            string pszClassList);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int DrawThemeText(
            IntPtr hTheme,
            IntPtr hdc,
            int iPartId,
            int iStateId,
            string pszText,
            int iCharCount,
            uint dwTextFlags,
            uint dwTextFlags2,
            ref RECT pRect);

        [DllImport("uxtheme.dll", SetLastError = true)]
        private static extern int CloseThemeData(
            IntPtr hTheme);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public override string Text { get => base.Text; set => base.Text = value; }

        private const uint DT_TOP = 0x00000000;
        private const uint DT_LEFT = 0x00000000;
        private const uint DT_CENTER = 0x00000001;
        private const uint DT_RIGHT = 0x00000002;
        private const uint DT_VCENTER = 0x00000004;
        private const uint DT_BOTTOM = 0x00000008;
        private const uint DT_WORDBREAK = 0x00000010;
        private const uint DT_SINGLELINE = 0x00000020;
        private const uint DT_EXPANDTABS = 0x00000040;
        private const uint DT_TABSTOP = 0x00000080;
        private const uint DT_NOCLIP = 0x00000100;
        private const uint DT_EXTERNALLEADING = 0x00000200;
        private const uint DT_NOPREFIX = 0x00000800;
        private const uint DT_EDITCONTROL = 0x00002000;

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        public ContentAlignment Alignment
        {
            get;
            set
            {
                field = value;
                Invalidate();
            }
        }

        [DefaultValue(false)]
        public bool SingleLine
        {
            get;
            set
            {
                field = value;
                Invalidate();
            }
        }

        [DefaultValue(false)]
        public bool WordBreak
        {
            get;
            set
            {
                field = value;
                Invalidate();
            }
        }

        [DefaultValue(false)]
        public bool ExpandTabs
        {
            get;
            set
            {
                field = value;
                Invalidate();
            }
        }

        [DefaultValue(false)]
        public bool NoClip
        {
            get;
            set
            {
                field = value;
                Invalidate();
            }
        }

        [DefaultValue(false)]
        [Description("Duplicates the text-displaying characteristics of a multiline TextBox control. Specifically, the" +
            " average character width is calculated in the same manner as for a TextBox control, and the function does not" +
            " display a partially visible last line.")]
        public bool EditLike
        {
            get;
            set
            {
                field = value;
                Invalidate();
            }
        }

        [DefaultValue(false)]
        public bool TextTabStop
        {
            get;
            set
            {
                field = value;
                Invalidate();
            }
        }

        [DefaultValue(false)]
        public bool IncludeExternalLeading
        {
            get;
            set
            {
                field = value;
                Invalidate();
            }
        }

        [DefaultValue(false)]
        public bool IgnoreMnemonic
        {
            get;
            set
            {
                field = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            nint hdc = e.Graphics.GetHdc();
            try
            {
                nint hTheme = OpenThemeData(Handle, "TEXTSTYLE");
                if (hTheme != IntPtr.Zero)
                {
                    RECT rc = new RECT();
                    rc.Left = 0;
                    rc.Top = 0;
                    rc.Right = Width;
                    rc.Bottom = Height;

                    uint flags = 0;
                    flags |= Alignment switch
                    {
                        ContentAlignment.TopLeft => DT_TOP | DT_LEFT,
                        ContentAlignment.TopCenter => DT_TOP | DT_CENTER,
                        ContentAlignment.BottomCenter => DT_BOTTOM | DT_CENTER,
                        ContentAlignment.BottomLeft => DT_BOTTOM | DT_LEFT,
                        ContentAlignment.BottomRight => DT_BOTTOM | DT_RIGHT,
                        ContentAlignment.MiddleCenter => DT_VCENTER | DT_CENTER,
                        ContentAlignment.MiddleLeft => DT_VCENTER | DT_LEFT,
                        ContentAlignment.MiddleRight => DT_VCENTER | DT_RIGHT,
                        ContentAlignment.TopRight => DT_TOP | DT_RIGHT,
                        _ => 0
                    };

                    if (SingleLine) flags |= DT_SINGLELINE;
                    if (EditLike) flags |= DT_EDITCONTROL;
                    if (WordBreak) flags |= DT_WORDBREAK;
                    if (ExpandTabs) flags |= DT_EXPANDTABS;
                    if (TextTabStop) flags |= DT_TABSTOP;
                    if (NoClip) flags |= DT_NOCLIP;
                    if (IncludeExternalLeading) flags |= DT_EXTERNALLEADING;
                    if (IgnoreMnemonic) flags |= DT_NOPREFIX;

                    DrawThemeText(hTheme, hdc, 1, 0, Text, -1, flags, 0, ref rc);

                    CloseThemeData(hTheme);
                }
            }
            finally
            {
                e.Graphics.ReleaseHdc(hdc);
            }
        }
    }
}
