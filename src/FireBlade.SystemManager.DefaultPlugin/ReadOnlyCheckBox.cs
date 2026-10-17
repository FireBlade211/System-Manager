using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace FireBlade.SystemManager.DefaultPlugin
{
    public class ReadOnlyCheckBox : Control
    {
        public bool Checked
        {
            get;
            set
            {
                field = value;
                Invalidate();
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (VisualStyleInformation.IsSupportedByOS && VisualStyleInformation.IsEnabledByUser)
            {
                var state = Checked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
                var size = CheckBoxRenderer.GetGlyphSize(e.Graphics, state);

                CheckBoxRenderer.DrawParentBackground(e.Graphics, ClientRectangle, this);
                CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(8, (Height - size.Height) / 2),
                    state);

                TextRenderer.DrawText(e.Graphics, Text, Font, new Point(size.Width + 12, (Height - size.Height) / 2), ForeColor);
            }
            else
            {
                var state = Checked ? ButtonState.Checked : ButtonState.Normal;

                ControlPaint.DrawCheckBox(e.Graphics, new Rectangle(8, 8, 8, 8), state);

                using var brush = new SolidBrush(ForeColor);
                e.Graphics.DrawString(Text, Font, brush, new PointF(22, 8));
            }
        }
    }
}
