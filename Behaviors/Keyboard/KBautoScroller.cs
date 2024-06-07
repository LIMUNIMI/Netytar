using Netytar.Modules;
using NITHdmis.Modules.Keyboard;
using RawInputProcessor;

namespace Netytar.Behaviors.Keyboard
{
    public class KBautoScroller : IKeyboardBehavior
    {
        const VKeyCodes keyAction = VKeyCodes.W;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                SetStuff();

                return 0;
            }

            return 1;
        }

        private void SetStuff()
        {
            Rack.NetytarDmiBox.AutoScroller.Enabled = true;
        }
    }
}