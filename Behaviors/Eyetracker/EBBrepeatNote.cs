using Netytar.Modules;
using NITHlibrary.Nith.Behaviors;

namespace Netytar.Behaviors.Eyetracker
{
    public class EBBrepeatNote : ANithBlinkEventBehavior
    {
        public EBBrepeatNote()
        {
            DCThresh = 4;
        }

        protected override void Event_doubleClose()
        {
            if (Rack.NetytarDmiBox.Blow)
            {
                Rack.NetytarDmiBox.Blow = false;
                Rack.NetytarDmiBox.Blow = true;
                //NetytarRack.DMIBox.NetytarSurface.FlashSpark();
            }
        }

        protected override void Event_doubleOpen() { }

        protected override void Event_leftClose() { }

        protected override void Event_leftOpen() { }

        protected override void Event_rightClose() { }

        protected override void Event_rightOpen() { }
    }
}
