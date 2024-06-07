using Netytar.Modules;
using NITHdmis.Modules.Keyboard;
using NITHdmis.Music;
using RawInputProcessor;

namespace Netytar.Behaviors.Keyboard
{
    class KBselectScale : IKeyboardBehavior
    {
        private const VKeyCodes keyMaj = VKeyCodes.Add;
        private const VKeyCodes keyMin = VKeyCodes.Subtract;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyMaj && e.KeyPressState == KeyPressState.Down)
            {
                Rack.NetytarDmiBox.NetytarSurface.Scale = new Scale(Rack.NetytarDmiBox.NetytarSurface.CheckedButton.Note.ToAbsNote(), ScaleCodes.maj);
                return 1;
            }
            if (e.VirtualKey == (ushort)keyMaj && e.KeyPressState == KeyPressState.Up)
            {
                Rack.NetytarDmiBox.NetytarSurface.Scale = new Scale(Rack.NetytarDmiBox.NetytarSurface.CheckedButton.Note.ToAbsNote(), ScaleCodes.min);
            };
            return 0;
        }
    }
}
