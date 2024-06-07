using Netytar.Modules;
using NITHdmis.Modules.Keyboard;
using RawInputProcessor;

namespace Netytar.Behaviors.Keyboard
{
    public class KBstopAutoScroller : IKeyboardBehavior
    {
        const VKeyCodes keyAction = VKeyCodes.S;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction && e.KeyPressState == KeyPressState.Down)
            {
                SetStuff();

                return 0;
            }

            return 1;
        }

        private void SetStuff()
        {
            Rack.NetytarDmiBox.AutoScroller.Enabled = false;
        }
    }
}