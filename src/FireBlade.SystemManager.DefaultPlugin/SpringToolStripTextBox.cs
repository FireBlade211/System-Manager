using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager.DefaultPlugin
{
    public class SpringToolStripTextBox : ToolStripTextBox
    {
        public override Size GetPreferredSize(Size constrainingSize)
        {
            Size size = base.GetPreferredSize(constrainingSize);

            if (Owner is not { } owner)
                return size;

            int availableWidth = owner.DisplayRectangle.Width;

            foreach (ToolStripItem item in owner.Items)
            {
                if (item != this && item.Available)
                    availableWidth -= item.Width + item.Margin.Horizontal;
            }

            availableWidth -= Margin.Horizontal;

            return new Size(Math.Max(size.Width, availableWidth), size.Height);
        }
    }
}
