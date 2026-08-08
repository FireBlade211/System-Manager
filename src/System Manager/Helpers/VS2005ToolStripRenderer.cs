//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace FireBlade.SystemManager.Helpers
//{
//    // rendering code ported from mfc afxvisualmanagervs2005.cpp
//    public class VS2005ToolStripRenderer : ToolStripRenderer
//    {
//        private bool DefaultWinxpColors = true;
//        private Color PressedButtonBorder;
//        private WinXpTheme CurrentAppTheme;

//        private Color ToolBarGradientDark;
//        private Color ToolBarGradientLight;
//        private Color BarGradientDark;
//        private Color BarGradientLight;
//        private Color Highlight;
//        private Color HighlightDark;
//        private Color HighlightChecked;
//        private Color CustomizeButtonGradientDark;
//        private Color CustomizeButtonGradientLight;
//        private Color ToolBarBottomLine;
//        private Color Gripper;
//        private Color MenuItemBorder;
//        private Color MenuBorder;
//        private Color ToolbarDisabled;

//        private IntPtr ThemeHandleWindow;
//        private IntPtr ThemeHandleToolbar;

//        private static Color FromColorRef(int colorRef)
//        {
//            return Color.FromArgb(
//                colorRef & 0xFF,
//                (colorRef >> 8) & 0xFF,
//                (colorRef >> 16) & 0xFF);
//        }

//        private Color GetBaseThemeColor()
//        {
//            return DefaultWinxpColors ? GetThemeColor(ThemeHandleWindow, 29) : GetGlobalData()->clrBarFace;
//        }

//        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
//        private static extern int GetCurrentThemeName(
//            StringBuilder pszThemeFileName,
//            int dwMaxNameChars,
//            StringBuilder pszColorBuff,
//            int cchMaxColorChars,
//            StringBuilder pszSizeBuff,
//            int cchMaxSizeChars);

//        private enum WinXpTheme
//        {
//            None,
//            NonStandard,
//            Blue,
//            Olive,
//            Silver
//        }

//        private WinXpTheme GetStandardWindowsTheme()
//        {
//            var themeName = new StringBuilder(256);
//            var colorName = new StringBuilder(256);
//            var sizeName = new StringBuilder(256);

//            int result = GetCurrentThemeName(
//                themeName, themeName.Capacity,
//                colorName, colorName.Capacity,
//                sizeName, sizeName.Capacity);

//            if (result != 0)
//                return WinXpTheme.None;

//            string themeFileName = Path.GetFileNameWithoutExtension(themeName.ToString());

//            if (!themeFileName.Equals("Luna", StringComparison.OrdinalIgnoreCase) &&
//    !themeFileName.Equals("Aero", StringComparison.OrdinalIgnoreCase))
//            {
//                return WinXpTheme.NonStandard;
//            }

//            string color = colorName.ToString();

//            if (color.Equals("normalcolor", StringComparison.OrdinalIgnoreCase))
//                return WinXpTheme.Blue;

//            if (color.Equals("homestead", StringComparison.OrdinalIgnoreCase))
//                return WinXpTheme.Olive;

//            if (color.Equals("metallic", StringComparison.OrdinalIgnoreCase))
//            {
//                if (themeName.ToString().Contains("royale"))
//                {
//                    return WinXpTheme.NonStandard;
//                }

//                return WinXpTheme.Silver;
//            }

//            return WinXpTheme.None;
//        }

//        private static Color PixelAlpha(Color source, double percent)
//        {
//            double factor = percent / 100.0;

//            return Color.FromArgb(
//                Clamp((int)Math.Round(source.R * factor)),
//                Clamp((int)Math.Round(source.G * factor)),
//                Clamp((int)Math.Round(source.B * factor)));
//        }

//        private static int Clamp(int value)
//            => Math.Max(0, Math.Min(255, value));

//        private static double Clamp(double value, double min, double max)
//            => Math.Max(min, Math.Min(max, value));

//        private static Color PixelAlpha(
//            Color source,
//            double percentR,
//            double percentG,
//            double percentB)
//        {
//            return Color.FromArgb(
//                Clamp((int)Math.Round(source.R * percentR)),
//                Clamp((int)Math.Round(source.G * percentG)),
//                Clamp((int)Math.Round(source.B * percentB)));
//        }

//        private static Color SmartMixColors(
//    Color color1,
//    Color color2,
//    double lumRatio = 1.0,
//    int k1 = 1,
//    int k2 = 1)
//        {
//            if (k1 < 0 || k2 < 0)
//                throw new ArgumentOutOfRangeException();

//            if (k1 == 0 && k2 == 0)
//                return Color.Black;

//            double total = k1 + k2;

//            int r = (int)Math.Round(
//                (color1.R * k1 + color2.R * k2) / total);

//            int g = (int)Math.Round(
//                (color1.G * k1 + color2.G * k2) / total);

//            int b = (int)Math.Round(
//                (color1.B * k1 + color2.B * k2) / total);

//            Color mixed = Color.FromArgb(r, g, b);

//            // Then modify its HSL luminosity.
//            HslFromRgb(mixed, out double h, out double s, out double l);

//            l *= lumRatio;
//            l = Clamp(l, 0.0, 1.0);

//            return HslToRgb(h, s, l);
//        }

//        private static void HslFromRgb(
//    Color color,
//    out double h,
//    out double s,
//    out double l)
//        {
//            double r = color.R / 255.0;
//            double g = color.G / 255.0;
//            double b = color.B / 255.0;

//            double max = Math.Max(r, Math.Max(g, b));
//            double min = Math.Min(r, Math.Min(g, b));

//            l = (max + min) / 2.0;

//            if (max == min)
//            {
//                h = 0.0;
//                s = 0.0;
//                return;
//            }

//            double delta = max - min;

//            s = l > 0.5
//                ? delta / (2.0 - max - min)
//                : delta / (max + min);

//            if (max == r)
//            {
//                h = (g - b) / delta;

//                if (g < b)
//                    h += 6.0;
//            }
//            else if (max == g)
//            {
//                h = (b - r) / delta + 2.0;
//            }
//            else
//            {
//                h = (r - g) / delta + 4.0;
//            }

//            h /= 6.0;
//        }

//        private static Color HslToRgb(
//    double h,
//    double s,
//    double l)
//        {
//            h %= 1.0;

//            if (h < 0.0)
//                h += 1.0;

//            s = Math.Max(0.0, Math.Min(1.0, s));
//            l = Math.Max(0.0, Math.Min(1.0, l));

//            if (s == 0.0)
//            {
//                int gray = (int)Math.Round(l * 255.0);

//                return Color.FromArgb(gray, gray, gray);
//            }

//            double q = l < 0.5
//                ? l * (1.0 + s)
//                : l + s - l * s;

//            double p = 2.0 * l - q;

//            double r = HueToRgb(p, q, h + 1.0 / 3.0);
//            double g = HueToRgb(p, q, h);
//            double b = HueToRgb(p, q, h - 1.0 / 3.0);

//            return Color.FromArgb(
//                Clamp((int)Math.Round(r * 255.0)),
//                Clamp((int)Math.Round(g * 255.0)),
//                Clamp((int)Math.Round(b * 255.0)));
//        }

//        private static double HueToRgb(
//            double p,
//            double q,
//            double t)
//        {
//            if (t < 0.0)
//                t += 1.0;

//            if (t > 1.0)
//                t -= 1.0;

//            if (t < 1.0 / 6.0)
//                return p + (q - p) * 6.0 * t;

//            if (t < 1.0 / 2.0)
//                return q;

//            if (t < 2.0 / 3.0)
//                return p + (q - p) * (2.0 / 3.0 - t) * 6.0;

//            return p;
//        }

//        [DllImport("uxtheme.dll", ExactSpelling = true)]
//        private static extern int GetThemeSysColor(
//    IntPtr hTheme,
//    int iColorId);

//        [DllImport("user32.dll", ExactSpelling = true)]
//        private static extern int GetSysColor(
//    int nIndex);

//        private Color GetThemeColor(IntPtr hTheme, int nIndex)
//        {
//            int cref = 0;

//	        if (hTheme != null)
//	        {
//		        cref = GetThemeSysColor(hTheme, nIndex);
//            }
//            else
//	            cref = GetSysColor(nIndex);

//            return FromColorRef(cref);
//        }

//        private void UpdateSystemColors2003()
//        {

//            CurrentAppTheme = DefaultWinxpColors
//                ? GetStandardWindowsTheme()
//                : WinXpTheme.None;

//            DefaultWinxpColors =
//                CurrentAppTheme == WinXpTheme.Blue ||
//                CurrentAppTheme == WinXpTheme.Olive ||
//                CurrentAppTheme == WinXpTheme.Silver;

//            Color baseColor = GetBaseThemeColor();

//            if (CurrentAppTheme == WinXpTheme.Olive)
//            {
//                ToolBarGradientDark =
//                    PixelAlpha(baseColor, 120);

//                BarGradientDark =
//                    SmartMixColors(
//                        baseColor,
//                        GetThemeColor(m_hThemeWindow, COLOR_3DFACE),
//                        0.87, 1, 3);

//                ToolBarGradientLight =
//                    SmartMixColors(
//                        baseColor,
//                        GetThemeColor(m_hThemeWindow, COLOR_WINDOW),
//                        1.0, 2, 1);

//                BarGradientLight =
//                    SmartMixColors(
//                        baseColor,
//                        GetThemeColor(m_hThemeWindow, COLOR_WINDOW),
//                        1.03);
//            }
//            else if (CurrentAppTheme == WinXpTheme.Silver)
//            {
//                ToolBarGradientDark =
//                    SmartMixColors(
//                        baseColor,
//                        GetThemeColor(m_hThemeWindow, COLOR_3DFACE),
//                        0.75, 2);

//                BarGradientDark =
//                    PixelAlpha(baseColor, 120);

//                ToolBarGradientLight =
//                    SmartMixColors(
//                        baseColor,
//                        GetThemeColor(m_hThemeWindow, COLOR_3DHIGHLIGHT),
//                        0.98);

//                BarGradientLight =
//                    SmartMixColors(
//                        baseColor,
//                        GetThemeColor(m_hThemeWindow, COLOR_WINDOW),
//                        1.03);
//            }
//            else if (CurrentAppTheme == WinXpTheme.Blue)
//            {
//                ToolBarGradientDark =
//                    SmartMixColors(
//                        baseColor,
//                        GetThemeColor(m_hThemeWindow, COLOR_3DFACE),
//                        0.93, 2);

//                BarGradientDark =
//                    SmartMixColors(
//                        baseColor,
//                        GetThemeColor(m_hThemeWindow, COLOR_3DLIGHT),
//                        0.99, 2, 1);

//                ToolBarGradientLight =
//                    SmartMixColors(
//                        baseColor,
//                        GetThemeColor(m_hThemeWindow, COLOR_WINDOW),
//                        1.03);

//                BarGradientLight = ToolBarGradientLight;
//            }
//            else
//            {
//                ToolBarGradientDark =
//                    SmartMixColors(
//                        baseColor,
//                        GetThemeColor(m_hThemeWindow, COLOR_3DFACE),
//                        0.93, 2);

//                BarGradientDark =
//                    SmartMixColors(
//                        baseColor,
//                        GetThemeColor(m_hThemeWindow, COLOR_3DLIGHT),
//                        0.99, 2, 1);

//                ToolBarGradientLight =
//                    SmartMixColors(
//                        baseColor,
//                        GetThemeColor(m_hThemeWindow, COLOR_WINDOW),
//                        1.0, 1, 4);

//                BarGradientLight = ToolBarGradientLight;
//            }

//            // These are still toolbar-related.
//            ToolBarGradientVertLight = ToolBarGradientLight;
//            ToolBarGradientVertDark = PixelAlpha(ToolBarGradientDark, 98);

//            Color clr1 = Color.FromArgb(250, 196, 88);
//            Color clr2 = Color.FromArgb(250, 196, 88);
//            Color clr3 = Color.FromArgb(228, 93, 61);

//            HighlightMenuItem =
//                SmartMixColors(clr1, clr2, 1.3, 1, 1);

//            HighlightGradientLight =
//                SmartMixColors(clr1, clr3, 1.55, 2, 1);

//            HighlightGradientDark =
//                SmartMixColors(clr1, clr2, 1.03, 2, 1);

//            HighlightDnGradientLight =
//                SmartMixColors(clr1, clr3, 1.03, 1, 2);

//            if (CurrentAppTheme == WinXpTheme.Blue ||
//                CurrentAppTheme == WinXpTheme.Silver)
//            {
//                CustomizeButtonGradientDark =
//                    Color.FromArgb(7, 57, 142);

//                double k = CurrentAppTheme == WinXpTheme.Blue
//                    ? 1.61
//                    : 1.0;

//                CustomizeButtonGradientLight =
//                    SmartMixColors(
//                        CustomizeButtonGradientDark,
//                        baseColor,
//                        k, 3, 1);
//            }

//            ToolBarBottomLine =
//    CurrentAppTheme == WinXpTheme.Silver
//        ? PixelAlpha(ToolBarGradientDark, 80)
//        : PixelAlpha(ToolBarGradientDark, 50);

//            Gripper =
//                PixelAlpha(ToolBarGradientVertDark, 40);

//            MenuItemBorder = Gripper;

//            MenuBorder =
//                PixelAlpha(baseColor, 80);

//            HighlightDnGradientDark = HighlightGradientDark;
//            HighlightCheckedGradientLight = HighlightDnGradientDark;

//            HighlightCheckedGradientDark =
//                PixelAlpha(HighlightDnGradientLight, 120);

//            ToolbarDisabled =
//                SmartMixColors(
//                    ToolBarGradientDark,
//                    ToolBarGradientLight,
//                    0.92);
//        }

//        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
//        private static extern IntPtr OpenThemeData(
//    IntPtr hwnd,
//    string pszClassList);

//        [DllImport("uxtheme.dll", ExactSpelling = true)]
//        private static extern int CloseThemeData(
//            IntPtr hTheme);

//        protected override void Initialize(ToolStrip toolStrip)
//        {
//            base.Initialize(toolStrip);

//            if (ThemeHandleWindow != IntPtr.Zero)
//                CloseThemeData(ThemeHandleWindow);

//            if (ThemeHandleToolbar != IntPtr.Zero)
//                CloseThemeData(ThemeHandleToolbar);

//            ThemeHandleWindow = OpenThemeData(toolStrip.Handle, "WINDOW");
//            ThemeHandleToolbar = OpenThemeData(toolStrip.Handle, "TOOLBAR");

//            bool bDefaultWinXPColors = DefaultWinxpColors;
//            PressedButtonBorder = FromColorRef(-1);
//            CurrentAppTheme = GetStandardWindowsTheme();

//            if (CurrentAppTheme != WinXpTheme.Silver)
//            {
//                DefaultWinxpColors = false;
//            }

//            UpdateSystemColors2003();

//            if (!bDefaultWinXPColors)
//            {
//                return;
//            }

//            COLORREF clrMenuButtonDroppedDown = m_clrBarBkgnd;
//            COLORREF clrMenuItemCheckedHighlight = m_clrHighlightDn;

//            if (m_hThemeComboBox == NULL || ::GetThemeColor(m_hThemeComboBox, 5, 0, 3801, &m_colorActiveTabBorder) != S_OK)
//            {
//                m_colorActiveTabBorder = (COLORREF) - 1;
//            }

//            if (GetGlobalData()->m_nBitsPerPixel > 8 && !GetGlobalData()->IsHighContrastMode())
//            {
//                m_clrCustomizeButtonGradientLight = CDrawingManager::SmartMixColors(m_clrCustomizeButtonGradientDark, GetGlobalData()->clrBarFace, 1.5, 1, 1);

//                if (m_CurrAppTheme == WinXpTheme_Blue || m_CurrAppTheme == WinXpTheme_Olive)
//                {
//                    m_clrToolBarGradientDark = CDrawingManager::PixelAlpha(m_clrToolBarGradientDark, 83);

//                    m_clrToolBarGradientLight = CDrawingManager::SmartMixColors(GetBaseThemeColor(), GetThemeColor(m_hThemeWindow, COLOR_WINDOW), 1., 3, 2);
//                }

//                if (m_CurrAppTheme == WinXpTheme_Blue)
//                {
//                    m_clrCustomizeButtonGradientDark = CDrawingManager::PixelAlpha(m_clrCustomizeButtonGradientDark, 90);
//                    m_clrCustomizeButtonGradientLight = CDrawingManager::PixelAlpha(m_clrCustomizeButtonGradientLight, 115);

//                    m_clrToolBarBottomLine = CDrawingManager::PixelAlpha(m_clrToolBarBottomLine, 85);
//                }
//                else if (m_CurrAppTheme == WinXpTheme_Olive)
//                {
//                    m_clrToolBarBottomLine = CDrawingManager::PixelAlpha(m_clrToolBarBottomLine, 110);

//                    m_clrCustomizeButtonGradientDark = m_clrToolBarBottomLine;

//                    m_clrCustomizeButtonGradientLight = CDrawingManager::PixelAlpha(m_clrCustomizeButtonGradientLight, 120);

//                    m_clrHighlightDn = GetGlobalData()->clrHilite;

//                    m_clrHighlight = CDrawingManager::PixelAlpha(m_clrHighlightDn, 124);
//                    m_clrHighlightChecked = CDrawingManager::PixelAlpha(GetThemeColor(m_hThemeWindow, 27 /*COLOR_GRADIENTACTIVECAPTION*/), 98);

//                    m_brHighlight.DeleteObject();
//                    m_brHighlightDn.DeleteObject();

//                    m_brHighlight.CreateSolidBrush(m_clrHighlight);
//                    m_brHighlightDn.CreateSolidBrush(m_clrHighlightDn);

//                    m_brHighlightChecked.DeleteObject();
//                    m_brHighlightChecked.CreateSolidBrush(m_clrHighlightChecked);
//                }
//                else if (m_CurrAppTheme != WinXpTheme_Silver)
//                {
//                    m_clrToolBarBottomLine = m_clrToolBarGradientDark;
//                }

//                clrMenuButtonDroppedDown = CDrawingManager::PixelAlpha(m_clrBarBkgnd, 107);

//                clrMenuItemCheckedHighlight = GetThemeColor(m_hThemeWindow, COLOR_HIGHLIGHT);

//                if (m_CurrAppTheme == WinXpTheme_Blue || m_CurrAppTheme == WinXpTheme_Olive)
//                {
//                    m_clrBarGradientLight = CDrawingManager::PixelAlpha(m_clrToolBarGradientLight, 95);
//                    m_clrBarGradientDark = CDrawingManager::PixelAlpha(m_clrBarGradientDark, 97);
//                }

//                m_clrToolbarDisabled = CDrawingManager::SmartMixColors(m_clrToolBarGradientDark, m_clrToolBarGradientLight, .92, 1, 2);
//                m_clrPressedButtonBorder = CDrawingManager::SmartMixColors(m_clrMenuItemBorder, GetGlobalData()->clrBarDkShadow, .8, 1, 2);
//            }

//            m_brMenuButtonDroppedDown.DeleteObject();
//            m_brMenuButtonDroppedDown.CreateSolidBrush(clrMenuButtonDroppedDown);

//            m_brMenuItemCheckedHighlight.DeleteObject();
//            m_brMenuItemCheckedHighlight.CreateSolidBrush(clrMenuItemCheckedHighlight);

//            m_penActiveTabBorder.DeleteObject();

//            if (m_colorActiveTabBorder != (COLORREF) - 1)
//            {
//                m_penActiveTabBorder.CreatePen(PS_SOLID, 1, m_colorActiveTabBorder);
//            }

//            m_bDefaultWinXPColors = bDefaultWinXPColors;

//            m_clrInactiveTabText = GetGlobalData()->clrBtnDkShadow;

//            if (GetGlobalData()->m_nBitsPerPixel > 8 && !GetGlobalData()->IsHighContrastMode())
//            {
//                m_penSeparator.DeleteObject();

//                COLORREF clrSeparator = CDrawingManager::PixelAlpha(GetGlobalData()->clrBarFace, 84);

//                m_penSeparator.CreatePen(PS_SOLID, 1, clrSeparator);
//            }
//        }
//    }
//}
