using Java.Util;

namespace BalizaFacil.Droid.Services
{
    public static class BluetoothSensorTagAttributes
    {
        public static UUID KeyService = UUID.FromString("0000ffe0-0000-1000-8000-00805f9b34fb");
        public static UUID KeyData = UUID.FromString("0000ffe1-0000-1000-8000-00805f9b34fb");
        public static UUID ClientConfigurationDescriptor = UUID.FromString("00002902-0000-1000-8000-00805f9b34fb");
        public static UUID MovementService = UUID.FromString("f000aa80-0451-4000-b000-000000000000");
        public static UUID MovementData = UUID.FromString("f000aa81-0451-4000-b000-000000000000");
        public static UUID MovementConfiguration = UUID.FromString("f000aa82-0451-4000-b000-000000000000");
        public static UUID MovementPeriod = UUID.FromString("f000aa83-0451-4000-b000-000000000000");
    }
}