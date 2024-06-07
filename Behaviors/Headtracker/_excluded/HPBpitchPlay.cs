using Netytar.DMIbox;
using Netytar.Modules;

namespace Netytar.Behaviors.Headtracker._excluded
{
    public class HPBpitchPlay
    {
        private int offThresh;
        private int onThresh;
        private float maxSwing;
        private float sensitivity;

        private float b;
        public float B
        {
            get { return b; }
            set
            {
                if (value < 0)
                {
                    b = 0;
                }
                else if (value > maxSwing)
                {
                    b = 50;
                }
                else if (value == 0) { }
                else if (value < -50) { }
                else
                {
                    b = value;
                }

            }
        }

        public HPBpitchPlay(int offThresh, int onThresh, float sensitivity, float maxSwing)
        {
            this.offThresh = offThresh;
            this.onThresh = onThresh;
            this.sensitivity = sensitivity;
            this.maxSwing = maxSwing;
        }



        public void ReceiveHeadPoseData(HeadPoseData data)
        {

            if (Rack.UserSettings.NetytarControlMode == _NetytarControlModes.EyePos)
            {
                if (data.HasHeadPosition)
                {
                    B = (float)(data.HeadRotation.Y - Rack.NetytarDmiBox.HeadPoseBaseY);
                    int Bnorm = (int)(B * 127f / maxSwing);

                    Rack.NetytarMainWindow.BreathSensorValue = Bnorm;
                    Rack.NetytarDmiBox.Pressure = (int)(Bnorm * 2 * sensitivity);
                    //NetytarRack.DMIBox.Modulation = (int)(Bnorm / 8 * sensitivity);

                    if (Bnorm > onThresh && Rack.NetytarDmiBox.Blow == false)
                    {
                        Rack.NetytarDmiBox.Blow = true;
                        //NetytarRack.DMIBox.Pressure = 110;
                    }

                    if (Bnorm < offThresh)
                    {
                        Rack.NetytarDmiBox.Blow = false;
                    }
                }
            }

        }
    }
}
