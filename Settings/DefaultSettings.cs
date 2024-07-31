using NITHdmis.Music;
using System;
using Netytar.Modules;

namespace Netytar.Settings
{
    [Serializable]
    internal class DefaultSettings : NetytarSettings
    {
        public DefaultSettings() : base()
        {
            HighlightStrokeDim = 5;
            HighlightRadius = 65;
            VerticalSpacer = -100;
            HorizontalSpacer = 200;
            OccluderOffset = 35;
            EllipseRadius = 23;
            LineThickness = 3;
            SharpNotesMode = _SharpNotesModes.On;
            BlinkSelectScaleMode = _BlinkSelectScaleMode.On;
            BreathControlMode = _BreathControlModes.Dynamic;
            ModulationControlMode = _ModulationControlModes.Off;
            InteractionMethod = NetytarInteractionMethods.Breath;
            SlidePlayMode = _SlidePlayModes.On;
            SensorPort = 4;
            MIDIPort = 1;
            RootNote = AbsNotes.C;
            ScaleCode = ScaleCodes.maj;
            NoteNamesVisualized = false;
            SensorIntensityBreath = 1f;
            SensorIntensityTeeth = 1f;
            SensorIntensityHead = 1f;
            SensorIntensityMouth = 1f;
        }
    }
}