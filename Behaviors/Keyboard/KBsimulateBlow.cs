using Netytar.Modules;
using NITHdmis.Modules.Keyboard;
using RawInputProcessor;

namespace Netytar.Behaviors.Keyboard
{
    public class KBsimulateBlow : IKeyboardBehavior
    {
        private VKeyCodes keyBlow = VKeyCodes.Space;

        private bool blowing = false;
        int returnVal = 0;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            returnVal = 0;

            if(Rack.UserSettings.InteractionMethod == NetytarInteractionMethods.Keyboard)
            {
                if (e.VirtualKey == (ushort)keyBlow && e.KeyPressState == KeyPressState.Down)
                {
                    blowing = true;
                    returnVal = 1;
                    Rack.NetytarDmiBox.BreathValue = 127;
                    Rack.NetytarDmiBox.Velocity = 127;
                    Rack.NetytarDmiBox.Pressure = 127;
                }
                else if (e.VirtualKey == (ushort)keyBlow && e.KeyPressState == KeyPressState.Up)
                {
                    blowing = false;
                    returnVal = 1;
                    Rack.NetytarDmiBox.BreathValue = 0;
                    Rack.NetytarDmiBox.Velocity = 0;
                    Rack.NetytarDmiBox.Pressure = 0;
                }
                Rack.NetytarDmiBox.Blow = blowing;
            }

            return returnVal;
        }
    }
}
