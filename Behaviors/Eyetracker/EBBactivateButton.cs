using System.Windows;
using System.Windows.Controls.Primitives;
using Netytar.Modules;
using NITHlibrary.Nith.Behaviors;

namespace Netytar.Behaviors.Eyetracker
{
    public class EBBactivateButton : ANithBlinkEventBehavior
    {
        public EBBactivateButton()
        {
            DCThresh = 4;
        }

        protected override void Event_doubleClose()
        {
            if(Rack.UserSettings.InteractionMethod == NetytarInteractionMethods.EyePos)
            {
                if (Rack.NetytarDmiBox.HasAButtonGaze)
                {
                    Rack.NetytarDmiBox.LastGazedButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                }
            }
        }

        protected override void Event_doubleOpen() { }

        protected override void Event_leftClose() { }

        protected override void Event_leftOpen() { }

        protected override void Event_rightClose() { }

        protected override void Event_rightOpen() { }
    }
}
