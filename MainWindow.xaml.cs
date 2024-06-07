using Netytar.Modules;
using NITHdmis.Music;
using NITHlibrary.Tools.Logging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Netytar.Behaviors.Headtracker;
using Netytar.Behaviors.Pressure;
using NITHlibrary.Nith.Internals;

namespace Netytar
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SolidColorBrush ActiveBrush = new SolidColorBrush(Colors.LightGreen);
        private readonly SolidColorBrush BlankBrush = new SolidColorBrush(Colors.Black);
        private readonly SolidColorBrush GazeButtonColor = new SolidColorBrush(Colors.DarkGoldenrod);
        private readonly SolidColorBrush WarningBrush = new SolidColorBrush(Colors.DarkRed);
        private Brush LastGazedBrush = null;
        private Scale lastScale = ScalesFactory.Cmaj;
        private NetytarSetup netytarSetup;
        private bool NetytarStarted = false;
        private DispatcherTimer updater;
        private double velocityBarMaxHeight = 0;

        public MainWindow()
        {
            InitializeComponent();

            // Debugger
            TraceAdder.AddTrace();
            DataContext = this;

            // GUI updater
            updater = new DispatcherTimer();
            updater.Interval = new TimeSpan(1000);
            updater.Tick += UpdateTimedVisuals;
            updater.Start();
        }

        public int BreathSensorValue { get; set; } = 0;
        public bool IsSettingsShown { get; set; } = false;
        public Button LastSettingsGazedButton { get; set; } = null;
        public Scale SelectedScale { get; set; } = ScalesFactory.Cmaj;

        public int SensorPort
        {
            get { return Rack.UserSettings.SensorPort; }
            set
            {
                if (value > 0)
                {
                    Rack.UserSettings.SensorPort = value;
                }
            }
        }

        public void ReceiveBlowingChange()
        {
            if (Rack.NetytarDmiBox.Blow)
            {
                txtIsBlowing.Text = "B";
            }
            else
            {
                txtIsBlowing.Text = "_";
            }
        }

        public void ReceiveNoteChange()
        {
            UpdateGUIVisuals();
        }

        internal void ChangeScale(ScaleCodes scaleCode)
        {
            Rack.NetytarDmiBox.NetytarSurface.Scale = new Scale(Rack.NetytarDmiBox.SelectedNote.ToAbsNote(), scaleCode);
        }

        private void btnBlinkPlay_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnBlinkSelectScale_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                switch (Rack.UserSettings.BlinkSelectScaleMode)
                {
                    case _BlinkSelectScaleMode.Off:
                        Rack.UserSettings.BlinkSelectScaleMode = _BlinkSelectScaleMode.On;
                        break;

                    case _BlinkSelectScaleMode.On:
                        Rack.UserSettings.BlinkSelectScaleMode = _BlinkSelectScaleMode.Off;
                        break;
                }

                UpdateGUIVisuals();
            }
        }

        private void btnBreath_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.InteractionMethod = NetytarInteractionMethods.Breath;
                ChangeInteractionMethod();
                UpdateGUIVisuals();
            }
        }

        private void btnBreathControlSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                if (Rack.NetytarDmiBox.BreathControlMode == _BreathControlModes.Switch)
                {
                    Rack.NetytarDmiBox.BreathControlMode = _BreathControlModes.Dynamic;
                }
                else if (Rack.NetytarDmiBox.BreathControlMode == _BreathControlModes.Dynamic)
                {
                    Rack.NetytarDmiBox.BreathControlMode = _BreathControlModes.Switch;
                }
            }

            BreathSensorValue = 0;

            UpdateGUIVisuals();
        }

        private void btnBSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.BreathControlMode = Rack.UserSettings.BreathControlMode == _BreathControlModes.Dynamic ? _BreathControlModes.Switch : _BreathControlModes.Dynamic;
                UpdateGUIVisuals();
            }
        }

        private void btnCalibrateHeadPose_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //btnCalibrateHeadPose.Background = new SolidColorBrush(Colors.Black);
        }

        private void btnCtrlEyePos_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.InteractionMethod = NetytarInteractionMethods.EyePos;
                Rack.NetytarDmiBox.ResetModulationAndPressure();

                BreathSensorValue = 0;

                UpdateGUIVisuals();
            }
        }

        private void btnCtrlEyeVel_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.InteractionMethod = NetytarInteractionMethods.EyeVel;
                Rack.NetytarDmiBox.ResetModulationAndPressure();

                BreathSensorValue = 0;

                UpdateGUIVisuals();
            }
        }

        private void btnCtrlKeyboard_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.InteractionMethod = NetytarInteractionMethods.Keyboard;
                Rack.NetytarDmiBox.ResetModulationAndPressure();

                BreathSensorValue = 0;

                UpdateGUIVisuals();
            }
        }

        private void btnExit_Activate(object sender, RoutedEventArgs e)
        {
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Rack.SavingSystem.SaveSettings(Rack.UserSettings);
            Close();
        }

        private void BtnFFBTest_Click(object sender, RoutedEventArgs e)
        {
            //Rack.DMIBox.FfbModule.FlashFFB();
        }

        private void btnHeadYaw_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.InteractionMethod = NetytarInteractionMethods.HeadYaw;
                ChangeInteractionMethod();
                UpdateGUIVisuals();
            }
        }

        private void btnKeyboard_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.InteractionMethod = NetytarInteractionMethods.Keyboard;
                ChangeInteractionMethod();
                UpdateGUIVisuals();
            }
        }

        private void BtnMIDIchMinus_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.MIDIPort--;
                Rack.MidiModule.OutDevice = Rack.UserSettings.MIDIPort;
                //lblMIDIch.Text = "MP" + Rack.DMIBox.MidiModule.OutDevice.ToString();

                //CheckMidiPort();
                UpdateGUIVisuals();
            }
        }

        private void BtnMIDIchPlus_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.MIDIPort++;
                Rack.MidiModule.OutDevice = Rack.UserSettings.MIDIPort;
                //lblMIDIch.Text = "MP" + Rack.DMIBox.MidiModule.OutDevice.ToString();

                //CheckMidiPort();
                UpdateGUIVisuals();
            }
        }

        private void btnMod_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.ModulationControlMode = Rack.UserSettings.ModulationControlMode == _ModulationControlModes.On ? _ModulationControlModes.Off : _ModulationControlModes.On;
                UpdateGUIVisuals();
            }
        }

        private void btnModulationControlSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.ModulationControlMode = Rack.UserSettings.ModulationControlMode == _ModulationControlModes.On ? _ModulationControlModes.Off : _ModulationControlModes.On;
                UpdateGUIVisuals();
            }
        }

        private void btnNeutral_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnNoCursor_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.NetytarDmiBox.CursorHidden = !Rack.NetytarDmiBox.CursorHidden;
                Cursor = Rack.NetytarDmiBox.CursorHidden ? System.Windows.Input.Cursors.None : System.Windows.Input.Cursors.Arrow;
            }

            UpdateGUIVisuals();
        }

        private void btnNoteNames_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.NoteNamesVisualized = !Rack.UserSettings.NoteNamesVisualized;
                Rack.NetytarDmiBox.NetytarSurface.NoteNamesVisualized = Rack.UserSettings.NoteNamesVisualized;
                UpdateGUIVisuals();
            }
        }

        private void btnRemoveSharps_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                if (Rack.UserSettings.SharpNotesMode == _SharpNotesModes.Off)
                {
                    Rack.UserSettings.SharpNotesMode = _SharpNotesModes.On;
                }
                else if (Rack.UserSettings.SharpNotesMode == _SharpNotesModes.On)
                {
                    Rack.UserSettings.SharpNotesMode = _SharpNotesModes.Off;
                }

                UpdateGUIVisuals();
                Rack.NetytarDmiBox.NetytarSurface.DrawButtons();
            }
        }

        private void btnRootNoteMinus_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.RootNote = Rack.UserSettings.RootNote.Previous();
                Rack.NetytarDmiBox.NetytarSurface.Scale = new Scale(Rack.UserSettings.RootNote, Rack.UserSettings.ScaleCode);
                UpdateGUIVisuals();
            }
        }

        private void btnRootNotePlus_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.RootNote = Rack.UserSettings.RootNote.Next();
                Rack.NetytarDmiBox.NetytarSurface.Scale = new Scale(Rack.UserSettings.RootNote, Rack.UserSettings.ScaleCode);
                UpdateGUIVisuals();
            }
        }

        private void btnScaleMajor_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.ScaleCode = ScaleCodes.maj;
                Rack.NetytarDmiBox.NetytarSurface.Scale = new Scale(Rack.UserSettings.RootNote, Rack.UserSettings.ScaleCode);
                UpdateGUIVisuals();
            }
        }

        private void btnScaleMinor_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.ScaleCode = ScaleCodes.min;
                Rack.NetytarDmiBox.NetytarSurface.Scale = new Scale(Rack.UserSettings.RootNote, Rack.UserSettings.ScaleCode);
                UpdateGUIVisuals();
            }
        }

        private void BtnScroll_Click(object sender, RoutedEventArgs e)
        {
            Rack.NetytarDmiBox.AutoScroller.Enabled = !Rack.NetytarDmiBox.AutoScroller.Enabled;
        }

        private void BtnSensorPortMinus_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                SensorPort--;
                UpdateSensorConnection();
            }
        }

        private void BtnSensorPortPlus_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                SensorPort++;
                UpdateSensorConnection();
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            switch (IsSettingsShown)
            {
                case false:
                    IsSettingsShown = true;
                    brdSettings.Visibility = Visibility.Visible;
                    break;

                case true:
                    IsSettingsShown = false;
                    brdSettings.Visibility = Visibility.Hidden;
                    break;
            }

            UpdateGUIVisuals();
        }

        private void btnSharpNotes_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.SharpNotesMode = Rack.UserSettings.SharpNotesMode == _SharpNotesModes.On ? _SharpNotesModes.Off : _SharpNotesModes.On;
                Rack.NetytarDmiBox.NetytarSurface.DrawButtons();
                UpdateGUIVisuals();
            }
        }

        private void btnSlidePlay_Click(object sender, RoutedEventArgs e)
        {
            // TODO Rifare (?)

            if (NetytarStarted)
            {
                switch (Rack.UserSettings.SlidePlayMode)
                {
                    case _SlidePlayModes.Off:
                        Rack.UserSettings.SlidePlayMode = _SlidePlayModes.On;
                        break;

                    case _SlidePlayModes.On:
                        Rack.UserSettings.SlidePlayMode = _SlidePlayModes.Off;
                        break;
                }

                UpdateGUIVisuals();
            }
        }

        private void btnSlidePlay_Click_1(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.SlidePlayMode = Rack.UserSettings.SlidePlayMode == _SlidePlayModes.On ? _SlidePlayModes.Off : _SlidePlayModes.On;
                UpdateGUIVisuals();
            }
        }

        private void btnSpacingMinus_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                if (Rack.UserSettings.HorizontalSpacer > Rack.HORIZONTALSPACING_MIN)
                {
                    Rack.UserSettings.HorizontalSpacer -= 10;
                    Rack.UserSettings.VerticalSpacer = -(Rack.UserSettings.HorizontalSpacer / 2);
                }

                Rack.NetytarDmiBox.NetytarSurface.DrawButtons();
                UpdateGUIVisuals();
            }
        }

        private void btnSpacingPlus_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                if (Rack.UserSettings.HorizontalSpacer < Rack.HORIZONTALSPACING_MAX)
                {
                    Rack.UserSettings.HorizontalSpacer += 10;
                    Rack.UserSettings.VerticalSpacer = -(Rack.UserSettings.HorizontalSpacer / 2);
                }

                Rack.NetytarDmiBox.NetytarSurface.DrawButtons();
                UpdateGUIVisuals();
            }
        }

        private void btnTeeth_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.UserSettings.InteractionMethod = NetytarInteractionMethods.Teeth;
                ChangeInteractionMethod();
                UpdateGUIVisuals();
            }
        }

        private void btnTestClick(object sender, RoutedEventArgs e)
        {
            throw (new NotImplementedException("Test button is not set!"));
        }

        private void btnToggleAutoScroll_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.NetytarDmiBox.AutoScroller.Enabled = !Rack.NetytarDmiBox.AutoScroller.Enabled;
            }

            UpdateGUIVisuals();
        }

        private void btnToggleEyeTracker_Click(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                Rack.GazeToMouse.Enabled = !Rack.GazeToMouse.Enabled;
            }

            UpdateGUIVisuals();
        }

        private void CheckMidiPort()
        {
            if (Rack.MidiModule.IsMidiOk())
            {
                txtMIDIch.Foreground = ActiveBrush;
            }
            else
            {
                txtMIDIch.Foreground = WarningBrush;
            }
        }

        private void StartNetytar()
        {
            // EventHandler for all buttons
            EventManager.RegisterClassHandler(typeof(Button), Button.MouseEnterEvent, new RoutedEventHandler(Global_SettingsButton_MouseEnter));

            Rack.UserSettings = Rack.SavingSystem.LoadSettings();
            netytarSetup = new NetytarSetup(this);
            netytarSetup.Setup();

            // Checks the selected MIDI port is available
            CheckMidiPort();

            brdSettings.Visibility = Visibility.Hidden;

            // LEAVE AT THE END!
            NetytarStarted = true;
            ChangeInteractionMethod();
            UpdateSensorConnection();
            UpdateGUIVisuals();
        }

        private void Test(object sender, RoutedEventArgs e)
        {
            Rack.NetytarDmiBox.NetytarSurface.DrawScale();
        }

        private void UpdateGUIVisuals()
        {
            // TEXT
            txtMIDIch.Text = "MP" + Rack.UserSettings.MIDIPort.ToString();
            txtSensorPort.Text = "COM" + Rack.UserSettings.SensorPort.ToString();
            txtRootNote.Text = Rack.UserSettings.RootNote.ToString();
            txtSpacing.Text = Rack.UserSettings.HorizontalSpacer.ToString();

            /// INDICATORS
            indBreath.Background = Rack.UserSettings.InteractionMethod == NetytarInteractionMethods.Breath ? ActiveBrush : BlankBrush;
            indTeeth.Background = Rack.UserSettings.InteractionMethod == NetytarInteractionMethods.Teeth ? ActiveBrush : BlankBrush;
            indHeadYaw.Background = Rack.UserSettings.InteractionMethod == NetytarInteractionMethods.HeadYaw ? ActiveBrush : BlankBrush;
            indKeyboard.Background = Rack.UserSettings.InteractionMethod == NetytarInteractionMethods.Keyboard ? ActiveBrush : BlankBrush;
            indRootNoteColor.Background = Rack.ColorCode.FromAbsNote(Rack.UserSettings.RootNote);
            indScaleMajor.Background = (Rack.UserSettings.ScaleCode == ScaleCodes.maj) ? ActiveBrush : BlankBrush;
            indScaleMinor.Background = (Rack.UserSettings.ScaleCode == ScaleCodes.min) ? ActiveBrush : BlankBrush;
            indMod.Background = Rack.UserSettings.ModulationControlMode == _ModulationControlModes.On ? ActiveBrush : BlankBrush;
            indBSwitch.Background = Rack.UserSettings.BreathControlMode == _BreathControlModes.Switch ? ActiveBrush : BlankBrush;
            indSharpNotes.Background = Rack.UserSettings.SharpNotesMode == _SharpNotesModes.On ? ActiveBrush : BlankBrush;
            indSlidePlay.Background = Rack.UserSettings.SlidePlayMode == _SlidePlayModes.On ? ActiveBrush : BlankBrush;
            indToggleCursor.Background = Rack.NetytarDmiBox.CursorHidden ? ActiveBrush : BlankBrush;
            indToggleAutoScroll.Background = Rack.NetytarDmiBox.AutoScroller.Enabled ? ActiveBrush : BlankBrush;
            indToggleEyeTracker.Background = Rack.GazeToMouse.Enabled ? ActiveBrush : BlankBrush;
            indSettings.Background = IsSettingsShown ? ActiveBrush : BlankBrush;
            indNoteNames.Background = Rack.NetytarDmiBox.NetytarSurface.NoteNamesVisualized ? ActiveBrush : BlankBrush;

            /* MIDI */
            txtMIDIch.Text = "MP" + Rack.MidiModule.OutDevice.ToString();
            CheckMidiPort();

            Update_SensorIntensityVisuals();
        }

        private void UpdateSensorConnection()
        {
            txtSensorPort.Text = "COM" + SensorPort.ToString();

            txtSensorPort.Foreground = Rack.USBreceiverMotionSensor.Connect(SensorPort) ? ActiveBrush : WarningBrush;
        }

        private void UpdateTimedVisuals(object sender, EventArgs e)
        {
            if (NetytarStarted)
            {
                if (SelectedScale.GetName().Equals(lastScale.GetName()) == false)
                {
                    lastScale = SelectedScale;
                    Rack.NetytarDmiBox.NetytarSurface.Scale = SelectedScale;
                    UpdateGUIVisuals();
                }

                txtNoteName.Text = Rack.NetytarDmiBox.SelectedNote.ToStandardString();
                txtPitch.Text = Rack.NetytarDmiBox.SelectedNote.ToPitchValue().ToString();
                if (Rack.NetytarDmiBox.Blow)
                {
                    txtIsBlowing.Text = "B";
                }
                else
                {
                    txtIsBlowing.Text = "_";
                }

                prbBreathSensor.Value = Rack.NetytarDmiBox.BreathValue;

                if (Rack.RaiseClickEvent)
                {
                    LastSettingsGazedButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    Rack.RaiseClickEvent = false;
                }

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartNetytar();
        }

        #region Global SettingsButtons

        public void Global_NetytarButtonMouseEnter()
        {
            if (NetytarStarted)
            {
                if (LastSettingsGazedButton != null)
                {
                    // Reset Previous Button
                    LastSettingsGazedButton.Background = LastGazedBrush;
                    LastSettingsGazedButton = null;
                }
            }
        }

        private void Global_SettingsButton_MouseEnter(object sender, RoutedEventArgs e)
        {
            if (NetytarStarted)
            {
                if (LastSettingsGazedButton != null)
                {
                    // Reset Previous Button
                    LastSettingsGazedButton.Background = LastGazedBrush;
                }

                LastSettingsGazedButton = (Button)sender;
                LastGazedBrush = LastSettingsGazedButton.Background;
                LastSettingsGazedButton.Background = GazeButtonColor;
            }
        }

        #endregion Global SettingsButtons

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            netytarSetup.Dispose();
        }

        private void BtnSensingIntensityMinus_OnClick(object sender, RoutedEventArgs e)
        {
            switch (Rack.UserSettings.InteractionMethod)
            {
                case NetytarInteractionMethods.Keyboard:
                    break;
                case NetytarInteractionMethods.Breath:
                    Rack.UserSettings.SensorIntensityBreath -= 0.1f;
                    break;
                case NetytarInteractionMethods.Teeth:
                    Rack.UserSettings.SensorIntensityTeeth -= 0.1f;
                    break;
                case NetytarInteractionMethods.HeadYaw:
                    Rack.UserSettings.SensorIntensityHead -= 0.1f;
                    break;
            }

            ChangeInteractionMethod();
            UpdateGUIVisuals();
        }

        private void BtnSensingIntensityPlus_OnClick(object sender, RoutedEventArgs e)
        {
            switch (Rack.UserSettings.InteractionMethod)
            {
                case NetytarInteractionMethods.Keyboard:
                    break;
                case NetytarInteractionMethods.Breath:
                    Rack.UserSettings.SensorIntensityBreath += 0.1f;
                    break;
                case NetytarInteractionMethods.Teeth:
                    Rack.UserSettings.SensorIntensityTeeth += 0.1f;
                    break;
                case NetytarInteractionMethods.HeadYaw:
                    Rack.UserSettings.SensorIntensityHead += 0.1f;
                    break;
            }

            ChangeInteractionMethod();
            UpdateGUIVisuals();
        }

        private void Update_SensorIntensityVisuals()
        {
            switch (Rack.UserSettings.InteractionMethod)
            {
                case NetytarInteractionMethods.Keyboard:
                    break;

                case NetytarInteractionMethods.Breath:
                    txtSensingIntensity.Text = Rack.UserSettings.SensorIntensityBreath.ToString("F1");
                    break;

                case NetytarInteractionMethods.Teeth:
                    txtSensingIntensity.Text = Rack.UserSettings.SensorIntensityTeeth.ToString("F1");
                    break;

                case NetytarInteractionMethods.HeadYaw:
                    txtSensingIntensity.Text = Rack.UserSettings.SensorIntensityHead.ToString("F1");
                    break;
                default:
                    break;
            }
        }

        private void ChangeInteractionMethod()
        {
            Rack.NithModuleSensor.SensorBehaviors.Clear();

            switch (Rack.UserSettings.InteractionMethod)
            {
                case NetytarInteractionMethods.HeadYaw:
                    Rack.NithModuleSensor.SensorBehaviors.Add(new NithSensorBehaviorYawPlay(Rack.UserSettings.SensorIntensityHead));
                    break;

                case NetytarInteractionMethods.Breath:
                    Rack.NithModuleSensor.SensorBehaviors.Add(new NithSensorBehaviorPressurePlay(NithParameters.breath_press, Rack.UserSettings.SensorIntensityBreath, Rack.UserSettings.SensorIntensityBreath * 1.5f));
                    break;

                case NetytarInteractionMethods.Teeth:
                    Rack.NithModuleSensor.SensorBehaviors.Add(new NithSensorBehaviorPressurePlay(NithParameters.teeth_press, Rack.UserSettings.SensorIntensityTeeth, Rack.UserSettings.SensorIntensityTeeth * 1.5f));
                    break;

                case NetytarInteractionMethods.Keyboard:
                    break;
            }
        }
    }
}