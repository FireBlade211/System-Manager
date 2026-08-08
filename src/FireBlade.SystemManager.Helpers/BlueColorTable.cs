using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FireBlade.SystemManager.Helpers
{
    public class BlueColorTable : ProfessionalColorTable
    {
        // Token: 0x17000F29 RID: 3881
        // (get) Token: 0x06004B69 RID: 19305 RVA: 0x00110D94 File Offset: 0x0010FD94
        private Dictionary<BlueColorTable.KnownColors, Color> ColorTable
        {
            get
            {
                if (this.UseSystemColors)
                {
                    if (!this.usingSystemColors || this.professionalRGB == null)
                    {
                        if (this.professionalRGB == null)
                        {
                            this.professionalRGB = new Dictionary<BlueColorTable.KnownColors, Color>(212);
                        }
                        this.InitSystemColors(ref this.professionalRGB);
                    }
                }
                else if (ToolStripManager.VisualStylesEnabled)
                {
                    if (this.usingSystemColors || this.professionalRGB == null)
                    {
                        if (this.professionalRGB == null)
                        {
                            this.professionalRGB = new Dictionary<BlueColorTable.KnownColors, Color>(212);
                        }
                        this.InitThemedColors(ref this.professionalRGB);
                    }
                }
                else if (!this.usingSystemColors || this.professionalRGB == null)
                {
                    if (this.professionalRGB == null)
                    {
                        this.professionalRGB = new Dictionary<BlueColorTable.KnownColors, Color>(212);
                    }
                    this.InitSystemColors(ref this.professionalRGB);
                }
                return this.professionalRGB;
            }
        }

        // Token: 0x17000F2A RID: 3882
        // (get) Token: 0x06004B6A RID: 19306 RVA: 0x00110E59 File Offset: 0x0010FE59
        // (set) Token: 0x06004B6B RID: 19307 RVA: 0x00110E61 File Offset: 0x0010FE61
        //public new bool UseSystemColors
        //{
        //    get
        //    {
        //        return this.useSystemColors;
        //    }
        //    set
        //    {
        //        if (this.useSystemColors != value)
        //        {
        //            this.useSystemColors = value;
        //            this.ResetRGBTable();
        //        }
        //    }
        //}

        // Token: 0x06004B6C RID: 19308 RVA: 0x00110E7C File Offset: 0x0010FE7C
        internal Color FromKnownColor(BlueColorTable.KnownColors color)
        {
            //if (ProfessionalColors.ColorFreshnessKey != this.colorFreshnessKey || ProfessionalColors.ColorScheme != this.lastKnownColorScheme)
            //{
            //    this.ResetRGBTable();
            //}
            //this.colorFreshnessKey = ProfessionalColors.ColorFreshnessKey;
            //this.lastKnownColorScheme = ProfessionalColors.ColorScheme;
            return this.ColorTable[color];
        }

        // Token: 0x06004B6D RID: 19309 RVA: 0x00110ED0 File Offset: 0x0010FED0
        private void ResetRGBTable()
        {
            if (this.professionalRGB != null)
            {
                this.professionalRGB.Clear();
            }
            this.professionalRGB = null;
        }

        public override Color ButtonSelectedHighlight
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.ButtonSelectedHighlight);
            }
        }

        public override Color ButtonSelectedHighlightBorder
        {
            get
            {
                return this.ButtonPressedBorder;
            }
        }

        public override Color ButtonPressedHighlight
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.ButtonPressedHighlight);
            }
        }

        public override Color ButtonPressedHighlightBorder
        {
            get
            {
                return SystemColors.Highlight;
            }
        }

        public override Color ButtonCheckedHighlight
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.ButtonCheckedHighlight);
            }
        }

        public override Color ButtonCheckedHighlightBorder
        {
            get
            {
                return SystemColors.Highlight;
            }
        }

        public override Color ButtonPressedBorder
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver);
            }
        }

        public override Color ButtonSelectedBorder
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver);
            }
        }

        public override Color ButtonCheckedGradientBegin
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradSelectedBegin);
            }
        }

        public override Color ButtonCheckedGradientMiddle
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradSelectedMiddle);
            }
        }

        public override Color ButtonCheckedGradientEnd
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradSelectedEnd);
            }
        }

        public override Color ButtonSelectedGradientBegin
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMouseOverBegin);
            }
        }

        public override Color ButtonSelectedGradientMiddle
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle);
            }
        }

        public override Color ButtonSelectedGradientEnd
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMouseOverEnd);
            }
        }

        public override Color ButtonPressedGradientBegin
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMouseDownBegin);
            }
        }

        public override Color ButtonPressedGradientMiddle
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle);
            }
        }

        public override Color ButtonPressedGradientEnd
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMouseDownEnd);
            }
        }

        public override Color CheckBackground
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBCtlBkgdSelected);
            }
        }

        public override Color CheckSelectedBackground
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver);
            }
        }

        public override Color CheckPressedBackground
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver);
            }
        }

        public override Color GripDark
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBDragHandle);
            }
        }

        public override Color GripLight
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBDragHandleShadow);
            }
        }

        public override Color ImageMarginGradientBegin
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradVertBegin);
            }
        }

        public override Color ImageMarginGradientMiddle
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradVertMiddle);
            }
        }

        public override Color ImageMarginGradientEnd
        {
            get
            {
                if (!this.usingSystemColors)
                {
                    return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradVertEnd);
                }
                return SystemColors.Control;
            }
        }

        public override Color ImageMarginRevealedGradientBegin
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin);
            }
        }

        public override Color ImageMarginRevealedGradientMiddle
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle);
            }
        }

        public override Color ImageMarginRevealedGradientEnd
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd);
            }
        }

        public override Color MenuStripGradientBegin
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin);
            }
        }

        public override Color MenuStripGradientEnd
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd);
            }
        }

        public override Color MenuItemSelected
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver);
            }
        }

        public override Color MenuItemBorder
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBCtlBdrSelected);
            }
        }

        public override Color MenuBorder
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBMenuBdrOuter);
            }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMouseOverBegin);
            }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMouseOverEnd);
            }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin);
            }
        }

        public override Color MenuItemPressedGradientMiddle
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle);
            }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd);
            }
        }

        public override Color RaftingContainerGradientBegin
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin);
            }
        }

        public override Color RaftingContainerGradientEnd
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd);
            }
        }

        public override Color SeparatorDark
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBSplitterLine);
            }
        }

        public override Color SeparatorLight
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBSplitterLineLight);
            }
        }

        public override Color StatusStripGradientBegin
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin);
            }
        }

        public override Color StatusStripGradientEnd
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd);
            }
        }

        public override Color ToolStripBorder
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBShadow);
            }
        }

        public override Color ToolStripDropDownBackground
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBMenuBkgd);
            }
        }

        public override Color ToolStripGradientBegin
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradVertBegin);
            }
        }

        public override Color ToolStripGradientMiddle
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradVertMiddle);
            }
        }

        public override Color ToolStripGradientEnd
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradVertEnd);
            }
        }

        public override Color ToolStripContentPanelGradientBegin
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin);
            }
        }

        public override Color ToolStripContentPanelGradientEnd
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd);
            }
        }

        public override Color ToolStripPanelGradientBegin
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin);
            }
        }

        public override Color ToolStripPanelGradientEnd
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd);
            }
        }

        public override Color OverflowButtonGradientBegin
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradOptionsBegin);
            }
        }

        public override Color OverflowButtonGradientMiddle
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradOptionsMiddle);
            }
        }

        public override Color OverflowButtonGradientEnd
        {
            get
            {
                return this.FromKnownColor(BlueColorTable.KnownColors.msocbvcrCBGradOptionsEnd);
            }
        }


        internal Color ComboBoxButtonGradientBegin
        {
            get
            {
                return this.MenuItemPressedGradientBegin;
            }
        }

        internal Color ComboBoxButtonGradientEnd
        {
            get
            {
                return this.MenuItemPressedGradientEnd;
            }
        }

        internal Color ComboBoxButtonSelectedGradientBegin
        {
            get
            {
                return this.MenuItemSelectedGradientBegin;
            }
        }

        internal Color ComboBoxButtonSelectedGradientEnd
        {
            get
            {
                return this.MenuItemSelectedGradientEnd;
            }
        }

        internal Color ComboBoxButtonPressedGradientBegin
        {
            get
            {
                return this.ButtonPressedGradientBegin;
            }
        }

        internal Color ComboBoxButtonPressedGradientEnd
        {
            get
            {
                return this.ButtonPressedGradientEnd;
            }
        }

        internal Color ComboBoxButtonOnOverflow
        {
            get
            {
                return this.ToolStripDropDownBackground;
            }
        }

        internal Color ComboBoxBorder
        {
            get
            {
                return this.ButtonSelectedHighlightBorder;
            }
        }

        internal Color TextBoxBorder
        {
            get
            {
                return this.ButtonSelectedHighlightBorder;
            }
        }

        private static Color GetAlphaBlendedColor(Graphics g, Color src, Color dest, int alpha)
        {
            int num = ((int)src.R * alpha + (255 - alpha) * (int)dest.R) / 255;
            int num2 = ((int)src.G * alpha + (255 - alpha) * (int)dest.G) / 255;
            int num3 = ((int)src.B * alpha + (255 - alpha) * (int)dest.B) / 255;
            int num4 = ((int)src.A * alpha + (255 - alpha) * (int)dest.A) / 255;
            if (g == null)
            {
                return Color.FromArgb(num4, num, num2, num3);
            }
            return g.GetNearestColor(Color.FromArgb(num4, num, num2, num3));
        }

        // Token: 0x06004BB0 RID: 19376 RVA: 0x0011121C File Offset: 0x0011021C
        private static Color GetAlphaBlendedColorHighRes(Graphics graphics, Color src, Color dest, int alpha)
        {
            int num;
            int num2;
            if (alpha < 100)
            {
                num = 100 - alpha;
                num2 = 100;
            }
            else
            {
                num = 1000 - alpha;
                num2 = 1000;
            }
            int num3 = (alpha * (int)src.R + num * (int)dest.R + num2 / 2) / num2;
            int num4 = (alpha * (int)src.G + num * (int)dest.G + num2 / 2) / num2;
            int num5 = (alpha * (int)src.B + num * (int)dest.B + num2 / 2) / num2;
            if (graphics == null)
            {
                return Color.FromArgb(num3, num4, num5);
            }
            return graphics.GetNearestColor(Color.FromArgb(num3, num4, num5));
        }

        // Token: 0x06004BB1 RID: 19377 RVA: 0x001112BC File Offset: 0x001102BC
        private void InitCommonColors(ref Dictionary<BlueColorTable.KnownColors, Color> rgbTable)
        {
            //if (!DisplayInformation.LowResolution)
            //{
                using (Bitmap bitmap = new Bitmap(1, 1))
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    rgbTable[BlueColorTable.KnownColors.ButtonPressedHighlight] = BlueColorTable.GetAlphaBlendedColor(graphics, SystemColors.Window, BlueColorTable.GetAlphaBlendedColor(graphics, SystemColors.Highlight, SystemColors.Window, 160), 50);
                    rgbTable[BlueColorTable.KnownColors.ButtonCheckedHighlight] = BlueColorTable.GetAlphaBlendedColor(graphics, SystemColors.Window, BlueColorTable.GetAlphaBlendedColor(graphics, SystemColors.Highlight, SystemColors.Window, 80), 20);
                    rgbTable[BlueColorTable.KnownColors.ButtonSelectedHighlight] = rgbTable[BlueColorTable.KnownColors.ButtonCheckedHighlight];
                    return;
                }
            //}
            //rgbTable[BlueColorTable.KnownColors.ButtonPressedHighlight] = SystemColors.Highlight;
            //rgbTable[BlueColorTable.KnownColors.ButtonCheckedHighlight] = SystemColors.ControlLight;
            //rgbTable[BlueColorTable.KnownColors.ButtonSelectedHighlight] = SystemColors.ControlLight;
        }

        // Token: 0x06004BB2 RID: 19378 RVA: 0x0011139C File Offset: 0x0011039C
        internal void InitSystemColors(ref Dictionary<BlueColorTable.KnownColors, Color> rgbTable)
        {
            this.usingSystemColors = true;
            this.InitCommonColors(ref rgbTable);
            Color buttonFace = SystemColors.ButtonFace;
            Color buttonShadow = SystemColors.ButtonShadow;
            Color highlight = SystemColors.Highlight;
            Color window = SystemColors.Window;
            Color empty = Color.Empty;
            Color controlText = SystemColors.ControlText;
            Color buttonHighlight = SystemColors.ButtonHighlight;
            Color grayText = SystemColors.GrayText;
            Color highlightText = SystemColors.HighlightText;
            Color windowText = SystemColors.WindowText;
            Color color = buttonFace;
            Color color2 = buttonFace;
            Color color3 = buttonFace;
            Color color4 = highlight;
            Color color5 = highlight;
            bool lowResolution = /* DisplayInformation.LowResolution */ false;
            bool highContrast = SystemInformation.HighContrast;
            if (lowResolution)
            {
                color4 = window;
            }
            else if (!highContrast)
            {
                color = BlueColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 23);
                color2 = BlueColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 50);
                color3 = SystemColors.ButtonFace;
                color4 = BlueColorTable.GetAlphaBlendedColorHighRes(null, highlight, window, 30);
                color5 = BlueColorTable.GetAlphaBlendedColorHighRes(null, highlight, window, 50);
            }
            if (lowResolution || highContrast)
            {
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBBkgd] = buttonFace;
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = SystemColors.ControlLight;
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBDragHandle] = controlText;
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = buttonFace;
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = buttonShadow;
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = buttonShadow;
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = buttonShadow;
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = buttonShadow;
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = buttonShadow;
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = controlText;
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuBkgd] = window;
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBSplitterLine] = buttonShadow;
            }
            else
            {
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBBkgd] = BlueColorTable.GetAlphaBlendedColorHighRes(null, window, buttonFace, 165);
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = BlueColorTable.GetAlphaBlendedColorHighRes(null, highlight, window, 50);
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBDragHandle] = BlueColorTable.GetAlphaBlendedColorHighRes(null, buttonShadow, window, 75);
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = BlueColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 205);
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = BlueColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 70);
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = BlueColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 90);
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = BlueColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 40);
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = BlueColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 70);
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = BlueColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 90);
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = BlueColorTable.GetAlphaBlendedColorHighRes(null, controlText, buttonShadow, 20);
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuBkgd] = BlueColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 143);
                rgbTable[BlueColorTable.KnownColors.msocbvcrCBSplitterLine] = BlueColorTable.GetAlphaBlendedColorHighRes(null, buttonShadow, window, 70);
            }
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = (lowResolution ? SystemColors.ControlLight : highlight);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBdrOuterFloating] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrMouseDown] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrSelected] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgd] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdLight] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdMouseDown] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlText] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextDisabled] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextLight] = grayText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextMouseDown] = highlightText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = windowText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDockSeparatorLine] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDragHandleShadow] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDropDownArrow] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseOverEnd] = color4;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseOverBegin] = color4;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle] = color4;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsEnd] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsSelectedBegin] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsSelectedEnd] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradSelectedBegin] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradSelectedEnd] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradSelectedMiddle] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradVertBegin] = color;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradVertMiddle] = color2;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradVertEnd] = color3;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseDownBegin] = color5;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle] = color5;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseDownEnd] = color5;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = color;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = color2;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBIconDisabledDark] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBIconDisabledLight] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBLabelBkgnd] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBLowColorIconDisabled] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMainMenuBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuCtlText] = windowText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuCtlTextDisabled] = grayText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuIconBkgd] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuShadow] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuSplitArrow] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBOptionsButtonShadow] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBShadow] = rgbTable[BlueColorTable.KnownColors.msocbvcrCBBkgd];
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBSplitterLineLight] = buttonHighlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTearOffHandle] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTearOffHandleMouseOver] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTitleBkgd] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTitleText] = buttonHighlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDisabledFocuslessHighlightedText] = grayText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDisabledHighlightedText] = grayText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDlgGroupBoxText] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdr] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDark] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseDown] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = SystemColors.MenuText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLight] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseDown] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = SystemColors.MenuText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseDown] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = SystemColors.MenuText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrSelected] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdMouseDown] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdSelected] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabText] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseDown] = highlightText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextSelected] = windowText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabText] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabText] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabText] = buttonHighlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabText] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabBkgdMouseDown] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabBkgdMouseOver] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabTextMouseDown] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabTextMouseOver] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = SystemColors.InactiveCaption;
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = SystemColors.InactiveCaptionText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderBdr] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderBkgd] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderCellBdr] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderCellBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderCellBkgdSelected] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderSeeThroughSelection] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPDarkBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPDarkBkgd] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentDarkBkgd] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentLightBkgd] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentText] = windowText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentTextDisabled] = grayText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = windowText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupline] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupline] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPHyperlink] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPLightBkgd] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrHyperlink] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrHyperlinkFollowed] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIBdr] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIBdr] = windowText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradEnd] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIText] = windowText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrListHeaderArrow] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrNetLookBkgnd] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOABBkgd] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOBBkgdBdr] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOBBkgdBdrContrast] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerActiveBkgd] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerBdr] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerInactiveBkgd] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerTabBoxBdr] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = buttonHighlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerTabStopTicks] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerText] = windowText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGWorkspaceBkgd] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFlagNone] = buttonHighlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFolderbarDark] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFolderbarLight] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFolderbarText] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGridlines] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupLine] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupNested] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupShaded] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupText] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKIconBar] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKInfoBarBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKInfoBarText] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKPreviewPaneLabelText] = windowText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKTodayIndicatorDark] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKTodayIndicatorLight] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBActionDividerLine] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBButtonDark] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBButtonLight] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBButtonLight] = buttonHighlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBDarkOutline] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBFoldersBackground] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBHoverButtonDark] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBHoverButtonLight] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBLabelText] = windowText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBPressedButtonDark] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBPressedButtonLight] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSelectedButtonDark] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSelectedButtonLight] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSplitterDark] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrPlacesBarBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = windowText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelected] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrInactiveSelected] = grayText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrMouseOver] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrPubWebDocScratchPageBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrSBBdr] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrScrollbarBkgd] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrToastGradBegin] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrToastGradEnd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBdrInnerDocked] = empty;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBdrOuterDocked] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBdrOuterFloating] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBkgd] = window;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdr] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdrDisabled] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBkgd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBkgdDisabled] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlText] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlTextDisabled] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlTextMouseDown] = highlightText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPGroupline] = buttonShadow;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPInfoTipBkgd] = SystemColors.Info;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPInfoTipText] = SystemColors.InfoText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPNavBarBkgnd] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPText] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPText] = windowText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTextDisabled] = grayText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleBkgdActive] = highlight;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleBkgdInactive] = buttonFace;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleTextActive] = highlightText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleTextInactive] = controlText;
            rgbTable[BlueColorTable.KnownColors.msocbvcrXLFormulaBarBkgd] = buttonFace;
        }

        // Token: 0x06004BB3 RID: 19379 RVA: 0x00111FC4 File Offset: 0x00110FC4
        internal void InitOliveLunaColors(ref Dictionary<BlueColorTable.KnownColors, Color> rgbTable)
        {
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(81, 94, 51);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(81, 94, 51);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBdrOuterFloating] = Color.FromArgb(116, 134, 94);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBkgd] = Color.FromArgb(209, 222, 173);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrMouseDown] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrSelected] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgd] = Color.FromArgb(209, 222, 173);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdLight] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdMouseDown] = Color.FromArgb(254, 128, 62);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = Color.FromArgb(255, 192, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = Color.FromArgb(254, 128, 62);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextDisabled] = Color.FromArgb(141, 141, 141);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextLight] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDockSeparatorLine] = Color.FromArgb(96, 119, 66);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDragHandle] = Color.FromArgb(81, 94, 51);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDragHandleShadow] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDropDownArrow] = Color.FromArgb(236, 233, 216);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin] = Color.FromArgb(217, 217, 167);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = Color.FromArgb(242, 241, 228);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = Color.FromArgb(230, 230, 209);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = Color.FromArgb(160, 177, 116);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = Color.FromArgb(186, 201, 143);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = Color.FromArgb(237, 240, 214);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = Color.FromArgb(181, 196, 143);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseDownBegin] = Color.FromArgb(254, 128, 62);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseDownEnd] = Color.FromArgb(255, 223, 154);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle] = Color.FromArgb(255, 177, 109);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseOverBegin] = Color.FromArgb(255, 255, 222);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseOverEnd] = Color.FromArgb(255, 203, 136);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle] = Color.FromArgb(255, 225, 172);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = Color.FromArgb(186, 204, 150);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsEnd] = Color.FromArgb(96, 119, 107);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = Color.FromArgb(141, 160, 107);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = Color.FromArgb(255, 255, 222);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = Color.FromArgb(255, 193, 118);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = Color.FromArgb(255, 225, 172);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsSelectedBegin] = Color.FromArgb(254, 140, 73);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsSelectedEnd] = Color.FromArgb(255, 221, 152);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = Color.FromArgb(255, 184, 116);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradSelectedBegin] = Color.FromArgb(255, 223, 154);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradSelectedEnd] = Color.FromArgb(255, 166, 76);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradSelectedMiddle] = Color.FromArgb(255, 195, 116);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradVertBegin] = Color.FromArgb(255, 255, 237);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradVertEnd] = Color.FromArgb(181, 196, 143);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradVertMiddle] = Color.FromArgb(206, 220, 167);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBIconDisabledDark] = Color.FromArgb(131, 144, 113);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBIconDisabledLight] = Color.FromArgb(243, 244, 240);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(218, 227, 187);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(218, 227, 187);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBLowColorIconDisabled] = Color.FromArgb(159, 174, 122);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMainMenuBkgd] = Color.FromArgb(236, 233, 216);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = Color.FromArgb(117, 141, 94);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuBkgd] = Color.FromArgb(244, 244, 238);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuCtlText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuCtlTextDisabled] = Color.FromArgb(141, 141, 141);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuIconBkgd] = Color.FromArgb(216, 227, 182);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(173, 181, 157);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(173, 181, 157);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuShadow] = Color.FromArgb(134, 148, 108);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuSplitArrow] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBOptionsButtonShadow] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBShadow] = Color.FromArgb(96, 128, 88);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBSplitterLine] = Color.FromArgb(96, 128, 88);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBSplitterLineLight] = Color.FromArgb(244, 247, 222);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTearOffHandle] = Color.FromArgb(197, 212, 159);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTearOffHandleMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTitleBkgd] = Color.FromArgb(116, 134, 94);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTitleText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDisabledFocuslessHighlightedText] = Color.FromArgb(172, 168, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDisabledHighlightedText] = Color.FromArgb(220, 224, 208);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDlgGroupBoxText] = Color.FromArgb(153, 84, 10);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdr] = Color.FromArgb(96, 119, 107);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDark] = Color.FromArgb(176, 194, 140);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseDown] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLight] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseDown] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseDown] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(63, 93, 56);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrSelected] = Color.FromArgb(96, 128, 88);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgd] = Color.FromArgb(218, 227, 187);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdSelected] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseDown] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextSelected] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(218, 227, 187);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(218, 227, 187);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(183, 198, 145);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(183, 198, 145);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabTextMouseDown] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(236, 233, 216);
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(236, 233, 216);
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderBdr] = Color.FromArgb(191, 191, 223);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderBkgd] = Color.FromArgb(239, 235, 222);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderCellBdr] = Color.FromArgb(126, 125, 104);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderCellBkgd] = Color.FromArgb(239, 235, 222);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderCellBkgdSelected] = Color.FromArgb(255, 192, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderSeeThroughSelection] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(159, 171, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(159, 171, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentDarkBkgd] = Color.FromArgb(217, 227, 187);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentLightBkgd] = Color.FromArgb(230, 234, 208);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentTextDisabled] = Color.FromArgb(150, 145, 133);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = Color.FromArgb(161, 176, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = Color.FromArgb(210, 223, 174);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(90, 107, 70);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(90, 107, 70);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPHyperlink] = Color.FromArgb(0, 61, 178);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPLightBkgd] = Color.FromArgb(243, 242, 231);
            rgbTable[BlueColorTable.KnownColors.msocbvcrHyperlink] = Color.FromArgb(0, 61, 178);
            rgbTable[BlueColorTable.KnownColors.msocbvcrHyperlinkFollowed] = Color.FromArgb(170, 0, 170);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(96, 128, 88);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(96, 128, 88);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(217, 217, 167);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(217, 217, 167);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradEnd] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(242, 241, 228);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(242, 241, 228);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrListHeaderArrow] = Color.FromArgb(172, 168, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrNetLookBkgnd] = Color.FromArgb(255, 255, 237);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOABBkgd] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOBBkgdBdr] = Color.FromArgb(211, 211, 211);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOBBkgdBdrContrast] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = Color.FromArgb(151, 160, 123);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerActiveBkgd] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerBdr] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerBkgd] = Color.FromArgb(226, 231, 191);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerInactiveBkgd] = Color.FromArgb(171, 192, 138);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerTabBoxBdr] = Color.FromArgb(117, 141, 94);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerTabStopTicks] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = Color.FromArgb(218, 227, 187);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGWorkspaceBkgd] = Color.FromArgb(151, 160, 123);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFlagNone] = Color.FromArgb(242, 240, 228);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFolderbarDark] = Color.FromArgb(96, 119, 66);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFolderbarLight] = Color.FromArgb(175, 192, 130);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFolderbarText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGridlines] = Color.FromArgb(234, 233, 225);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupLine] = Color.FromArgb(181, 196, 143);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupNested] = Color.FromArgb(253, 238, 201);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupShaded] = Color.FromArgb(175, 186, 145);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupText] = Color.FromArgb(115, 137, 84);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKIconBar] = Color.FromArgb(253, 247, 233);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKInfoBarBkgd] = Color.FromArgb(151, 160, 123);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKInfoBarText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKPreviewPaneLabelText] = Color.FromArgb(151, 160, 123);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKTodayIndicatorDark] = Color.FromArgb(187, 85, 3);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKTodayIndicatorLight] = Color.FromArgb(251, 200, 79);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBActionDividerLine] = Color.FromArgb(200, 212, 172);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBButtonDark] = Color.FromArgb(176, 191, 138);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(234, 240, 207);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(234, 240, 207);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBDarkOutline] = Color.FromArgb(96, 128, 88);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBFoldersBackground] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBHoverButtonDark] = Color.FromArgb(247, 190, 87);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBHoverButtonLight] = Color.FromArgb(255, 255, 220);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBLabelText] = Color.FromArgb(50, 69, 105);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBPressedButtonDark] = Color.FromArgb(248, 222, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBPressedButtonLight] = Color.FromArgb(232, 127, 8);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSelectedButtonDark] = Color.FromArgb(238, 147, 17);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSelectedButtonLight] = Color.FromArgb(251, 230, 148);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSplitterDark] = Color.FromArgb(64, 81, 59);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(120, 142, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(120, 142, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPlacesBarBkgd] = Color.FromArgb(236, 233, 216);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = Color.FromArgb(242, 240, 228);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = Color.FromArgb(96, 128, 88);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = Color.FromArgb(206, 220, 167);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelected] = Color.FromArgb(107, 129, 107);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = Color.FromArgb(107, 129, 107);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrInactiveSelected] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrMouseOver] = Color.FromArgb(107, 129, 107);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = Color.FromArgb(151, 160, 123);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPubWebDocScratchPageBkgd] = Color.FromArgb(193, 198, 176);
            rgbTable[BlueColorTable.KnownColors.msocbvcrSBBdr] = Color.FromArgb(211, 211, 211);
            rgbTable[BlueColorTable.KnownColors.msocbvcrScrollbarBkgd] = Color.FromArgb(249, 249, 247);
            rgbTable[BlueColorTable.KnownColors.msocbvcrToastGradBegin] = Color.FromArgb(237, 242, 212);
            rgbTable[BlueColorTable.KnownColors.msocbvcrToastGradEnd] = Color.FromArgb(191, 206, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBdrInnerDocked] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBdrOuterDocked] = Color.FromArgb(242, 241, 228);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBdrOuterFloating] = Color.FromArgb(116, 134, 94);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBkgd] = Color.FromArgb(243, 242, 231);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdr] = Color.FromArgb(164, 185, 127);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdrDisabled] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBkgd] = Color.FromArgb(197, 212, 159);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBkgdDisabled] = Color.FromArgb(222, 222, 222);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlTextDisabled] = Color.FromArgb(172, 168, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPGroupline] = Color.FromArgb(188, 187, 177);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPInfoTipBkgd] = Color.FromArgb(255, 255, 204);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPInfoTipText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPNavBarBkgnd] = Color.FromArgb(116, 134, 94);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTextDisabled] = Color.FromArgb(172, 168, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleBkgdActive] = Color.FromArgb(216, 227, 182);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleBkgdInactive] = Color.FromArgb(188, 205, 131);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleTextActive] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleTextInactive] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrXLFormulaBarBkgd] = Color.FromArgb(217, 217, 167);
        }

        // Token: 0x06004BB4 RID: 19380 RVA: 0x001138C4 File Offset: 0x001128C4
        internal void InitSilverLunaColors(ref Dictionary<BlueColorTable.KnownColors, Color> rgbTable)
        {
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(173, 174, 193);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBdrOuterFloating] = Color.FromArgb(122, 121, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBkgd] = Color.FromArgb(219, 218, 228);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrMouseDown] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrSelected] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgd] = Color.FromArgb(219, 218, 228);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdLight] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdMouseDown] = Color.FromArgb(254, 128, 62);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = Color.FromArgb(255, 192, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = Color.FromArgb(254, 128, 62);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextDisabled] = Color.FromArgb(141, 141, 141);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextLight] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDockSeparatorLine] = Color.FromArgb(110, 109, 143);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDragHandle] = Color.FromArgb(84, 84, 117);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDragHandleShadow] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDropDownArrow] = Color.FromArgb(224, 223, 227);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin] = Color.FromArgb(215, 215, 229);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = Color.FromArgb(243, 243, 247);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = Color.FromArgb(215, 215, 226);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = Color.FromArgb(118, 116, 151);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = Color.FromArgb(184, 185, 202);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = Color.FromArgb(232, 233, 242);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = Color.FromArgb(172, 170, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseDownBegin] = Color.FromArgb(254, 128, 62);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseDownEnd] = Color.FromArgb(255, 223, 154);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle] = Color.FromArgb(255, 177, 109);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseOverBegin] = Color.FromArgb(255, 255, 222);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseOverEnd] = Color.FromArgb(255, 203, 136);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle] = Color.FromArgb(255, 225, 172);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = Color.FromArgb(186, 185, 206);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsEnd] = Color.FromArgb(118, 116, 146);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = Color.FromArgb(156, 155, 180);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = Color.FromArgb(255, 255, 222);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = Color.FromArgb(255, 193, 118);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = Color.FromArgb(255, 225, 172);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsSelectedBegin] = Color.FromArgb(254, 140, 73);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsSelectedEnd] = Color.FromArgb(255, 221, 152);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = Color.FromArgb(255, 184, 116);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradSelectedBegin] = Color.FromArgb(255, 223, 154);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradSelectedEnd] = Color.FromArgb(255, 166, 76);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradSelectedMiddle] = Color.FromArgb(255, 195, 116);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradVertBegin] = Color.FromArgb(249, 249, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradVertEnd] = Color.FromArgb(147, 145, 176);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradVertMiddle] = Color.FromArgb(225, 226, 236);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBIconDisabledDark] = Color.FromArgb(122, 121, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBIconDisabledLight] = Color.FromArgb(247, 245, 249);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(212, 212, 226);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(212, 212, 226);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBLowColorIconDisabled] = Color.FromArgb(168, 167, 190);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMainMenuBkgd] = Color.FromArgb(198, 200, 215);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = Color.FromArgb(124, 124, 148);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuBkgd] = Color.FromArgb(253, 250, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuCtlText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuCtlTextDisabled] = Color.FromArgb(141, 141, 141);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuIconBkgd] = Color.FromArgb(214, 211, 231);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(185, 187, 200);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(185, 187, 200);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuShadow] = Color.FromArgb(154, 140, 176);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuSplitArrow] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBOptionsButtonShadow] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBShadow] = Color.FromArgb(124, 124, 148);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBSplitterLine] = Color.FromArgb(110, 109, 143);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBSplitterLineLight] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTearOffHandle] = Color.FromArgb(192, 192, 211);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTearOffHandleMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTitleBkgd] = Color.FromArgb(122, 121, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTitleText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDisabledFocuslessHighlightedText] = Color.FromArgb(172, 168, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDisabledHighlightedText] = Color.FromArgb(59, 59, 63);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDlgGroupBoxText] = Color.FromArgb(7, 70, 213);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdr] = Color.FromArgb(118, 116, 146);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDark] = Color.FromArgb(186, 185, 206);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseDown] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLight] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseDown] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseDown] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrSelected] = Color.FromArgb(124, 124, 148);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgd] = Color.FromArgb(212, 212, 226);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdSelected] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseDown] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextSelected] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(212, 212, 226);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(212, 212, 226);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(148, 148, 148);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(148, 148, 148);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(171, 169, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(171, 169, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabTextMouseDown] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(224, 223, 227);
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(224, 223, 227);
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderBdr] = Color.FromArgb(191, 191, 223);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderBkgd] = Color.FromArgb(239, 235, 222);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderCellBdr] = Color.FromArgb(126, 125, 104);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderCellBkgd] = Color.FromArgb(223, 223, 234);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderCellBkgdSelected] = Color.FromArgb(255, 192, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderSeeThroughSelection] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(162, 162, 181);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(162, 162, 181);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentDarkBkgd] = Color.FromArgb(212, 213, 229);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentLightBkgd] = Color.FromArgb(227, 227, 236);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentTextDisabled] = Color.FromArgb(150, 145, 133);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = Color.FromArgb(169, 168, 191);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = Color.FromArgb(208, 208, 223);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(92, 91, 121);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(92, 91, 121);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPHyperlink] = Color.FromArgb(0, 61, 178);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPLightBkgd] = Color.FromArgb(238, 238, 244);
            rgbTable[BlueColorTable.KnownColors.msocbvcrHyperlink] = Color.FromArgb(0, 61, 178);
            rgbTable[BlueColorTable.KnownColors.msocbvcrHyperlinkFollowed] = Color.FromArgb(170, 0, 170);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(124, 124, 148);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(124, 124, 148);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(215, 215, 229);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(215, 215, 229);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradEnd] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(243, 243, 247);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(243, 243, 247);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrListHeaderArrow] = Color.FromArgb(172, 168, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrNetLookBkgnd] = Color.FromArgb(249, 249, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOABBkgd] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOBBkgdBdr] = Color.FromArgb(211, 211, 211);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOBBkgdBdrContrast] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = Color.FromArgb(155, 154, 179);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerActiveBkgd] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerBdr] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerBkgd] = Color.FromArgb(223, 223, 234);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerInactiveBkgd] = Color.FromArgb(177, 176, 195);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerTabBoxBdr] = Color.FromArgb(124, 124, 148);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerTabStopTicks] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = Color.FromArgb(212, 212, 226);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGWorkspaceBkgd] = Color.FromArgb(155, 154, 179);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFlagNone] = Color.FromArgb(239, 239, 244);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFolderbarDark] = Color.FromArgb(110, 109, 143);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFolderbarLight] = Color.FromArgb(168, 167, 191);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFolderbarText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGridlines] = Color.FromArgb(234, 233, 225);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupLine] = Color.FromArgb(165, 164, 189);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupNested] = Color.FromArgb(253, 238, 201);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupShaded] = Color.FromArgb(229, 229, 235);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupText] = Color.FromArgb(112, 111, 145);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKIconBar] = Color.FromArgb(253, 247, 233);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKInfoBarBkgd] = Color.FromArgb(155, 154, 179);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKInfoBarText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKPreviewPaneLabelText] = Color.FromArgb(155, 154, 179);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKTodayIndicatorDark] = Color.FromArgb(187, 85, 3);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKTodayIndicatorLight] = Color.FromArgb(251, 200, 79);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBActionDividerLine] = Color.FromArgb(204, 206, 219);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBButtonDark] = Color.FromArgb(147, 145, 176);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(225, 226, 236);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(225, 226, 236);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBDarkOutline] = Color.FromArgb(124, 124, 148);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBFoldersBackground] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBHoverButtonDark] = Color.FromArgb(247, 190, 87);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBHoverButtonLight] = Color.FromArgb(255, 255, 220);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBLabelText] = Color.FromArgb(50, 69, 105);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBPressedButtonDark] = Color.FromArgb(248, 222, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBPressedButtonLight] = Color.FromArgb(232, 127, 8);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSelectedButtonDark] = Color.FromArgb(238, 147, 17);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSelectedButtonLight] = Color.FromArgb(251, 230, 148);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSplitterDark] = Color.FromArgb(110, 109, 143);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(168, 167, 191);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(168, 167, 191);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPlacesBarBkgd] = Color.FromArgb(224, 223, 227);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = Color.FromArgb(243, 243, 247);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = Color.FromArgb(124, 124, 148);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = Color.FromArgb(215, 215, 229);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelected] = Color.FromArgb(142, 142, 170);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = Color.FromArgb(142, 142, 170);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrInactiveSelected] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrMouseOver] = Color.FromArgb(142, 142, 170);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = Color.FromArgb(155, 154, 179);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPubWebDocScratchPageBkgd] = Color.FromArgb(195, 195, 210);
            rgbTable[BlueColorTable.KnownColors.msocbvcrSBBdr] = Color.FromArgb(236, 234, 218);
            rgbTable[BlueColorTable.KnownColors.msocbvcrScrollbarBkgd] = Color.FromArgb(247, 247, 249);
            rgbTable[BlueColorTable.KnownColors.msocbvcrToastGradBegin] = Color.FromArgb(239, 239, 247);
            rgbTable[BlueColorTable.KnownColors.msocbvcrToastGradEnd] = Color.FromArgb(179, 178, 204);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBdrInnerDocked] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBdrOuterDocked] = Color.FromArgb(243, 243, 247);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBdrOuterFloating] = Color.FromArgb(122, 121, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBkgd] = Color.FromArgb(238, 238, 244);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdr] = Color.FromArgb(165, 172, 178);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdrDisabled] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBkgd] = Color.FromArgb(192, 192, 211);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBkgdDisabled] = Color.FromArgb(222, 222, 222);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlTextDisabled] = Color.FromArgb(172, 168, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPGroupline] = Color.FromArgb(161, 160, 187);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPInfoTipBkgd] = Color.FromArgb(255, 255, 204);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPInfoTipText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPNavBarBkgnd] = Color.FromArgb(122, 121, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTextDisabled] = Color.FromArgb(172, 168, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleBkgdActive] = Color.FromArgb(184, 188, 234);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleBkgdInactive] = Color.FromArgb(198, 198, 217);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleTextActive] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleTextInactive] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrXLFormulaBarBkgd] = Color.FromArgb(215, 215, 229);
        }

        // Token: 0x06004BB5 RID: 19381 RVA: 0x001151B4 File Offset: 0x001141B4
        private void InitRoyaleColors(ref Dictionary<BlueColorTable.KnownColors, Color> rgbTable)
        {
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBkgd] = Color.FromArgb(238, 237, 240);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDragHandle] = Color.FromArgb(189, 188, 191);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBSplitterLine] = Color.FromArgb(193, 193, 196);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTitleBkgd] = Color.FromArgb(167, 166, 170);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTitleText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBdrOuterFloating] = Color.FromArgb(142, 141, 145);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(235, 233, 237);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTearOffHandle] = Color.FromArgb(238, 237, 240);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTearOffHandleMouseOver] = Color.FromArgb(194, 207, 229);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgd] = Color.FromArgb(238, 237, 240);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextDisabled] = Color.FromArgb(176, 175, 179);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(194, 207, 229);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver] = Color.FromArgb(51, 94, 168);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdMouseDown] = Color.FromArgb(153, 175, 212);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrMouseDown] = Color.FromArgb(51, 94, 168);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextMouseDown] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = Color.FromArgb(226, 229, 238);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrSelected] = Color.FromArgb(51, 94, 168);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = Color.FromArgb(51, 94, 168);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = Color.FromArgb(51, 94, 168);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdLight] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextLight] = Color.FromArgb(167, 166, 170);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMainMenuBkgd] = Color.FromArgb(235, 233, 237);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuBkgd] = Color.FromArgb(252, 252, 252);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuCtlText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuCtlTextDisabled] = Color.FromArgb(193, 193, 196);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = Color.FromArgb(134, 133, 136);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuIconBkgd] = Color.FromArgb(238, 237, 240);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(228, 226, 230);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuSplitArrow] = Color.FromArgb(167, 166, 170);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBkgd] = Color.FromArgb(245, 244, 246);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleBkgdActive] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleBkgdInactive] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleTextActive] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleTextInactive] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBdrOuterFloating] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBdrOuterDocked] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdr] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlText] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdrDisabled] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlTextDisabled] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBkgdDisabled] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPGroupline] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrSBBdr] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOBBkgdBdr] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOBBkgdBdrContrast] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOABBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderBdr] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderCellBdr] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderSeeThroughSelection] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderCellBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderCellBkgdSelected] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBSplitterLineLight] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBShadow] = Color.FromArgb(238, 237, 240);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBOptionsButtonShadow] = Color.FromArgb(245, 244, 246);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPNavBarBkgnd] = Color.FromArgb(193, 193, 196);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBdrInnerDocked] = Color.FromArgb(245, 244, 246);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(235, 233, 237);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBIconDisabledLight] = Color.FromArgb(235, 233, 237);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBIconDisabledDark] = Color.FromArgb(167, 166, 170);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBLowColorIconDisabled] = Color.FromArgb(176, 175, 179);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin] = Color.FromArgb(235, 233, 237);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = Color.FromArgb(251, 250, 251);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradVertBegin] = Color.FromArgb(252, 252, 252);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradVertMiddle] = Color.FromArgb(245, 244, 246);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradVertEnd] = Color.FromArgb(235, 233, 237);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = Color.FromArgb(242, 242, 242);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = Color.FromArgb(224, 224, 225);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsEnd] = Color.FromArgb(167, 166, 170);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = Color.FromArgb(252, 252, 252);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = Color.FromArgb(245, 244, 246);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = Color.FromArgb(247, 246, 248);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = Color.FromArgb(241, 240, 242);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = Color.FromArgb(228, 226, 230);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsSelectedBegin] = Color.FromArgb(226, 229, 238);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = Color.FromArgb(226, 229, 238);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsSelectedEnd] = Color.FromArgb(226, 229, 238);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = Color.FromArgb(194, 207, 229);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = Color.FromArgb(194, 207, 229);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = Color.FromArgb(194, 207, 229);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradSelectedBegin] = Color.FromArgb(226, 229, 238);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradSelectedMiddle] = Color.FromArgb(226, 229, 238);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradSelectedEnd] = Color.FromArgb(226, 229, 238);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseOverBegin] = Color.FromArgb(194, 207, 229);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle] = Color.FromArgb(194, 207, 229);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseOverEnd] = Color.FromArgb(194, 207, 229);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseDownBegin] = Color.FromArgb(153, 175, 212);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle] = Color.FromArgb(153, 175, 212);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseDownEnd] = Color.FromArgb(153, 175, 212);
            rgbTable[BlueColorTable.KnownColors.msocbvcrNetLookBkgnd] = Color.FromArgb(235, 233, 237);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuShadow] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDockSeparatorLine] = Color.FromArgb(51, 94, 168);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDropDownArrow] = Color.FromArgb(235, 233, 237);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGridlines] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupText] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupLine] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupShaded] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupNested] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKIconBar] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFlagNone] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFolderbarLight] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFolderbarDark] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFolderbarText] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBButtonDark] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSelectedButtonLight] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSelectedButtonDark] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBHoverButtonLight] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBHoverButtonDark] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBPressedButtonLight] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBPressedButtonDark] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBDarkOutline] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSplitterDark] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBActionDividerLine] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBLabelText] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBFoldersBackground] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKTodayIndicatorLight] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKTodayIndicatorDark] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKInfoBarBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKInfoBarText] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKPreviewPaneLabelText] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrHyperlink] = Color.FromArgb(0, 61, 178);
            rgbTable[BlueColorTable.KnownColors.msocbvcrHyperlinkFollowed] = Color.FromArgb(170, 0, 170);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGWorkspaceBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerActiveBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerInactiveBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerText] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerTabStopTicks] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerBdr] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerTabBoxBdr] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrXLFormulaBarBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDragHandleShadow] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelected] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrInactiveSelected] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrMouseOver] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDlgGroupBoxText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrScrollbarBkgd] = Color.FromArgb(237, 235, 239);
            rgbTable[BlueColorTable.KnownColors.msocbvcrListHeaderArrow] = Color.FromArgb(155, 154, 156);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDisabledHighlightedText] = Color.FromArgb(188, 202, 226);
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(235, 233, 237);
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDisabledFocuslessHighlightedText] = Color.FromArgb(167, 166, 170);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlTextMouseDown] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTextDisabled] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPInfoTipBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPInfoTipText] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabBkgdMouseOver] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabTextMouseOver] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabBkgdMouseDown] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabTextMouseDown] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPLightBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentLightBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentDarkBkgd] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentText] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentTextDisabled] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPHyperlink] = Color.FromArgb(255, 51, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgd] = Color.FromArgb(212, 212, 226);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdr] = Color.FromArgb(118, 116, 146);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLight] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDark] = Color.FromArgb(186, 185, 206);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdSelected] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextSelected] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrSelected] = Color.FromArgb(124, 124, 148);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(193, 210, 238);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(49, 106, 197);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(49, 106, 197);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(49, 106, 197);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(49, 106, 197);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdMouseDown] = Color.FromArgb(154, 183, 228);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseDown] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseDown] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseDown] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseDown] = Color.FromArgb(75, 75, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrToastGradBegin] = Color.FromArgb(246, 244, 236);
            rgbTable[BlueColorTable.KnownColors.msocbvcrToastGradEnd] = Color.FromArgb(179, 178, 204);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(236, 233, 216);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(236, 233, 216);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradEnd] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(172, 168, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPlacesBarBkgd] = Color.FromArgb(224, 223, 227);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = Color.FromArgb(152, 181, 226);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPubWebDocScratchPageBkgd] = Color.FromArgb(193, 210, 238);
        }

        [DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
        private static extern int GetCurrentThemeName(StringBuilder pszThemeFileName,
            int dwMaxNameChars, StringBuilder pszColorBuff, int dwMaxColorChars,
            StringBuilder pszSizeBuff, int cchMaxSizeChars);

        private string GetVisualStyleName()
        {
            if (VisualStyleInformation.IsEnabledByUser)
            {
                StringBuilder stringBuilder = new StringBuilder(512);
                GetCurrentThemeName(stringBuilder, stringBuilder.Capacity, null, 0, null, 0);
                return stringBuilder.ToString();
            }
            return string.Empty;
        }

        // Token: 0x06004BB6 RID: 19382 RVA: 0x0011686C File Offset: 0x0011586C
        internal void InitThemedColors(ref Dictionary<BlueColorTable.KnownColors, Color> rgbTable)
        {
            //string colorScheme = VisualStyleInformation.ColorScheme;
            //string fileName = Path.GetFileName(GetVisualStyleName());
            //bool flag = false;
            //if (string.Equals("luna.msstyles", fileName, StringComparison.OrdinalIgnoreCase))
            //{
            //    if (colorScheme == "NormalColor")
            //    {
            //        this.InitBlueLunaColors(ref rgbTable);
            //        this.usingSystemColors = false;
            //        flag = true;
            //    }
            //    else if (colorScheme == "HomeStead")
            //    {
            //        this.InitOliveLunaColors(ref rgbTable);
            //        this.usingSystemColors = false;
            //        flag = true;
            //    }
            //    else if (colorScheme == "Metallic")
            //    {
            //        this.InitSilverLunaColors(ref rgbTable);
            //        this.usingSystemColors = false;
            //        flag = true;
            //    }
            //}
            //else if (string.Equals("royale.msstyles", fileName, StringComparison.OrdinalIgnoreCase) && (colorScheme == "NormalColor" || colorScheme == "Royale"))
            //{
            //    this.InitRoyaleColors(ref rgbTable);
            //    this.usingSystemColors = false;
            //    flag = true;
            //}
            //if (!flag)
            //{
            //    this.InitSystemColors(ref rgbTable);
            //    this.usingSystemColors = true;
            //}
            //this.InitCommonColors(ref rgbTable);
            this.InitBlueLunaColors(ref rgbTable);
            this.usingSystemColors = false;
        }

        // Token: 0x06004BB7 RID: 19383 RVA: 0x00116948 File Offset: 0x00115948
        internal void InitBlueLunaColors(ref Dictionary<BlueColorTable.KnownColors, Color> rgbTable)
        {
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(196, 205, 218);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(196, 205, 218);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBdrOuterFloating] = Color.FromArgb(42, 102, 201);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBBkgd] = Color.FromArgb(196, 219, 249);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrMouseDown] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrSelected] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgd] = Color.FromArgb(196, 219, 249);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdLight] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdMouseDown] = Color.FromArgb(254, 128, 62);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = Color.FromArgb(255, 192, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = Color.FromArgb(254, 128, 62);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextDisabled] = Color.FromArgb(141, 141, 141);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextLight] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDockSeparatorLine] = Color.FromArgb(0, 53, 145);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDragHandle] = Color.FromArgb(39, 65, 118);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDragHandleShadow] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBDropDownArrow] = Color.FromArgb(236, 233, 216);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin] = Color.FromArgb(158, 190, 245);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = Color.FromArgb(196, 218, 250);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = Color.FromArgb(203, 221, 246);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = Color.FromArgb(114, 155, 215);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = Color.FromArgb(161, 197, 249);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = Color.FromArgb(227, 239, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = Color.FromArgb(123, 164, 224);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseDownBegin] = Color.FromArgb(254, 128, 62);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseDownEnd] = Color.FromArgb(255, 223, 154);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle] = Color.FromArgb(255, 177, 109);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseOverBegin] = Color.FromArgb(255, 255, 222);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseOverEnd] = Color.FromArgb(255, 203, 136);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle] = Color.FromArgb(255, 225, 172);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = Color.FromArgb(127, 177, 250);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsEnd] = Color.FromArgb(0, 53, 145);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = Color.FromArgb(82, 127, 208);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = Color.FromArgb(255, 255, 222);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = Color.FromArgb(255, 193, 118);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = Color.FromArgb(255, 225, 172);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsSelectedBegin] = Color.FromArgb(254, 140, 73);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsSelectedEnd] = Color.FromArgb(255, 221, 152);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = Color.FromArgb(255, 184, 116);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradSelectedBegin] = Color.FromArgb(255, 223, 154);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradSelectedEnd] = Color.FromArgb(255, 166, 76);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradSelectedMiddle] = Color.FromArgb(255, 195, 116);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradVertBegin] = Color.FromArgb(227, 239, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradVertEnd] = Color.FromArgb(123, 164, 224);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBGradVertMiddle] = Color.FromArgb(203, 225, 252);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBIconDisabledDark] = Color.FromArgb(97, 122, 172);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBIconDisabledLight] = Color.FromArgb(233, 236, 242);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(186, 211, 245);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(186, 211, 245);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBLowColorIconDisabled] = Color.FromArgb(109, 150, 208);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMainMenuBkgd] = Color.FromArgb(153, 204, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = Color.FromArgb(0, 45, 150);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuBkgd] = Color.FromArgb(246, 246, 246);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuCtlText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuCtlTextDisabled] = Color.FromArgb(141, 141, 141);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuIconBkgd] = Color.FromArgb(203, 225, 252);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(172, 183, 201);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(172, 183, 201);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuShadow] = Color.FromArgb(95, 130, 234);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBMenuSplitArrow] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBOptionsButtonShadow] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBShadow] = Color.FromArgb(59, 97, 156);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBSplitterLine] = Color.FromArgb(106, 140, 203);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBSplitterLineLight] = Color.FromArgb(241, 249, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTearOffHandle] = Color.FromArgb(169, 199, 240);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTearOffHandleMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTitleBkgd] = Color.FromArgb(42, 102, 201);
            rgbTable[BlueColorTable.KnownColors.msocbvcrCBTitleText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDisabledFocuslessHighlightedText] = Color.FromArgb(172, 168, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDisabledHighlightedText] = Color.FromArgb(187, 206, 236);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDlgGroupBoxText] = Color.FromArgb(0, 70, 213);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdr] = Color.FromArgb(0, 53, 154);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDark] = Color.FromArgb(117, 166, 241);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseDown] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLight] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseDown] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseDown] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(0, 0, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBdrSelected] = Color.FromArgb(59, 97, 156);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgd] = Color.FromArgb(186, 211, 245);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabBkgdSelected] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseDown] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDocTabTextSelected] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(186, 211, 245);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(186, 211, 245);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(94, 94, 94);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(94, 94, 94);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(129, 169, 226);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(129, 169, 226);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabTextMouseDown] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrDWTabTextMouseOver] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(236, 233, 216);
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(236, 233, 216);
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderBdr] = Color.FromArgb(89, 89, 172);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderBkgd] = Color.FromArgb(239, 235, 222);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderCellBdr] = Color.FromArgb(126, 125, 104);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderCellBkgd] = Color.FromArgb(239, 235, 222);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderCellBkgdSelected] = Color.FromArgb(255, 192, 111);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGDHeaderSeeThroughSelection] = Color.FromArgb(191, 191, 223);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(74, 122, 201);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(74, 122, 201);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentDarkBkgd] = Color.FromArgb(185, 208, 241);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentLightBkgd] = Color.FromArgb(221, 236, 254);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupContentTextDisabled] = Color.FromArgb(150, 145, 133);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = Color.FromArgb(101, 143, 224);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = Color.FromArgb(196, 219, 249);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(0, 45, 134);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(0, 45, 134);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPHyperlink] = Color.FromArgb(0, 61, 178);
            rgbTable[BlueColorTable.KnownColors.msocbvcrGSPLightBkgd] = Color.FromArgb(221, 236, 254);
            rgbTable[BlueColorTable.KnownColors.msocbvcrHyperlink] = Color.FromArgb(0, 61, 178);
            rgbTable[BlueColorTable.KnownColors.msocbvcrHyperlinkFollowed] = Color.FromArgb(170, 0, 170);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(59, 97, 156);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(59, 97, 156);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(158, 190, 245);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(158, 190, 245);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradEnd] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(196, 218, 250);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(196, 218, 250);
            rgbTable[BlueColorTable.KnownColors.msocbvcrJotNavUIText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrListHeaderArrow] = Color.FromArgb(172, 168, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrNetLookBkgnd] = Color.FromArgb(227, 239, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOABBkgd] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOBBkgdBdr] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOBBkgdBdrContrast] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = Color.FromArgb(144, 153, 174);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerActiveBkgd] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerBdr] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerBkgd] = Color.FromArgb(216, 231, 252);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerInactiveBkgd] = Color.FromArgb(158, 190, 245);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerTabBoxBdr] = Color.FromArgb(75, 120, 202);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerTabStopTicks] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGRulerText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = Color.FromArgb(186, 211, 245);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOGWorkspaceBkgd] = Color.FromArgb(144, 153, 174);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFlagNone] = Color.FromArgb(242, 240, 228);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFolderbarDark] = Color.FromArgb(0, 53, 145);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFolderbarLight] = Color.FromArgb(89, 135, 214);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKFolderbarText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGridlines] = Color.FromArgb(234, 233, 225);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupLine] = Color.FromArgb(123, 164, 224);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupNested] = Color.FromArgb(253, 238, 201);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupShaded] = Color.FromArgb(190, 218, 251);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKGroupText] = Color.FromArgb(55, 104, 185);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKIconBar] = Color.FromArgb(253, 247, 233);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKInfoBarBkgd] = Color.FromArgb(144, 153, 174);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKInfoBarText] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKPreviewPaneLabelText] = Color.FromArgb(144, 153, 174);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKTodayIndicatorDark] = Color.FromArgb(187, 85, 3);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKTodayIndicatorLight] = Color.FromArgb(251, 200, 79);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBActionDividerLine] = Color.FromArgb(215, 228, 251);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBButtonDark] = Color.FromArgb(123, 164, 224);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(203, 225, 252);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(203, 225, 252);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBDarkOutline] = Color.FromArgb(0, 45, 150);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBFoldersBackground] = Color.FromArgb(255, 255, 255);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBHoverButtonDark] = Color.FromArgb(247, 190, 87);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBHoverButtonLight] = Color.FromArgb(255, 255, 220);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBLabelText] = Color.FromArgb(50, 69, 105);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBPressedButtonDark] = Color.FromArgb(248, 222, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBPressedButtonLight] = Color.FromArgb(232, 127, 8);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSelectedButtonDark] = Color.FromArgb(238, 147, 17);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSelectedButtonLight] = Color.FromArgb(251, 230, 148);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSplitterDark] = Color.FromArgb(0, 53, 145);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(89, 135, 214);
            rgbTable[BlueColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(89, 135, 214);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPlacesBarBkgd] = Color.FromArgb(236, 233, 216);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = Color.FromArgb(195, 218, 249);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = Color.FromArgb(59, 97, 156);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = Color.FromArgb(158, 190, 245);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelected] = Color.FromArgb(61, 108, 192);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = Color.FromArgb(61, 108, 192);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrInactiveSelected] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPPSlideBdrMouseOver] = Color.FromArgb(61, 108, 192);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = Color.FromArgb(144, 153, 174);
            rgbTable[BlueColorTable.KnownColors.msocbvcrPubWebDocScratchPageBkgd] = Color.FromArgb(189, 194, 207);
            rgbTable[BlueColorTable.KnownColors.msocbvcrSBBdr] = Color.FromArgb(211, 211, 211);
            rgbTable[BlueColorTable.KnownColors.msocbvcrScrollbarBkgd] = Color.FromArgb(251, 251, 248);
            rgbTable[BlueColorTable.KnownColors.msocbvcrToastGradBegin] = Color.FromArgb(220, 236, 254);
            rgbTable[BlueColorTable.KnownColors.msocbvcrToastGradEnd] = Color.FromArgb(167, 197, 238);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBdrInnerDocked] = Color.FromArgb(185, 212, 249);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBdrOuterDocked] = Color.FromArgb(196, 218, 250);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBdrOuterFloating] = Color.FromArgb(42, 102, 201);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPBkgd] = Color.FromArgb(221, 236, 254);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdr] = Color.FromArgb(127, 157, 185);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBdrDisabled] = Color.FromArgb(128, 128, 128);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBkgd] = Color.FromArgb(169, 199, 240);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlBkgdDisabled] = Color.FromArgb(222, 222, 222);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlTextDisabled] = Color.FromArgb(172, 168, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPGroupline] = Color.FromArgb(123, 164, 224);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPInfoTipBkgd] = Color.FromArgb(255, 255, 204);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPInfoTipText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPNavBarBkgnd] = Color.FromArgb(74, 122, 201);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTextDisabled] = Color.FromArgb(172, 168, 153);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleBkgdActive] = Color.FromArgb(123, 164, 224);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleBkgdInactive] = Color.FromArgb(148, 187, 239);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleTextActive] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrWPTitleTextInactive] = Color.FromArgb(0, 0, 0);
            rgbTable[BlueColorTable.KnownColors.msocbvcrXLFormulaBarBkgd] = Color.FromArgb(158, 190, 245);
        }

        // Token: 0x040030B1 RID: 12465
        private const string oliveColorScheme = "HomeStead";

        // Token: 0x040030B2 RID: 12466
        private const string normalColorScheme = "NormalColor";

        // Token: 0x040030B3 RID: 12467
        private const string silverColorScheme = "Metallic";

        // Token: 0x040030B4 RID: 12468
        private const string royaleColorScheme = "Royale";

        // Token: 0x040030B5 RID: 12469
        private const string lunaFileName = "luna.msstyles";

        // Token: 0x040030B6 RID: 12470
        private const string royaleFileName = "royale.msstyles";

        // Token: 0x040030B7 RID: 12471
        private Dictionary<BlueColorTable.KnownColors, Color> professionalRGB;

        // Token: 0x040030B8 RID: 12472
        private bool usingSystemColors;

        // Token: 0x040030B9 RID: 12473
        private bool useSystemColors;

        // Token: 0x040030BA RID: 12474
        private string lastKnownColorScheme = string.Empty;

        // Token: 0x040030BB RID: 12475
        private object colorFreshnessKey;

        // Token: 0x02000597 RID: 1431
        internal enum KnownColors
        {
            // Token: 0x040030BD RID: 12477
            msocbvcrCBBdrOuterDocked,
            // Token: 0x040030BE RID: 12478
            msocbvcrCBBdrOuterFloating,
            // Token: 0x040030BF RID: 12479
            msocbvcrCBBkgd,
            // Token: 0x040030C0 RID: 12480
            msocbvcrCBCtlBdrMouseDown,
            // Token: 0x040030C1 RID: 12481
            msocbvcrCBCtlBdrMouseOver,
            // Token: 0x040030C2 RID: 12482
            msocbvcrCBCtlBdrSelected,
            // Token: 0x040030C3 RID: 12483
            msocbvcrCBCtlBdrSelectedMouseOver,
            // Token: 0x040030C4 RID: 12484
            msocbvcrCBCtlBkgd,
            // Token: 0x040030C5 RID: 12485
            msocbvcrCBCtlBkgdLight,
            // Token: 0x040030C6 RID: 12486
            msocbvcrCBCtlBkgdMouseDown,
            // Token: 0x040030C7 RID: 12487
            msocbvcrCBCtlBkgdMouseOver,
            // Token: 0x040030C8 RID: 12488
            msocbvcrCBCtlBkgdSelected,
            // Token: 0x040030C9 RID: 12489
            msocbvcrCBCtlBkgdSelectedMouseOver,
            // Token: 0x040030CA RID: 12490
            msocbvcrCBCtlText,
            // Token: 0x040030CB RID: 12491
            msocbvcrCBCtlTextDisabled,
            // Token: 0x040030CC RID: 12492
            msocbvcrCBCtlTextLight,
            // Token: 0x040030CD RID: 12493
            msocbvcrCBCtlTextMouseDown,
            // Token: 0x040030CE RID: 12494
            msocbvcrCBCtlTextMouseOver,
            // Token: 0x040030CF RID: 12495
            msocbvcrCBDockSeparatorLine,
            // Token: 0x040030D0 RID: 12496
            msocbvcrCBDragHandle,
            // Token: 0x040030D1 RID: 12497
            msocbvcrCBDragHandleShadow,
            // Token: 0x040030D2 RID: 12498
            msocbvcrCBDropDownArrow,
            // Token: 0x040030D3 RID: 12499
            msocbvcrCBGradMainMenuHorzBegin,
            // Token: 0x040030D4 RID: 12500
            msocbvcrCBGradMainMenuHorzEnd,
            // Token: 0x040030D5 RID: 12501
            msocbvcrCBGradMenuIconBkgdDroppedBegin,
            // Token: 0x040030D6 RID: 12502
            msocbvcrCBGradMenuIconBkgdDroppedEnd,
            // Token: 0x040030D7 RID: 12503
            msocbvcrCBGradMenuIconBkgdDroppedMiddle,
            // Token: 0x040030D8 RID: 12504
            msocbvcrCBGradMenuTitleBkgdBegin,
            // Token: 0x040030D9 RID: 12505
            msocbvcrCBGradMenuTitleBkgdEnd,
            // Token: 0x040030DA RID: 12506
            msocbvcrCBGradMouseDownBegin,
            // Token: 0x040030DB RID: 12507
            msocbvcrCBGradMouseDownEnd,
            // Token: 0x040030DC RID: 12508
            msocbvcrCBGradMouseDownMiddle,
            // Token: 0x040030DD RID: 12509
            msocbvcrCBGradMouseOverBegin,
            // Token: 0x040030DE RID: 12510
            msocbvcrCBGradMouseOverEnd,
            // Token: 0x040030DF RID: 12511
            msocbvcrCBGradMouseOverMiddle,
            // Token: 0x040030E0 RID: 12512
            msocbvcrCBGradOptionsBegin,
            // Token: 0x040030E1 RID: 12513
            msocbvcrCBGradOptionsEnd,
            // Token: 0x040030E2 RID: 12514
            msocbvcrCBGradOptionsMiddle,
            // Token: 0x040030E3 RID: 12515
            msocbvcrCBGradOptionsMouseOverBegin,
            // Token: 0x040030E4 RID: 12516
            msocbvcrCBGradOptionsMouseOverEnd,
            // Token: 0x040030E5 RID: 12517
            msocbvcrCBGradOptionsMouseOverMiddle,
            // Token: 0x040030E6 RID: 12518
            msocbvcrCBGradOptionsSelectedBegin,
            // Token: 0x040030E7 RID: 12519
            msocbvcrCBGradOptionsSelectedEnd,
            // Token: 0x040030E8 RID: 12520
            msocbvcrCBGradOptionsSelectedMiddle,
            // Token: 0x040030E9 RID: 12521
            msocbvcrCBGradSelectedBegin,
            // Token: 0x040030EA RID: 12522
            msocbvcrCBGradSelectedEnd,
            // Token: 0x040030EB RID: 12523
            msocbvcrCBGradSelectedMiddle,
            // Token: 0x040030EC RID: 12524
            msocbvcrCBGradVertBegin,
            // Token: 0x040030ED RID: 12525
            msocbvcrCBGradVertEnd,
            // Token: 0x040030EE RID: 12526
            msocbvcrCBGradVertMiddle,
            // Token: 0x040030EF RID: 12527
            msocbvcrCBIconDisabledDark,
            // Token: 0x040030F0 RID: 12528
            msocbvcrCBIconDisabledLight,
            // Token: 0x040030F1 RID: 12529
            msocbvcrCBLabelBkgnd,
            // Token: 0x040030F2 RID: 12530
            msocbvcrCBLowColorIconDisabled,
            // Token: 0x040030F3 RID: 12531
            msocbvcrCBMainMenuBkgd,
            // Token: 0x040030F4 RID: 12532
            msocbvcrCBMenuBdrOuter,
            // Token: 0x040030F5 RID: 12533
            msocbvcrCBMenuBkgd,
            // Token: 0x040030F6 RID: 12534
            msocbvcrCBMenuCtlText,
            // Token: 0x040030F7 RID: 12535
            msocbvcrCBMenuCtlTextDisabled,
            // Token: 0x040030F8 RID: 12536
            msocbvcrCBMenuIconBkgd,
            // Token: 0x040030F9 RID: 12537
            msocbvcrCBMenuIconBkgdDropped,
            // Token: 0x040030FA RID: 12538
            msocbvcrCBMenuShadow,
            // Token: 0x040030FB RID: 12539
            msocbvcrCBMenuSplitArrow,
            // Token: 0x040030FC RID: 12540
            msocbvcrCBOptionsButtonShadow,
            // Token: 0x040030FD RID: 12541
            msocbvcrCBShadow,
            // Token: 0x040030FE RID: 12542
            msocbvcrCBSplitterLine,
            // Token: 0x040030FF RID: 12543
            msocbvcrCBSplitterLineLight,
            // Token: 0x04003100 RID: 12544
            msocbvcrCBTearOffHandle,
            // Token: 0x04003101 RID: 12545
            msocbvcrCBTearOffHandleMouseOver,
            // Token: 0x04003102 RID: 12546
            msocbvcrCBTitleBkgd,
            // Token: 0x04003103 RID: 12547
            msocbvcrCBTitleText,
            // Token: 0x04003104 RID: 12548
            msocbvcrDisabledFocuslessHighlightedText,
            // Token: 0x04003105 RID: 12549
            msocbvcrDisabledHighlightedText,
            // Token: 0x04003106 RID: 12550
            msocbvcrDlgGroupBoxText,
            // Token: 0x04003107 RID: 12551
            msocbvcrDocTabBdr,
            // Token: 0x04003108 RID: 12552
            msocbvcrDocTabBdrDark,
            // Token: 0x04003109 RID: 12553
            msocbvcrDocTabBdrDarkMouseDown,
            // Token: 0x0400310A RID: 12554
            msocbvcrDocTabBdrDarkMouseOver,
            // Token: 0x0400310B RID: 12555
            msocbvcrDocTabBdrLight,
            // Token: 0x0400310C RID: 12556
            msocbvcrDocTabBdrLightMouseDown,
            // Token: 0x0400310D RID: 12557
            msocbvcrDocTabBdrLightMouseOver,
            // Token: 0x0400310E RID: 12558
            msocbvcrDocTabBdrMouseDown,
            // Token: 0x0400310F RID: 12559
            msocbvcrDocTabBdrMouseOver,
            // Token: 0x04003110 RID: 12560
            msocbvcrDocTabBdrSelected,
            // Token: 0x04003111 RID: 12561
            msocbvcrDocTabBkgd,
            // Token: 0x04003112 RID: 12562
            msocbvcrDocTabBkgdMouseDown,
            // Token: 0x04003113 RID: 12563
            msocbvcrDocTabBkgdMouseOver,
            // Token: 0x04003114 RID: 12564
            msocbvcrDocTabBkgdSelected,
            // Token: 0x04003115 RID: 12565
            msocbvcrDocTabText,
            // Token: 0x04003116 RID: 12566
            msocbvcrDocTabTextMouseDown,
            // Token: 0x04003117 RID: 12567
            msocbvcrDocTabTextMouseOver,
            // Token: 0x04003118 RID: 12568
            msocbvcrDocTabTextSelected,
            // Token: 0x04003119 RID: 12569
            msocbvcrDWActiveTabBkgd,
            // Token: 0x0400311A RID: 12570
            msocbvcrDWActiveTabText,
            // Token: 0x0400311B RID: 12571
            msocbvcrDWActiveTabTextDisabled,
            // Token: 0x0400311C RID: 12572
            msocbvcrDWInactiveTabBkgd,
            // Token: 0x0400311D RID: 12573
            msocbvcrDWInactiveTabText,
            // Token: 0x0400311E RID: 12574
            msocbvcrDWTabBkgdMouseDown,
            // Token: 0x0400311F RID: 12575
            msocbvcrDWTabBkgdMouseOver,
            // Token: 0x04003120 RID: 12576
            msocbvcrDWTabTextMouseDown,
            // Token: 0x04003121 RID: 12577
            msocbvcrDWTabTextMouseOver,
            // Token: 0x04003122 RID: 12578
            msocbvcrFocuslessHighlightedBkgd,
            // Token: 0x04003123 RID: 12579
            msocbvcrFocuslessHighlightedText,
            // Token: 0x04003124 RID: 12580
            msocbvcrGDHeaderBdr,
            // Token: 0x04003125 RID: 12581
            msocbvcrGDHeaderBkgd,
            // Token: 0x04003126 RID: 12582
            msocbvcrGDHeaderCellBdr,
            // Token: 0x04003127 RID: 12583
            msocbvcrGDHeaderCellBkgd,
            // Token: 0x04003128 RID: 12584
            msocbvcrGDHeaderCellBkgdSelected,
            // Token: 0x04003129 RID: 12585
            msocbvcrGDHeaderSeeThroughSelection,
            // Token: 0x0400312A RID: 12586
            msocbvcrGSPDarkBkgd,
            // Token: 0x0400312B RID: 12587
            msocbvcrGSPGroupContentDarkBkgd,
            // Token: 0x0400312C RID: 12588
            msocbvcrGSPGroupContentLightBkgd,
            // Token: 0x0400312D RID: 12589
            msocbvcrGSPGroupContentText,
            // Token: 0x0400312E RID: 12590
            msocbvcrGSPGroupContentTextDisabled,
            // Token: 0x0400312F RID: 12591
            msocbvcrGSPGroupHeaderDarkBkgd,
            // Token: 0x04003130 RID: 12592
            msocbvcrGSPGroupHeaderLightBkgd,
            // Token: 0x04003131 RID: 12593
            msocbvcrGSPGroupHeaderText,
            // Token: 0x04003132 RID: 12594
            msocbvcrGSPGroupline,
            // Token: 0x04003133 RID: 12595
            msocbvcrGSPHyperlink,
            // Token: 0x04003134 RID: 12596
            msocbvcrGSPLightBkgd,
            // Token: 0x04003135 RID: 12597
            msocbvcrHyperlink,
            // Token: 0x04003136 RID: 12598
            msocbvcrHyperlinkFollowed,
            // Token: 0x04003137 RID: 12599
            msocbvcrJotNavUIBdr,
            // Token: 0x04003138 RID: 12600
            msocbvcrJotNavUIGradBegin,
            // Token: 0x04003139 RID: 12601
            msocbvcrJotNavUIGradEnd,
            // Token: 0x0400313A RID: 12602
            msocbvcrJotNavUIGradMiddle,
            // Token: 0x0400313B RID: 12603
            msocbvcrJotNavUIText,
            // Token: 0x0400313C RID: 12604
            msocbvcrListHeaderArrow,
            // Token: 0x0400313D RID: 12605
            msocbvcrNetLookBkgnd,
            // Token: 0x0400313E RID: 12606
            msocbvcrOABBkgd,
            // Token: 0x0400313F RID: 12607
            msocbvcrOBBkgdBdr,
            // Token: 0x04003140 RID: 12608
            msocbvcrOBBkgdBdrContrast,
            // Token: 0x04003141 RID: 12609
            msocbvcrOGMDIParentWorkspaceBkgd,
            // Token: 0x04003142 RID: 12610
            msocbvcrOGRulerActiveBkgd,
            // Token: 0x04003143 RID: 12611
            msocbvcrOGRulerBdr,
            // Token: 0x04003144 RID: 12612
            msocbvcrOGRulerBkgd,
            // Token: 0x04003145 RID: 12613
            msocbvcrOGRulerInactiveBkgd,
            // Token: 0x04003146 RID: 12614
            msocbvcrOGRulerTabBoxBdr,
            // Token: 0x04003147 RID: 12615
            msocbvcrOGRulerTabBoxBdrHighlight,
            // Token: 0x04003148 RID: 12616
            msocbvcrOGRulerTabStopTicks,
            // Token: 0x04003149 RID: 12617
            msocbvcrOGRulerText,
            // Token: 0x0400314A RID: 12618
            msocbvcrOGTaskPaneGroupBoxHeaderBkgd,
            // Token: 0x0400314B RID: 12619
            msocbvcrOGWorkspaceBkgd,
            // Token: 0x0400314C RID: 12620
            msocbvcrOLKFlagNone,
            // Token: 0x0400314D RID: 12621
            msocbvcrOLKFolderbarDark,
            // Token: 0x0400314E RID: 12622
            msocbvcrOLKFolderbarLight,
            // Token: 0x0400314F RID: 12623
            msocbvcrOLKFolderbarText,
            // Token: 0x04003150 RID: 12624
            msocbvcrOLKGridlines,
            // Token: 0x04003151 RID: 12625
            msocbvcrOLKGroupLine,
            // Token: 0x04003152 RID: 12626
            msocbvcrOLKGroupNested,
            // Token: 0x04003153 RID: 12627
            msocbvcrOLKGroupShaded,
            // Token: 0x04003154 RID: 12628
            msocbvcrOLKGroupText,
            // Token: 0x04003155 RID: 12629
            msocbvcrOLKIconBar,
            // Token: 0x04003156 RID: 12630
            msocbvcrOLKInfoBarBkgd,
            // Token: 0x04003157 RID: 12631
            msocbvcrOLKInfoBarText,
            // Token: 0x04003158 RID: 12632
            msocbvcrOLKPreviewPaneLabelText,
            // Token: 0x04003159 RID: 12633
            msocbvcrOLKTodayIndicatorDark,
            // Token: 0x0400315A RID: 12634
            msocbvcrOLKTodayIndicatorLight,
            // Token: 0x0400315B RID: 12635
            msocbvcrOLKWBActionDividerLine,
            // Token: 0x0400315C RID: 12636
            msocbvcrOLKWBButtonDark,
            // Token: 0x0400315D RID: 12637
            msocbvcrOLKWBButtonLight,
            // Token: 0x0400315E RID: 12638
            msocbvcrOLKWBDarkOutline,
            // Token: 0x0400315F RID: 12639
            msocbvcrOLKWBFoldersBackground,
            // Token: 0x04003160 RID: 12640
            msocbvcrOLKWBHoverButtonDark,
            // Token: 0x04003161 RID: 12641
            msocbvcrOLKWBHoverButtonLight,
            // Token: 0x04003162 RID: 12642
            msocbvcrOLKWBLabelText,
            // Token: 0x04003163 RID: 12643
            msocbvcrOLKWBPressedButtonDark,
            // Token: 0x04003164 RID: 12644
            msocbvcrOLKWBPressedButtonLight,
            // Token: 0x04003165 RID: 12645
            msocbvcrOLKWBSelectedButtonDark,
            // Token: 0x04003166 RID: 12646
            msocbvcrOLKWBSelectedButtonLight,
            // Token: 0x04003167 RID: 12647
            msocbvcrOLKWBSplitterDark,
            // Token: 0x04003168 RID: 12648
            msocbvcrOLKWBSplitterLight,
            // Token: 0x04003169 RID: 12649
            msocbvcrPlacesBarBkgd,
            // Token: 0x0400316A RID: 12650
            msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd,
            // Token: 0x0400316B RID: 12651
            msocbvcrPPOutlineThumbnailsPaneTabBdr,
            // Token: 0x0400316C RID: 12652
            msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd,
            // Token: 0x0400316D RID: 12653
            msocbvcrPPOutlineThumbnailsPaneTabText,
            // Token: 0x0400316E RID: 12654
            msocbvcrPPSlideBdrActiveSelected,
            // Token: 0x0400316F RID: 12655
            msocbvcrPPSlideBdrActiveSelectedMouseOver,
            // Token: 0x04003170 RID: 12656
            msocbvcrPPSlideBdrInactiveSelected,
            // Token: 0x04003171 RID: 12657
            msocbvcrPPSlideBdrMouseOver,
            // Token: 0x04003172 RID: 12658
            msocbvcrPubPrintDocScratchPageBkgd,
            // Token: 0x04003173 RID: 12659
            msocbvcrPubWebDocScratchPageBkgd,
            // Token: 0x04003174 RID: 12660
            msocbvcrSBBdr,
            // Token: 0x04003175 RID: 12661
            msocbvcrScrollbarBkgd,
            // Token: 0x04003176 RID: 12662
            msocbvcrToastGradBegin,
            // Token: 0x04003177 RID: 12663
            msocbvcrToastGradEnd,
            // Token: 0x04003178 RID: 12664
            msocbvcrWPBdrInnerDocked,
            // Token: 0x04003179 RID: 12665
            msocbvcrWPBdrOuterDocked,
            // Token: 0x0400317A RID: 12666
            msocbvcrWPBdrOuterFloating,
            // Token: 0x0400317B RID: 12667
            msocbvcrWPBkgd,
            // Token: 0x0400317C RID: 12668
            msocbvcrWPCtlBdr,
            // Token: 0x0400317D RID: 12669
            msocbvcrWPCtlBdrDefault,
            // Token: 0x0400317E RID: 12670
            msocbvcrWPCtlBdrDisabled,
            // Token: 0x0400317F RID: 12671
            msocbvcrWPCtlBkgd,
            // Token: 0x04003180 RID: 12672
            msocbvcrWPCtlBkgdDisabled,
            // Token: 0x04003181 RID: 12673
            msocbvcrWPCtlText,
            // Token: 0x04003182 RID: 12674
            msocbvcrWPCtlTextDisabled,
            // Token: 0x04003183 RID: 12675
            msocbvcrWPCtlTextMouseDown,
            // Token: 0x04003184 RID: 12676
            msocbvcrWPGroupline,
            // Token: 0x04003185 RID: 12677
            msocbvcrWPInfoTipBkgd,
            // Token: 0x04003186 RID: 12678
            msocbvcrWPInfoTipText,
            // Token: 0x04003187 RID: 12679
            msocbvcrWPNavBarBkgnd,
            // Token: 0x04003188 RID: 12680
            msocbvcrWPText,
            // Token: 0x04003189 RID: 12681
            msocbvcrWPTextDisabled,
            // Token: 0x0400318A RID: 12682
            msocbvcrWPTitleBkgdActive,
            // Token: 0x0400318B RID: 12683
            msocbvcrWPTitleBkgdInactive,
            // Token: 0x0400318C RID: 12684
            msocbvcrWPTitleTextActive,
            // Token: 0x0400318D RID: 12685
            msocbvcrWPTitleTextInactive,
            // Token: 0x0400318E RID: 12686
            msocbvcrXLFormulaBarBkgd,
            // Token: 0x0400318F RID: 12687
            ButtonSelectedHighlight,
            // Token: 0x04003190 RID: 12688
            ButtonPressedHighlight,
            // Token: 0x04003191 RID: 12689
            ButtonCheckedHighlight,
            // Token: 0x04003192 RID: 12690
            lastKnownColor = 212
        }
    }
}
