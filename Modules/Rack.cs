using Netytar.Settings;
using Netytar.Surface;
using Netytar.Surface.ButtonsSettings;
using Netytar.Surface.ColorCodes;
using NITHdmis.Modules.Keyboard;
using NITHdmis.Modules.MIDI;
using NITHdmis.Modules.Mouse;
using NITHlibrary.Nith.Module;
using NITHlibrary.Nith.Preprocessors;
using NITHlibrary.Tools.Ports;

namespace Netytar.Modules
{
    internal static class Rack
    {
        public const int HORIZONTALSPACING_MAX = 300;
        public const int HORIZONTALSPACING_MIN = 80;

        public static NithPreprocessor_HeadTrackerCalibrator PreprocessorHeadTrackerCalibrator = new();
        public static IButtonsSettings ButtonsSettings { get; set; } = new DefaultButtonSettings();
        public static IColorCode ColorCode { get; set; } = new DefaultColorCode();
        public static NetytarSurfaceLineModes DrawMode { get; set; } = NetytarSurfaceLineModes.OnlyScaleLines;
        public static KeyboardModuleWPF KeyboardModule { get; set; }
        public static IMidiModule MidiModule { get; set; }
        public static NetytarDMIBox NetytarDmiBox { get; set; } = new NetytarDMIBox();
        public static MainWindow NetytarMainWindow { get; set; }
        public static NithModule NithModuleEyeTracker { get; set; }
        public static NithModule NithModuleSensor { get; set; }
        public static bool RaiseClickEvent { get; internal set; } = false;
        public static SavingSystem SavingSystem { get; set; } = new SavingSystem("Settings");
        public static UDPreceiver UDPreceiverEyeTracker { get; set; }
        public static USBreceiver USBreceiverMotionSensor { get; set; }
        public static NetytarSettings UserSettings { get; set; } = new DefaultSettings();
        public static MouseModule MouseModule { get; set; }
        public static NithSensorBehavior_GazeToMouse GazeToMouse { get; set; }
        public static UDPreceiver UDPreceiverFaceCam { get; set; }
        public static NithModule NithModuleFaceCam { get; set; }
    }
}