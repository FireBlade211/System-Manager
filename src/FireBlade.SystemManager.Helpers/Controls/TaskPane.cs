using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FireBlade.SystemManager.Helpers.Controls
{
    /// <summary>
    /// Represents a task pane.
    /// </summary>
    [DefaultEvent("LinkClicked")]
    public class TaskPane : ScrollableControl
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use the default back color for the color scheme.
        /// </summary>
        [DefaultValue(true)]
        [Category("Appearance")]
        [Description("Gets or sets a value indicating whether to use the default back color for the color scheme.")]
        public bool UseDefaultBackColor
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    Invalidate();
                }
            }
        } = true;

        [DefaultValue(DockStyle.Right)]
        public override DockStyle Dock { get => base.Dock; set => base.Dock = value; }

        
        protected override Size DefaultSize => new Size(200, base.DefaultSize.Height);

        /// <summary>
        /// Gets or sets the color scheme of the task pane.
        /// </summary>
        [DefaultValue(ColorScheme.Blue)]
        [Description("Gets or sets the color scheme of the task pane.")]
        [Category("Appearance")]
        public ColorScheme ColorScheme
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    Invalidate();
                }
            }
        } = ColorScheme.Blue;

        public override void ResetFont()
        {
            Font = new Font("Tahoma", 8, FontStyle.Bold, GraphicsUnit.Point);
        }

        private Font _linkFont = new Font("Tahoma", 8.0f, GraphicsUnit.Point);

        /// <summary>
        /// Gets or sets the font used for links.
        /// </summary>
        [Description("Gets or sets the font used for links.")]
        [Category("Appearance")]
        public Font LinkFont
        {
            get => _linkFont;
            set
            {
                _linkFont = value;
                Invalidate();
            }
        }

        private bool ShouldSerializeLinkFont() => true;
        private void ResetLinkFont() => _linkFont = new Font("Tahoma", 8.0f, GraphicsUnit.Point);

        internal bool IsDesigner => DesignMode;

        public TaskPane()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint 
                | ControlStyles.StandardClick, true);

            DoubleBuffered = true;

            Dock = DockStyle.Right;
            Font = new Font("Tahoma", 8, FontStyle.Bold, GraphicsUnit.Point);

            _groups = new ObservableCollection<TaskPaneGroup>();
            _groups.CollectionChanged += (s, e) =>
            {
                foreach (var grp in _groups)
                { 
                    if (grp._pane != this)
                    {
                        grp._pane = this;
                        grp.OnInitialized();
                    }

                    foreach (var link in grp.Links)
                        link._pane = this;
                }

                Invalidate();
            };

            AutoScroll = true;
            AutoScrollMinSize = Size.Empty;
        }

        private TaskPaneGroup _hoveredGroup = null;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            _hoveredGroup = null;

            Point contentPoint = new Point(
                e.Location.X - AutoScrollPosition.X,
                e.Location.Y - AutoScrollPosition.Y);

            foreach (var grp in _groups)
            {
                if (grp.HitTest(contentPoint))
                {
                    _hoveredGroup = grp;

                    if (grp._topRect.Contains(contentPoint) || grp.Links.Any(x => x._hover))
                        this.Cursor = Cursors.Hand;
                    else
                        this.Cursor = Cursors.Default;

                    break;
                }
            }

            if (_hoveredGroup is null)
                this.Cursor = Cursors.Default;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            _hoveredGroup?.OnClick();
        }

        private static Color FromHex(int hex)
        {
            return Color.FromArgb(
                (hex >> 16) & 0xFF,
                (hex >> 8) & 0xFF,
                hex & 0xFF);
        }

#nullable enable
        /// <summary>
        /// Fires when a link is clicked within the <see cref="TaskPane"/>.
        /// </summary>
        [Category("Action")]
        [Description("Fires when a link is clicked within the TaskPane.")]
        public event EventHandler<TaskPaneLinkClickedEventArgs>? LinkClicked;

        /// <summary>
        /// Fires when the <see cref="TaskPaneGroup.Expanded"/> property of a group within this <see cref="TaskPane"/> changes.
        /// </summary>
        [Category("Property Changed")]
        [Description("Fires when the Expanded property of a group within this TaskPane changes.")]
        public event EventHandler<TaskPaneGroupExpandedChangedEventArgs>? GroupExpandedChanged;
#nullable disable

        private ObservableCollection<TaskPaneGroup> _groups;

        protected internal void OnLinkClicked(TaskPaneLinkClickedEventArgs e)
        {
            LinkClicked?.Invoke(this, e);
        }

        protected internal void OnGroupExpandedChanged(TaskPaneGroupExpandedChangedEventArgs e)
        {
            GroupExpandedChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the System Manager toolbar should be hidden when the
        /// <see cref="TaskPane"/> is on the page.
        /// </summary>
        /// <remarks>This property cannot be set after the page is loaded.</remarks>
        [Description("Gets or sets a value that indicates whether the System Manager toolbar should be hidden when the TaskPane is on the page. " +
            "This property cannot be set after the page is loaded.")]
        [DefaultValue(true)]
        [Category("Behavior")]
        public bool HideToolbarWhenActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the collection of groups in the <see cref="TaskPane"/>.
        /// </summary>
        [Description("Gets or sets the collection of groups in the TaskPane.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<TaskPaneGroup> Groups => _groups; 

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            if (UseDefaultBackColor)
            {
                using (SolidBrush backBrush = new SolidBrush(ColorScheme switch
                {
                    ColorScheme.Blue =>   FromHex(0x687ed9),
                    ColorScheme.Olive =>  FromHex(0xacc28b),
                    ColorScheme.Silver => FromHex(0xb4b7ca),
                    ColorScheme.System => SystemColors.Window,
                    _ => SystemColors.Window
                }))
                {
                    g.FillRectangle(backBrush, ClientRectangle);
                }
            }

            e.Graphics.TranslateTransform(
                AutoScrollPosition.X,
                AutoScrollPosition.Y);

            // else, just let WinForms paint the BackColor
            int y = 8;

            foreach (var group in _groups)
            {
                group.Paint(g, ColorScheme, ClientSize.Width - 16, ref y);
                y += 12;
            }

            AutoScrollMinSize = new Size(0, y);
        }
    }

    /// <summary>
    /// Represents an object that provides event information for the <see cref="TaskPane.LinkClicked"/> event.
    /// </summary>
    public class TaskPaneLinkClickedEventArgs : EventArgs
    {
        internal TaskPaneLink _link;

        /// <summary>
        /// Gets the <see cref="TaskPaneLink"/> clicked.
        /// </summary>
        public TaskPaneLink Link => _link;

        public TaskPaneLinkClickedEventArgs(TaskPaneLink link)
        {
            _link = link;
        }
    }

    /// <summary>
    /// Represents an object that provides event information for the <see cref="TaskPane.GroupExpandedChanged"/> event.
    /// </summary>
    public class TaskPaneGroupExpandedChangedEventArgs : EventArgs
    {
        internal TaskPaneGroup _grp;


        /// <summary>
        /// Gets the <see cref="TaskPaneGroup"/> for this event.
        /// </summary>
        public TaskPaneGroup Group => _grp;

        public TaskPaneGroupExpandedChangedEventArgs(TaskPaneGroup grp)
        {
            _grp = grp;
        }
    }
}
