using Netytar.Modules;
using NITHlibrary.Nith.Internals;
using NITHlibrary.Tools.Mappers;

namespace Netytar.Behaviors.Pressure
{
    // A behavior that reads input from a pressure sensor, regulating note on/note off, MIDI channel pressure, and introducing some modulation.
    // The input comes in the form |press=val/max (e.g., |press=200/800). Proportioned to a percentage (e.g., 25% <- 200/800).
    internal class NithSensorBehaviorPressurePlay : INithSensorBehavior
    {
        private const float LOWERTHRESH = 1f;

        // Double threshold to handle Note On/Note Off. Above 5 -> Note On. Below 1 -> Note Off
        private const float UPPERTHRESH = 5f;

        private readonly NithParameters associatedParameter;

        // Internal values to adjust the detected value (multiplication constants)
        private readonly ValueMapperDouble
            inputMapper; // Used to proportion input to MIDI range from 0-100 to 0-127

        private readonly float modulationDivider;
        private readonly float pressureMultiplier; // Multiplier for channel pressure

        // Divider for modulation
        private readonly float sensitivity; // General sensitivity, a multiplier applied to the input

        /// <summary>
        ///     Initializes the behavior with provided settings (including default values).
        /// </summary>
        /// <param name="associatedInteractionMethod">Associated control mode</param>
        /// <param name="modulationDivider">Multiplier for modulation effect</param>
        /// <param name="pressureMultiplier">Channel pressure multiplier</param>
        /// <param name="sensitivity">Multiplier for input sensitivity</param>
        public NithSensorBehaviorPressurePlay(NithParameters associatedParameter, float sensitivity = 1f, float pressureMultiplier = 1f, float modulationDivider = 0.125f
            )
        {
            this.modulationDivider = modulationDivider;
            this.pressureMultiplier = pressureMultiplier;
            this.sensitivity = sensitivity;
            this.associatedParameter = associatedParameter;

            // Initialize mapper
            inputMapper = new ValueMapperDouble(100, 127);
        }

        // All incoming values from NithModuleBreathSensor are passed to this method as NithSensorData.
        public void HandleData(NithSensorData nithData)
        {
            // If a pressure value has actually arrived...
            if (nithData.ContainsParameter(associatedParameter))
            {
                // Extract pressure values using this method (nithData.GetValue). Parse as double, specifying number format using CultureInfo.InvariantCulture.
                var input = nithData.GetParameter(associatedParameter).Value.Proportional;

                input *= sensitivity; // Apply Sensitivity
                if (input > 100) input = 100; // Maximum threshold of 100
                Rack.NetytarDmiBox.InputIndicatorValue = input; // Provide "raw" input value to the instrument's logic module to update a graphical indicator
                input = inputMapper.Map(input); // Map to MIDI range 0-127
                Rack.NetytarDmiBox.Pressure = (int)(input * pressureMultiplier); // Update instrument logic by changing MIDI channel pressure
                Rack.NetytarDmiBox.Modulation = (int)(input / modulationDivider); // Change modulation

                // Check the double threshold to determine whether to send a note on/note off (the "Blow" in the instrument's logic)
                if ((int)input > UPPERTHRESH && !Rack.NetytarDmiBox.Blow) Rack.NetytarDmiBox.Blow = true;

                if ((int)input <= LOWERTHRESH) Rack.NetytarDmiBox.Blow = false;
            }
        }
    }
}