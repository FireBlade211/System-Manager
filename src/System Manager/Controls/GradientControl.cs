using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager.Controls
{
    public class GradientControl : Control
    {
        private float _x1 = 0.0f;
        private float _y1 = 0.0f;
        private float _x2 = 1.0f;
        private float _y2 = 0.0f;

        private Color _cl1 = Color.White;
        private Color _cl2 = Color.Black;

        public float X1
        {
            get => _x1;
            set
            {
                if (value < 0.0 || value > 1.0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _x1 = value;
                Refresh();
            }
        }

        public float Y1
        {
            get => _y1;
            set
            {
                if (value < 0.0 || value > 1.0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _y1 = value;
                Refresh();
            }
        }

        public float X2
        {
            get => _x2;
            set
            {
                if (value < 0.0 || value > 1.0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _x2 = value;
                Refresh();
            }
        }

        public float Y2
        {
            get => _y2;
            set
            {
                if (value < 0.0 || value > 1.0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _y2 = value;
                Refresh();
            }
        }

        public Color Color1
        {
            get => _cl1;
            set
            {
                _cl1 = value;
                Refresh();
            }
        }

        public Color Color2
        {
            get => _cl2;
            set
            {
                _cl2 = value;
                Refresh();
            }
        }

        public GradientControl()
        {
            Text = string.Empty;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            DoubleBuffered = true;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Width <= 0 || Height <= 0)
                return;

            Point p1 = new Point(
                (int)(X1 * Width),
                (int)(Y1 * Height));

            Point p2 = new Point(
                (int)(X2 * Width),
                (int)(Y2 * Height));

            if (p1 == p2)
                return;

            using LinearGradientBrush brush =
                new LinearGradientBrush(p1, p2, Color1, Color2);

            e.Graphics.FillRectangle(brush, ClientRectangle);

            using Brush foreBrush = new SolidBrush(ForeColor);
            e.Graphics.DrawString(Text, Font, foreBrush, new PointF(16, 16));
        }
    }
}
