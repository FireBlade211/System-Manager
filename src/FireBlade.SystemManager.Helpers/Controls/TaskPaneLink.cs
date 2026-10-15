using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager.Helpers.Controls
{
    /// <summary>
    /// Represents a link in a <see cref="TaskPaneGroup"/>.
    /// </summary>
    public class TaskPaneLink
    {
        internal TaskPane _pane;

        /// <summary>
        /// Gets the <see cref="Controls.TaskPane"/> that owns this link.
        /// </summary>
        [Description("Gets the TaskPane that owns this link.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public TaskPane TaskPane => _pane;

        /// <summary>
        /// Gets or sets the text of the <see cref="TaskPaneLink"/>.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the text of the TaskPaneLink.")]
        [DefaultValue("")]
        public string Text
        {
            get;
            set
            {
                field = value;
                _pane?.Invalidate();
            }
        } = string.Empty;

        /// <summary>
        /// Gets or sets the optional icon next to the <see cref="TaskPaneLink"/>.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the optional icon next to the TaskPaneLink.")]
        [DefaultValue(null)]
        public Image Icon
        {
            get;
            set
            {
                field = value;
                _pane?.Invalidate();
            }
        }

        internal bool _hover;
        internal Rectangle _rect = Rectangle.Empty;

        protected internal virtual bool HitTest(Point pt /*, ref bool lockCursorChange */)
        {
            if (!Visible)
            {
                _hover = false;
                _pane.Cursor = Cursors.Default;

                return false;
            }

            if (!_rect.IsEmpty && _rect.Contains(pt))
                _hover = true;
            else
                _hover = false;

            //lockCursorChange = _hover;
            //_pane.Cursor = _hover ? Cursors.Hand : Cursors.Default;

            //Debug.WriteLine($"PX: {pt.X}, PY: {pt.Y}, RX: {_rect.X}, RY: {_rect.Y}, RW: {_rect.Width}, RH: {_rect.Height}, H: {_hover}");
            //_hoverPt = pt;

            return _hover;
        }

        protected internal virtual SizeF Measure(Graphics g)
        {
            if (!Visible)
                return SizeF.Empty;

            Font font = _pane.LinkFont;

            if (_hover && !_pane.LinkFont.Underline)
                font = new Font(_pane.LinkFont, _pane.LinkFont.Style | FontStyle.Underline);

            var size = g.MeasureString(Text, font);

            if (_hover && !_pane.LinkFont.Underline)
                font.Dispose();

            return size;
        }

        protected internal virtual SizeF Measure(Graphics g, int width)
        {
            if (!Visible)
                return SizeF.Empty;

            Font font = _pane.LinkFont;

            if (_hover && !_pane.LinkFont.Underline)
                font = new Font(_pane.LinkFont, _pane.LinkFont.Style | FontStyle.Underline);

            try
            {
                return g.MeasureString(
                    Text,
                    font,
                    Math.Max(1, width - 4));
            }
            finally
            {
                if (!ReferenceEquals(font, _pane.LinkFont))
                    font.Dispose();
            }
        }

        private Point _hoverPt;

        /// <summary>
        /// Gets or sets a user-defined value.
        /// </summary>
        [Description("Gets or sets a user-defined value.")]
        [DefaultValue(null)]
        [TypeConverter(typeof(StringConverter))]
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the link is visible.
        /// </summary>
        [Description("Gets or sets a value indicating whether the link is visible.")]
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool Visible
        {
            get;
            set
            {
                field = value;
                _pane?.Invalidate();
            }
        } = true;

        protected internal virtual void Paint(Graphics g, ColorScheme scheme, int x, int w, double opacity, ref int y)
        {
            if (_pane == null || !Visible) return;

            Font font = _pane.LinkFont;

            if (_hover && !_pane.LinkFont.Underline)
                font = new Font(_pane.LinkFont, _pane.LinkFont.Style | FontStyle.Underline);

            try
            {
                SizeF size = Measure(g, w);

                int height = (int)Math.Round(size.Height);
                int width = (int)Math.Round(size.Width) + 6;

                if (width > w)
                    width = w;

                _rect = new Rectangle(x, y, width, height);

                Rectangle paintRect = _rect;

                paintRect.Offset(0, 4);

                if (Icon != null)
                {
                    g.DrawImage(Icon, new Rectangle(x + 2, y + 2, 16, 16));
                    x += 16 + 4;

                    paintRect.X += 20;
                    //paintRect.Width -= 20;
                }

                //g.FillRectangle(Brushes.Red, paintRect);
                //g.FillRectangle(Brushes.LightGreen, _rect);
                //g.FillRectangle(Brushes.Blue, new Rectangle(_hoverPt.X, _hoverPt.Y, 4, 4));

                Brush textBrush = null;

                if (scheme == ColorScheme.Blue)
                    textBrush = new SolidBrush(Color.FromArgb((int)Math.Round(255.0d * opacity),
                        _hover ? Color.RoyalBlue : SystemColors.Highlight));
                else if (scheme == ColorScheme.Olive)
                    textBrush = new SolidBrush(_hover ? Color.FromArgb((int)Math.Round(255.0d * opacity), 114, 146, 29) :
                        Color.FromArgb((int)Math.Round(255 * opacity),86, 102, 45));
                else if (scheme == ColorScheme.Silver)
                    textBrush = new SolidBrush(_hover ? Color.FromArgb((int)Math.Round(255.0d * opacity),
                        126, 124, 124) : Color.FromArgb((int)Math.Round(255 * opacity), 63, 61, 61));
                else
                    textBrush = new SolidBrush(Color.FromArgb((int)Math.Round(255.0d * opacity), SystemColors.ControlText));

                try
                {
                    g.DrawString(Text, font, textBrush, paintRect);
                }
                finally
                {
                    textBrush.Dispose();
                }

                y += height;
            }
            finally
            {
                if (_hover && !_pane.Font.Underline)
                    font.Dispose();
            }
        }

        public override string ToString() => Text;
    }
}
