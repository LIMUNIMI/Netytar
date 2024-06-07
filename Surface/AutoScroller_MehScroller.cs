using System.Drawing;
using System.Windows.Controls;
using System.Windows.Threading;
using Netytar.Modules;
using NITHlibrary.Tools.Filters.PointFilters;

namespace Netytar.Surface
{
    /// <summary>
    /// Automatically scrolls a ScrollViewer following the mouse.
    /// </summary>
    public class AutoScroller_MehScroller
    {
        #region Params
        protected ScrollViewer scrollViewer;
        protected int radiusThreshold;
        protected int proportional;
        protected IPointFilter filter;
        #endregion

        #region Scrollviewer params
        protected System.Windows.Point scrollCenter;
        protected System.Windows.Point basePosition;
        #endregion

        #region Internal counters
        protected DispatcherTimer samplerTimer = new DispatcherTimer(DispatcherPriority.Render);
        // private Timer samplerTimer = new Timer();
        // private MicroTimer samplerTimer = new MicroTimer();
        protected Point lastSampledPoint;
        protected Point lastMean;
        protected double Xdifference;
        protected double Ydifference;
        #endregion

        protected bool enabled = false;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        public AutoScroller_MehScroller(ScrollViewer scrollViewer, int radiusThreshold, int proportional, IPointFilter filter)
        {
            this.radiusThreshold = radiusThreshold;
            this.filter = filter;
            this.scrollViewer = scrollViewer;
            this.proportional = proportional;

            // Setting scrollviewer dimensions
            lastSampledPoint = new Point();
            basePosition = scrollViewer.PointToScreen(new System.Windows.Point(0, 0));
            scrollCenter = new System.Windows.Point(scrollViewer.ActualWidth / 2, scrollViewer.ActualHeight / 2);

            // Setting sampling timer
            samplerTimer.Interval = new TimeSpan(10000);//1000; //1;
            //samplerTimer.MicroTimerElapsed += SamplerTimer_MicroTimerElapsed;
            samplerTimer.Tick += Scroll;
            samplerTimer.Start();
        }

        protected void Scroll(object sender, EventArgs e)
        {

            if (enabled)
            {
                var buttonX = Canvas.GetLeft(Rack.NetytarDmiBox.NetytarSurface.CheckedButton);
                var buttonY = Canvas.GetTop(Rack.NetytarDmiBox.NetytarSurface.CheckedButton);

                var differenceX = scrollViewer.HorizontalOffset - buttonX + scrollCenter.X;
                var differenceY = scrollViewer.VerticalOffset - buttonY + scrollCenter.Y;

                if (Math.Abs(differenceX) > radiusThreshold && Math.Abs(differenceY) > radiusThreshold)
                {
                    scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + Math.Pow((differenceX / proportional), 2) * Math.Sign(differenceX));
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + Math.Pow((differenceY / proportional), 2) * Math.Sign(differenceY));
                }
            }
        }
    }
}
