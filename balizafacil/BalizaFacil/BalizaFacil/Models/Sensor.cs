using BalizaFacil.Core;
using System;

namespace BalizaFacil.Models
{
    public class Sensor
    {
        public event Action<SensorStatus> StatusChanged;
        //public event Action DataChanged;

        private static Sensor instance;
        public static Sensor Instance
        {
            get
            {
                if (instance is null)
                    instance = new Sensor();

                return instance;
            }
        }

        public static string DefaultName { get; set; } =  FlowManager.Instance.UseSensorBaliza ? "BalizaFacil" : "CC2650 SensorTag"; 
        public SensorFieldData Gyroscope { get; set; }
        public SensorFieldData Accelerometer { get; set; }
        public SensorFieldData Magnetometer { get; set; }

        private volatile SensorStatus status;
        public SensorStatus Status
        {
            get { return status; }
            set
            {
                if (status == value)
                    return;

                status = value;
                StatusChanged?.Invoke(Status);
            }
        }

        private Sensor()
        {
            this.Gyroscope = new SensorFieldData();
            this.Accelerometer = new SensorFieldData();
            this.Magnetometer = new SensorFieldData();
            Status = SensorStatus.Unknown;
        }
        public void SetGyro(int gyroscopeX, int gyroscopeY, int gyroscopeZ)
        {
            this.Gyroscope.X = gyroscopeX;
            this.Gyroscope.Y = gyroscopeY;
            this.Gyroscope.Z = gyroscopeZ;

        }

        public void SetAcc(int accelerometerX, int accelerometerY, int accelerometerZ)
        {
            this.Accelerometer.X = accelerometerX;
            this.Accelerometer.Y = accelerometerY;
            this.Accelerometer.Z = accelerometerZ;
        }

        public static bool StatusChecked = false;
        public bool checkStatus(int gyroscopeX, int gyroscopeY, int gyroscopeZ, int accelerometerX, int accelerometerY, int accelerometerZ, int magnetometerX, int magnetometerY, int magnetometerZ)
        {
            //return (gyroscopeX != this.Gyroscope.X || gyroscopeY != this.Gyroscope.Y || gyroscopeZ != this.Gyroscope.Z ||
            //    accelerometerX != this.Accelerometer.X || accelerometerY != this.Accelerometer.Y || accelerometerZ != this.Accelerometer.Z ||
            //    magnetometerX != this.Magnetometer.X || magnetometerY != this.Magnetometer.Y || magnetometerZ != this.Magnetometer.Z);
            return (Math.Abs(gyroscopeX - this.Gyroscope.X)>0.0001 || Math.Abs(gyroscopeY - this.Gyroscope.Y) > 0.0001 || Math.Abs(gyroscopeZ - this.Gyroscope.Z) > 0.0001 ||
                Math.Abs(accelerometerX - this.Accelerometer.X)>0.0001 || Math.Abs(accelerometerY - this.Accelerometer.Y) > 0.0001 || Math.Abs(accelerometerZ - this.Accelerometer.Z) > 0.0001 );
        }
    }
}