using Netytar.Behaviors.Eyetracker;
using Netytar.Behaviors.Keyboard;
using Netytar.Surface;
using NITHdmis.Modules.Keyboard;
using NITHdmis.Modules.MIDI;
using NITHdmis.Modules.Mouse;
using NITHdmis.Music;
using NITHlibrary.Nith.Module;
using NITHlibrary.Tools.Filters.PointFilters;
using NITHlibrary.Tools.Ports;
using RawInputProcessor;
using System.Windows.Interop;

namespace Netytar.Modules
{
    public class NetytarSetup
    {
        private bool disposed = false;

        public NetytarSetup(MainWindow window)
        {
            Rack.NetytarMainWindow = window;
        }

        private List<IDisposable> disposables = [];

        public void Setup()
        {
            // MIDI
            Rack.MidiModule = new MidiModuleNAudio(1, 1);
            Rack.MidiModule.OutDevice = 1;

            // Motion sensor
            Rack.USBreceiverMotionSensor = new USBreceiver();
            Rack.NithModuleSensor = new NithModule();
            Rack.USBreceiverMotionSensor.Listeners.Add(Rack.NithModuleSensor);
            //Rack.NithModuleSensor.SensorBehaviors.Add(new NithSensorBehaviorPressurePlay(NetytarInteractionMethods.Breath, 0.125f, 1, 8));
            //Rack.NithModuleSensor.SensorBehaviors.Add(new NithSensorBehaviorPressurePlay(NetytarInteractionMethods.Teeth, 1f, 1, 1));
            //Rack.NithModuleSensor.SensorBehaviors.Add(new NithSensorBehaviorYawPlay());

            // Mouse Module
            Rack.MouseModule = new MouseModule();

            // Eye tracker
            Rack.UDPreceiverEyeTracker = new UDPreceiver(20101);
            Rack.NithModuleEyeTracker = new NithModule();
            Rack.UDPreceiverEyeTracker.Listeners.Add(Rack.NithModuleEyeTracker);
            Rack.GazeToMouse = new NithSensorBehavior_GazeToMouse(Rack.MouseModule);
            Rack.NithModuleEyeTracker.SensorBehaviors.Add(Rack.GazeToMouse);
            Rack.NithModuleEyeTracker.SensorBehaviors.Add(new EBBselectScale(Rack.NetytarMainWindow));
            Rack.NithModuleEyeTracker.SensorBehaviors.Add(new EBBrepeatNote());
            Rack.NithModuleEyeTracker.SensorBehaviors.Add(new EBBdoubleCloseClick());
            Rack.UDPreceiverEyeTracker.Connect();

            // Keyboard Module
            IntPtr windowHandle = new WindowInteropHelper(Rack.NetytarMainWindow).Handle;
            Rack.KeyboardModule = new KeyboardModuleWPF(windowHandle, RawInputCaptureMode.Foreground);
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBautoScroller());
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBemulateMouse());
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBstopAutoScroller());
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBstopEmulateMouse());
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBsimulateBlow());
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBselectScale());


            // Surface
            // R.NetytarDmiBox.AutoScroller = new Autoscroller_ButtonFollower(R.NetytarMainWindow.scrlNetytar, 0, 130, new PointFilterMAExpDecaying(0.07f)); // OLD was 100, 0.1f
            Rack.NetytarDmiBox.AutoScroller = new AutoScroller(Rack.NetytarMainWindow.scrlNetytar, 0, 100, new PointFilterMAExpDecaying(0.07f)); // OLD was 100, 0.1f
            Rack.NetytarDmiBox.NetytarSurface = new NetytarSurface(Rack.NetytarMainWindow.canvasNetytar, Rack.DrawMode);
            Rack.NetytarDmiBox.NetytarSurface.DrawButtons();
            Rack.NetytarDmiBox.NetytarSurface.Scale = new Scale(Rack.UserSettings.RootNote, Rack.UserSettings.ScaleCode);

            // Disposables
            disposables.Add(Rack.USBreceiverMotionSensor);
            disposables.Add(Rack.UDPreceiverEyeTracker);
            disposables.Add(Rack.NithModuleEyeTracker);
            disposables.Add(Rack.NithModuleSensor);
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }

            disposables.Clear();
        }
    }
}