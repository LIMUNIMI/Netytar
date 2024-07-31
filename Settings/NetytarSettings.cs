using NITHdmis.Music;
using System;
using Netytar.Modules;

namespace Netytar.Settings
{
    [Serializable]
    public class NetytarSettings
    {
        private float _sensorIntensityBreath;
        private float _sensorIntensityHead;
        private float _sensorIntensityMouth;
        private float _sensorIntensityTeeth;

        public NetytarSettings()
        {
        }

        public NetytarSettings(float sensorIntensityBreath, float sensorIntensityHead, float sensorIntensityTeeth, float sensorIntensityMouth, _BlinkSelectScaleMode blinkSelectScaleMode, _BreathControlModes breathControlMode, int ellipseRadius, int highlightRadius, int highlightStrokeDim, int horizontalSpacer, int lineThickness, int midiPort, _ModulationControlModes modulationControlMode, NetytarInteractionMethods interactionMethod, bool noteNamesVisualized, int occluderOffset, AbsNotes rootNote, ScaleCodes scaleCode, int sensorPort, _SharpNotesModes sharpNotesMode, _SlidePlayModes slidePlayMode, int verticalSpacer)
        {
            _sensorIntensityBreath = sensorIntensityBreath;
            _sensorIntensityHead = sensorIntensityHead;
            _sensorIntensityTeeth = sensorIntensityTeeth;
            _sensorIntensityMouth = sensorIntensityMouth;
            BlinkSelectScaleMode = blinkSelectScaleMode;
            BreathControlMode = breathControlMode;
            EllipseRadius = ellipseRadius;
            HighlightRadius = highlightRadius;
            HighlightStrokeDim = highlightStrokeDim;
            HorizontalSpacer = horizontalSpacer;
            LineThickness = lineThickness;
            MIDIPort = midiPort;
            ModulationControlMode = modulationControlMode;
            InteractionMethod = interactionMethod;
            NoteNamesVisualized = noteNamesVisualized;
            OccluderOffset = occluderOffset;
            RootNote = rootNote;
            ScaleCode = scaleCode;
            SensorPort = sensorPort;
            SharpNotesMode = sharpNotesMode;
            SlidePlayMode = slidePlayMode;
            VerticalSpacer = verticalSpacer;
        }

        public _BlinkSelectScaleMode BlinkSelectScaleMode { get; set; }
        public _BreathControlModes BreathControlMode { get; set; }
        public int EllipseRadius { get; set; }
        public int HighlightRadius { get; set; }
        public int HighlightStrokeDim { get; set; }
        public int HorizontalSpacer { get; set; }
        public NetytarInteractionMethods InteractionMethod { get; set; }
        public int LineThickness { get; set; }
        public int MIDIPort { get; set; }
        public _ModulationControlModes ModulationControlMode { get; set; }
        public bool NoteNamesVisualized { get; set; }
        public int OccluderOffset { get; set; }
        public AbsNotes RootNote { get; set; }
        public ScaleCodes ScaleCode { get; set; }

        public float SensorIntensityBreath
        {
            get => _sensorIntensityBreath;
            set => _sensorIntensityBreath = Math.Clamp(value, 0.1f, 10.0f);
        }

        public float SensorIntensityHead
        {
            get => _sensorIntensityHead;
            set => _sensorIntensityHead = Math.Clamp(value, 0.1f, 10.0f);
        }

        public float SensorIntensityMouth
        {
            get => _sensorIntensityMouth;
            set => _sensorIntensityMouth = Math.Clamp(value, 0.1f, 10.0f);
        }

        public float SensorIntensityTeeth
        {
            get => _sensorIntensityTeeth;
            set => _sensorIntensityTeeth = Math.Clamp(value, 0.1f, 10.0f);
        }

        public int SensorPort { get; set; }
        public _SharpNotesModes SharpNotesMode { get; set; }
        public _SlidePlayModes SlidePlayMode { get; set; }
        public int VerticalSpacer { get; set; }
    }
}