using Netytar.Modules;
using NITHlibrary.Nith.Behaviors;

namespace Netytar.Behaviors.Eyetracker
{
    class EBBdoubleCloseClick : ANithBlinkEventBehavior
    {
        public EBBdoubleCloseClick() : base()
        {
            DCThresh = 5;
        }
        protected override void Event_doubleClose()
        {
            if (Rack.GazeToMouse.Enabled)
            {
                Rack.RaiseClickEvent = true;
            }
        }

        protected override void Event_doubleOpen()
        {
        }

        protected override void Event_leftClose()
        {
        }

        protected override void Event_leftOpen()
        {
        }

        protected override void Event_rightClose()
        {
        }

        protected override void Event_rightOpen()
        {
        }
    }
}
