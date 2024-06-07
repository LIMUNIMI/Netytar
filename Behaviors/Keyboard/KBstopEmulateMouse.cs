using Netytar.Modules;
using NITHdmis.Modules.Keyboard;
using RawInputProcessor;

namespace Netytar.Behaviors.Keyboard
{
    public class KBstopEmulateMouse : IKeyboardBehavior
    {
        private VKeyCodes keyAction = VKeyCodes.A;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                Rack.GazeToMouse.Enabled = false;
                Rack.NetytarDmiBox.CursorHidden = false;
                Rack.NetytarMainWindow.Cursor = Rack.NetytarDmiBox.CursorHidden ? System.Windows.Input.Cursors.None : System.Windows.Input.Cursors.Arrow;

                return 0;
            }

            return 1;
        }
    }
}