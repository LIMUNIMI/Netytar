using System.Globalization;
using Netytar.Modules;
using NITHlibrary.Nith.Internals;
using NITHlibrary.Tools.Filters.ValueFilters;
using NITHlibrary.Tools.Mappers;

namespace Netytar.Behaviors.Headtracker
{
    public class NithSensorBehaviorYawPlay(double pressureMultiplier = 1.0f, double velocityMultiplier = 1.5f)
        : INithSensorBehavior
    {

        private const int Deadspeed = 40;
        private readonly ValueMapperDouble _pressureMapper = new(0.5f, 127);
        private readonly DoubleFilterMAExpDecaying _speedFilter = new(0.8f);
        private int _currentDirection = 1;
        private int _previousDirection = 1;
        private double _yawSpeed = 0;
        private double _yawSpeedFiltered;

        ///<summary>
        ///Handles the NithSensorData by checking the associated control mode and calling the HTStrum_ElaboratePosition method if the NithSensorData contains the "acc_yaw" argument.
        ///</summary>
        ///<param name="nithData">The NithSensorData object to handle.</param>
        public void HandleData(NithSensorData nithData)
        {
                if (nithData.ContainsParameter(NithParameters.head_acc_yaw))
                {
                    HTStrum_ElaboratePosition(nithData);
                }
        }
        

        public void HTStrum_ElaboratePosition(NithSensorData nithData)
        {
            _yawSpeed = double.Parse(nithData.GetParameter(NithParameters.head_acc_yaw).Value.Base, CultureInfo.InvariantCulture);
            _previousDirection = _currentDirection;
            _currentDirection = Math.Sign(_yawSpeed);

            // Perché tutto 'sto casino?
            _speedFilter.Push(_yawSpeed);
            _yawSpeedFiltered = _pressureMapper.Map(_speedFilter.Pull());
            _yawSpeedFiltered = Math.Log(_yawSpeedFiltered, 1.5f) * pressureMultiplier;

            Rack.NetytarDmiBox.Pressure = (int)_yawSpeedFiltered - Deadspeed;

            if (_currentDirection != _previousDirection && Rack.NetytarDmiBox.Pressure > 0)
            {
                Rack.NetytarDmiBox.Velocity = (int)(_yawSpeedFiltered * velocityMultiplier - Deadspeed);
                Rack.NetytarDmiBox.Blow = false;
                Rack.NetytarDmiBox.Blow = true;
            }
        }
    }
}