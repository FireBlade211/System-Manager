using FireBlade.SystemManager.Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;

namespace FireBlade.SystemManager.Helpers.Controls
{
    /// <summary>
    /// Represents a group in a <see cref="Controls.TaskPane"/>.
    /// </summary>
    public class TaskPaneGroup
    {
        public TaskPaneGroup()
        {
            _links = new ObservableCollection<TaskPaneLink>();
            _links.CollectionChanged += _links_CollectionChanged;

            _timer.Tick += _timer_Tick;
        }

        private bool _collectionAnimating = false;

        private void _links_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var link in _links)
                link._pane = _pane;

            if (_expanded)
            {
                _animating = true;
                _collectionAnimating = true;

                _timer.Start();
                _pane?.Invalidate();
            }
        }

        private ObservableCollection<TaskPaneLink> _links;

        private bool _inited = false;

        protected internal void OnInitialized()
        {
            _inited = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="TaskPaneGroup"/> is expanded.
        /// </summary>
        [Description("Gets or sets a value indicating whether the TaskPaneGroup is expanded.")]
        [DefaultValue(true)]
        [Category("Behavior")]
        public bool Expanded
        {
            get => _expanded;
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    
                    if (_pane?.IsDesigner == true || _inited)
                    {
                        _animating = true;

                        _timer.Start();
                        _pane.Invalidate();
                    }

                    _pane?.OnGroupExpandedChanged(new TaskPaneGroupExpandedChangedEventArgs(this));
                }
            }
        }

        /// <summary>
        /// Gets or sets the collection of links in the <see cref="TaskPaneGroup"/>.
        /// </summary>
        [Description("Gets or sets the collection of links in the TaskPaneGroup.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<TaskPaneLink> Links => _links;

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (_collectionAnimating)
                _animationProgress = Math.Min(1.0, _animationProgress + 0.025);
            else if (_animating)
                _animationProgress = Math.Min(1.0, _animationProgress + 0.075);

            if (_widthAnimating)
                _widthAnimationProgress =
                    (float)Math.Min(1.0, _widthAnimationProgress + 0.15);

            if (_animating && _animationProgress >= 1.0)
            {
                _collectionAnimating = false;
                _animating = false;
                _animationProgress = 0;
            }

            if (_widthAnimating && _widthAnimationProgress >= 1.0)
            {
                _widthAnimating = false;
                _currentWidth = _targetWidth;
            }

            if (!_animating && !_widthAnimating)
                _timer.Stop();

            _pane.Invalidate();
        }

        internal TaskPane _pane;

        /// <summary>
        /// Gets or sets the title of the group.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the title of the group.")]
        [DefaultValue("Group")]
        public string Title
        {
            get;
            set
            {
                field = value;
                _pane?.Invalidate();
            }
        } = "Group";

        /// <summary>
        /// Gets or sets a user-defined value.
        /// </summary>
        [Description("Gets or sets a user-defined value.")]
        [DefaultValue(null)]
        [TypeConverter(typeof(StringConverter))]
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the group is visible.
        /// </summary>
        [Description("Gets or sets a value indicating whether the group is visible.")]
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

        /// <summary>
        /// Gets the <see cref="Controls.TaskPane"/> that owns this group.
        /// </summary>
        [Description("Gets the TaskPane that owns this group.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public TaskPane TaskPane => _pane;

        protected static void FillRoundedRectangle(Graphics g, Brush brush, RectangleF rect, float radius)
        {
            using (var path = new GraphicsPath())
            {
                float d = radius * 2;

                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();

                g.FillPath(brush, path);
            }
        }

        protected static Bitmap LoadBitmap(string name)
        {
            var assembly = typeof(TaskPane).Assembly;

            using (var stream = assembly.GetManifestResourceStream("FireBlade.SystemManager.Helpers.Resources." + name))
            {
                if (stream == null)
                    throw new InvalidOperationException($"Embedded resource '{name}' was not found.");

                // clone the bitmap
                using (var temp = new Bitmap(stream))
                    return new Bitmap(temp);
            }
        }

        protected static void FillRoundedTopRectangle(Graphics g, Brush brush, RectangleF rect, float radius)
        {
            radius = Math.Min(radius, Math.Min(rect.Width, rect.Height) / 2.0f);
            float d = radius * 2;

            using (var path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom);
                path.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom);
                path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y + radius);
                path.CloseFigure();

                g.FillPath(brush, path);
            }
        }

        private Timer _timer = new Timer
        {
            Interval = 16 // 60 FPS
        };

        private bool _animating = false;
        private double _animationProgress = 0.0f;

        internal protected virtual void OnClick()
        {
            bool expanderClicked = true;

            if (!Visible) expanderClicked = false;
            if (_animating || !_hover) expanderClicked = false;

            if (expanderClicked)
            {
                _expanded = !_expanded;

                _animating = true;
                _timer.Start();

                _pane.Invalidate();
                _pane.OnGroupExpandedChanged(new TaskPaneGroupExpandedChangedEventArgs(this));
            }
            else
            {
                var client = _pane.PointToClient(Cursor.Position);
                var pt = new Point(
                    client.X - _pane.AutoScrollPosition.X,
                    client.Y - _pane.AutoScrollPosition.Y); ;

                foreach (var link in _links)
                {
                    if (!link.Enabled) continue;

                    bool hit = link.HitTest(pt);
                    if (hit)
                    {
                        _pane.OnLinkClicked(new TaskPaneLinkClickedEventArgs(link));

                        break;
                    }
                }    
            }
        }

        private float _currentWidth;
        private float _startWidth;
        private float _targetWidth;
        private float _widthAnimationProgress;
        private bool _widthAnimating;

        private bool _hover = false;
        private bool _expanded = true;

        private Rectangle _rect = Rectangle.Empty;
        internal RectangleF _topRect = RectangleF.Empty;

        internal protected virtual bool HitTest(Point point)
        {
            if (!Visible)
            {
                _hover = false;

                return false;
            }

            bool result = _hover;

            if (!_rect.IsEmpty && _rect.Contains(point))
                result = true;
            else
                result = false;

            bool newHover = _hover;

            if (!_topRect.IsEmpty && _topRect.Contains(point))
            {
                newHover = true;
            }
            else
            {
                newHover = false;
            }

            // Hit test links.
            foreach (var link in _links)
            {
                link.HitTest(point);
            }

            //bool lockCursorChange = false;

            //foreach (var link in _links)
            //{
            //    bool linkLocksCursor = false;
            //    link.HitTest(point, ref linkLocksCursor);

            //    lockCursorChange |= linkLocksCursor;
            //}

            //_lockCursorChange = lockCursorChange;

            _hover = newHover;
            _pane.Invalidate();

            return result;
        }

        //private bool _lockCursorChange = false;

        private void AnimateWidthTo(float targetWidth)
        {
            if (_targetWidth == targetWidth)
                return;

            _startWidth = _currentWidth;
            _targetWidth = targetWidth;
            _widthAnimationProgress = 0;
            _widthAnimating = true;

            _timer.Start();
        }

        private int _oldHeight = 0;

        internal protected virtual void Paint(Graphics g, ColorScheme scheme, int initialWidth, ref int y)
        {
            if (!Visible) return;

            Color backColor = scheme switch
            {
                ColorScheme.Blue => Color.FromArgb(214, 223, 247),
                ColorScheme.OliveLuna => Color.FromArgb(246, 246, 236),
                ColorScheme.SilverLuna => Color.FromArgb(240, 241, 245),
                ColorScheme.Royale => SystemColors.ButtonHighlight,
                ColorScheme.System or _ => SystemColors.Window
            };

            int width = initialWidth;

            if (_currentWidth == 0)
                _currentWidth = initialWidth;

            if (initialWidth != _targetWidth || !_widthAnimating)
            {
                if (initialWidth != _targetWidth)
                    AnimateWidthTo(initialWidth);
            }

            if (_widthAnimating)
            {
                double easing = EaseOut(_widthAnimationProgress);

                width = (int)Math.Round(
                    _startWidth + (_targetWidth - _startWidth) * easing);
            }
            else
            {
                width = initialWidth;
                _currentWidth = initialWidth;
            }

            int initialHeight = 36;

            if (_expanded || _animating)
                foreach (var link in _links)
                    initialHeight += (int)Math.Round(link.Measure(g, width).Height);

            int height = initialHeight;
            double opacity = 1.0f;

            if (_animating)
            {
                double easing = _expanded
                    ? EaseOut(_animationProgress)
                    : 1.0 - EaseIn(_animationProgress);
                
                if (!_collectionAnimating)
                    backColor = Color.FromArgb(
                        (int)Math.Round(easing * 255),
                        backColor);

                if (_collectionAnimating)
                    height = (int)Math.Round(
                        _oldHeight + (initialHeight - _oldHeight) * easing);
                else
                    height = (int)Math.Round(
                        25 + (initialHeight - 25) * easing);

                if (!_collectionAnimating)
                    opacity = easing;
            }

            using Brush backBrush = new SolidBrush(backColor);

            if ((scheme is ColorScheme.System or ColorScheme.Royale) && (_expanded || _animating))
            {
                using (Pen pen = new Pen(Color.FromArgb(212, 208, 200), 2))
                {
                    g.DrawRectangle(pen, new Rectangle(8, 25 + y - 1, width, height - 25 - 1));
                }
            }

            if (!_expanded && !_animating)
                height = 25;

            _rect = new Rectangle(8, y, width, height);

            if (_expanded || _animating)
            {
                var rc = new Rectangle(8, y + 25, width, height - 25);

                g.FillRectangle(backBrush, rc);

                g.DrawLine(Pens.White, rc.Left, rc.Top, rc.Left, rc.Bottom - 1);
                g.DrawLine(Pens.White, rc.Left, rc.Bottom - 1, rc.Right - 1, rc.Bottom - 1);
                g.DrawLine(Pens.White, rc.Right - 1, rc.Bottom - 1, rc.Right - 1, rc.Top);
            }


            y += height;

            // Draw the caption.

            using Brush captionBrush = new LinearGradientBrush(new Point(0, 0), new Point(width + 8, 0),
                    scheme == ColorScheme.System ? Color.FromArgb(212, 208, 200) : Color.White, scheme switch
                    {
                        ColorScheme.BlueLuna => Color.FromArgb(198, 211, 247),
                        ColorScheme.OliveLuna => Color.FromArgb(224, 231, 184),
                        ColorScheme.SilverLuna => Color.FromArgb(194, 198, 210),
                        ColorScheme.Royale => Color.LightGray,
                        _ => Color.FromArgb(212, 208, 200) // I can't find the exact constant they used, sorry
                    });
            using Brush captionTextBrush = new SolidBrush(scheme switch
            {
                ColorScheme.BlueLuna => _hover ? Color.FromArgb(66, 142, 255) : Color.FromArgb(33, 93, 198),
                ColorScheme.OliveLuna => _hover ? Color.FromArgb(114, 146, 29) : Color.FromArgb(86, 102, 45),
                ColorScheme.SilverLuna => _hover ? Color.FromArgb(126, 124, 124) : Color.FromArgb(63, 61, 61),
                ColorScheme.System or _ => _hover ? SystemColors.ActiveCaptionText : SystemColors.InactiveCaptionText
            });

            _topRect = new RectangleF(8.0f, y - height, width + 1, 25);

            if (scheme == ColorScheme.System)
                g.FillRectangle(captionBrush, _topRect);
            else
                FillRoundedTopRectangle(g, captionBrush, _topRect, 2);

            g.DrawString(Title, TaskPane.Font, captionTextBrush, new RectangleF(12, (y - height) + 4, width - 28, 25),
                new StringFormat(StringFormatFlags.NoWrap)
                {
                    Trimming = StringTrimming.EllipsisCharacter
                });

            using Bitmap expanderBmp = LoadBitmap($"expander-{scheme switch
            {
                ColorScheme.Blue => "blue",
                ColorScheme.Silver => "silver",
                ColorScheme.Olive => "green",
                _ => "system"
            }}-" + (_expanded ? "expanded" : "collapsed") + (_hover ? "-hover" : string.Empty) + ".png");

            // Paint the expander.
            Point expanderLocation = new Point(width - 16, (y - height) + 4);
            if (_hover && scheme == ColorScheme.System)
            {
                var borderRect = new Rectangle(expanderLocation, expanderBmp.Size);

                var light = SystemColors.ControlLight;
                var dark  = SystemColors.ControlDark;

                using (var lightPen = new Pen(light))
                using (var darkPen  = new Pen(dark))
                {
                    g.DrawLine(lightPen, borderRect.Left, borderRect.Bottom - 1, borderRect.Left, borderRect.Top);
                    g.DrawLine(lightPen, borderRect.Left, borderRect.Top, borderRect.Right - 1, borderRect.Top);

                    g.DrawLine(darkPen, borderRect.Left, borderRect.Bottom - 1, borderRect.Right - 1, borderRect.Bottom - 1);
                    g.DrawLine(darkPen, borderRect.Right - 1, borderRect.Top, borderRect.Right - 1, borderRect.Bottom - 1);
                }
            }

            g.DrawImage(expanderBmp, expanderLocation);

            y -= height;
            y += 25 + 2;

            int linksTop = y;

            // Draw links that fit within the currently visible content.
            if (_expanded || _animating)
            {
                var state = g.Save();

                int linkY = linksTop;
                int contentBottom = _rect.Bottom;

                try
                {
                    //g.SetClip(_rect, CombineMode.Intersect);

                    foreach (var link in _links)
                    {
                        int linkHeight = (int)Math.Round(link.Measure(g).Height);

                        // The link must fit completely inside the group.
                        if (linkY + linkHeight > contentBottom)
                        {
                            int tempY = linkY;
                            link.Paint(g, scheme, _rect.X + 4, width - 8, opacity, ref tempY);
                        }
                        else
                            link.Paint(g, scheme, _rect.X + 4, width - 8, opacity, ref linkY);
                    }
                }
                finally
                {
                    g.Restore(state);
                }
            }

            // Keep the next group positioned at the animated group's bottom,
            // independently of how many links were painted.
            y = _rect.Bottom;

            _oldHeight = height;

            //y += height;
            //y -= 25 + 2;
        }

        private static double EaseIn(double t)
        {
            return t * t;
        }

        private static double EaseOut(double t)
        {
            return 1.0 - (1.0 - t) * (1.0 - t);
        }

        public override string ToString() => Title;
    }
}
