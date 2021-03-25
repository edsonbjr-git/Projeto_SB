using Java.Util;

namespace BalizaFacil.Droid.Services
{
    public static class BluetoothSensorAttributes
    {
        //Valores do sensor que está junto ao gyro
        public static UUID AccelerometerService = UUID.FromString("f000aa20-0451-4000-b000-000000000000");
        public static UUID AccelerometerData = UUID.FromString("f000aa21-0451-4000-b000-000000000000");
        public static UUID AccelerometerConfiguration = UUID.FromString("f000aa22-0451-4000-b000-000000000000");
        public static UUID AccelerometerPeriod = UUID.FromString("f000aa23-0451-4000-b000-000000000000");
        public static UUID AccelerometerNotification = UUID.FromString("00002902-0000-1000-8000-00805f9b34fb");

        //Valores do acelerometro separado ( LIS3DHTR) , descomente-os pra conectar com esse sensor
        //public static UUID AccelerometerService = UUID.FromString("f000aa20-0451-4000-b000-000000000000");
        //public static UUID AccelerometerData = UUID.FromString("f000aa21-0451-4000-b000-000000000000");
        //public static UUID AccelerometerConfiguration = UUID.FromString("f000aa22-0451-4000-b000-000000000000");
        //public static UUID AccelerometerPeriod = UUID.FromString("f000aa23-0451-4000-b000-000000000000");
        //public static UUID AccelerometerNotification = UUID.FromString("00002902-0000-1000-8000-00805f9b34fb");

        public static UUID GyroscopeService = UUID.FromString("f000aa50-0451-4000-b000-000000000000");
        public static UUID GyroscopeData = UUID.FromString("f000aa51-0451-4000-b000-000000000000");
        public static UUID GyroscopeConfiguration = UUID.FromString("f000aa52-0451-4000-b000-000000000000");
        public static UUID GyroscopePeriod = UUID.FromString("f000aa53-0451-4000-b000-000000000000");
        public static UUID GyroscopeNotification = UUID.FromString("00002902-0000-1000-8000-00805f9b34fb");
    }
}