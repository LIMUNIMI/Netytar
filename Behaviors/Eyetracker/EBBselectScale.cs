using Netytar.Modules;
using NITHdmis.Music;
using NITHlibrary.Nith.Behaviors;

namespace Netytar.Behaviors.Eyetracker
{
    public class EBBselectScale : ANithBlinkEventBehavior
    {
        private MainWindow mainWindow;

        public EBBselectScale(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            LCThresh = 40;
            RCThresh = 40;
        }

        protected override void Event_doubleClose() { }

        protected override void Event_doubleOpen() { }

        protected override void Event_leftClose()
        {
            if(Rack.UserSettings.BlinkSelectScaleMode == _BlinkSelectScaleMode.On)
            {
                Rack.NetytarMainWindow.SelectedScale = new Scale(Rack.NetytarDmiBox.NetytarSurface.CheckedButton.Note.ToAbsNote(), ScaleCodes.maj);
            }
        }

        protected override void Event_leftOpen() { }

        protected override void Event_rightClose()
        {
            if (Rack.UserSettings.BlinkSelectScaleMode == _BlinkSelectScaleMode.On)
            {
                Rack.NetytarMainWindow.SelectedScale = new Scale(Rack.NetytarDmiBox.NetytarSurface.CheckedButton.Note.ToAbsNote(), ScaleCodes.min);
            }
        }
        protected override void Event_rightOpen() { }
    }
}
