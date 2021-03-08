using System;
using System.Threading;
using Android.Bluetooth;
using Android.Runtime;
using BalizaFacil.Core;
using BalizaFacil.Droid.Services.Bluetooth;
using BalizaFacil.Models;
using Java.Math;

namespace BalizaFacil.Droid.Services
{
    public class SensorGATTCallback : BluetoothGattCallback
    {
        private ConfigurationStep ConfigurationStep { get; set; }
        private BluetoothGatt BluetoothGatt { get; set; }
        public byte GyroscopePeriod { get; set; } = byte.MaxValue;
        public byte AccelerometerPeriod { get; set; } = byte.MaxValue;

        public DateTime? LastCharacteristicChange { get; private set; }
        Thread ConnectionVerification { get; set; }

        private volatile bool timeoutVerification;



        public void ConfigureAccelerometer(BluetoothGatt gatt)
        {
            ConfigurationStep = ConfigurationStep.AccelerometerConfigured;
            var accelerometer = gatt.GetService(BluetoothSensorAttributes.AccelerometerService).GetCharacteristic(BluetoothSensorAttributes.AccelerometerConfiguration);
            accelerometer.SetValue(new byte[] { 1 });
            gatt.WriteCharacteristic(accelerometer);
        }

        public void ConfigureAccelerometerPeriod(BluetoothGatt gatt)
        {
            ConfigurationStep = ConfigurationStep.AccelerometerPeriod;
            var accelerometerPeriod = gatt.GetService(BluetoothSensorAttributes.AccelerometerService).GetCharacteristic(BluetoothSensorAttributes.AccelerometerPeriod);

            if (accelerometerPeriod.GetValue() != null)
                return;

            accelerometerPeriod.SetValue(new byte[] { AccelerometerPeriod });
            gatt.WriteCharacteristic(accelerometerPeriod);
        }

        public void ConfigureAccelerometerNotification(BluetoothGatt gatt)
        {
            ConfigurationStep = ConfigurationStep.AccelerometerNotification;
            var accelerometerData = gatt.GetService(BluetoothSensorAttributes.AccelerometerService).GetCharacteristic(BluetoothSensorAttributes.AccelerometerData);
            var accelerometerNotification = accelerometerData.GetDescriptor(BluetoothSensorAttributes.AccelerometerNotification);


            accelerometerNotification.SetValue(new byte[] { 1, 0 });

            gatt.SetCharacteristicNotification(accelerometerData, true);
            gatt.WriteDescriptor(accelerometerNotification);
        }


        public void ConfigureGyroscope(BluetoothGatt gatt)
        {
            ConfigurationStep = ConfigurationStep.GyroscopeConfigured;

            var gyroscope = gatt.GetService(BluetoothSensorAttributes.GyroscopeService)?.GetCharacteristic(BluetoothSensorAttributes.GyroscopeConfiguration);
            gyroscope.SetValue(new byte[] { 7 });
            gatt.WriteCharacteristic(gyroscope);
        }

        public void ConfigureGyroscopePeriod(BluetoothGatt gatt)
        {
            ConfigurationStep = ConfigurationStep.GyroscopePeriod;
            var gyroscopePeriod = gatt.GetService(BluetoothSensorAttributes.GyroscopeService).GetCharacteristic(BluetoothSensorAttributes.GyroscopePeriod);

            if (gyroscopePeriod.GetValue() != null)
                return;

            gyroscopePeriod.SetValue(new byte[] { GyroscopePeriod });
            gatt.WriteCharacteristic(gyroscopePeriod);
        }

        public void ConfigureGyroscopeNotification(BluetoothGatt gatt)
        {
            ConfigurationStep = ConfigurationStep.GyroscopeNotification;
            var gyroscopeData = gatt.GetService(BluetoothSensorAttributes.GyroscopeService).GetCharacteristic(BluetoothSensorAttributes.GyroscopeData);
            var gyroscopeNotification = gyroscopeData.GetDescriptor(BluetoothSensorAttributes.GyroscopeNotification);


            gyroscopeNotification.SetValue(new byte[] { 1, 0 });

            gatt.SetCharacteristicNotification(gyroscopeData, true);
            gatt.WriteDescriptor(gyroscopeNotification);
        }


        public override void OnConnectionStateChange(BluetoothGatt gatt, [GeneratedEnum] GattStatus status, [GeneratedEnum] ProfileState newState)
        {
            try
            {
                base.OnConnectionStateChange(gatt, status, newState);

                if (status == GattStatus.Success && newState == ProfileState.Connected) // && FlowManager.CurrentStep1 >= ApplicationStep.MeasureSpace)
                {
                    //Console.WriteLine("\n\n\n\n\n\n---> " + FlowManager.CurrentStep1 + "\n\n\n\n\n\n\n\n");

                    ConfigurationStep = ConfigurationStep.None;
                    gatt.DiscoverServices();
                    BluetoothGatt = gatt;
                }
                else
                {
                    Disconnect();
                }
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void Disconnect()
        {
            try
            {
                timeoutVerification = false;
                BluetoothGatt?.Disconnect();
                BluetoothGatt?.Close();
                BluetoothGatt = null;
                lock (Sensor.Instance)
                    Sensor.Instance.Status = SensorStatus.Disconnected;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public override void OnServicesDiscovered(BluetoothGatt gatt, [GeneratedEnum] GattStatus status)
        {
            BluetoothGatt = gatt;
            if (ConfigurationStep == ConfigurationStep.None)
                ConfigureAccelerometer(gatt);

            base.OnServicesDiscovered(gatt, status);
        }
        public override void OnCharacteristicWrite(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, [GeneratedEnum] GattStatus status)
        {
            BluetoothGatt = gatt;
            if (ConfigurationStep == ConfigurationStep.AccelerometerConfigured)
                ConfigureAccelerometerPeriod(gatt);
            else if (ConfigurationStep == ConfigurationStep.AccelerometerPeriod)
                ConfigureAccelerometerNotification(gatt);
            else if (ConfigurationStep == ConfigurationStep.GyroscopeConfigured)
                ConfigureGyroscopePeriod(gatt);
            else if (ConfigurationStep == ConfigurationStep.GyroscopePeriod)
                ConfigureGyroscopeNotification(gatt);

            if (resetingPeriod)
                ConfigureGyroscopePeriod();

            base.OnCharacteristicWrite(gatt, characteristic, status);
        }

        private bool resetingPeriod { get; set; }
        public void ResetSensorPeriod()
        {
            if (BluetoothGatt is null)
            {
                AccelerometerPeriod = 10;
                GyroscopePeriod = 10;
                return;
            }
            resetingPeriod = true;
            ConfigureAccelerometerPeriod();
        }

        public void ConfigureAccelerometerPeriod()
        {
            var accelerometerPeriod = BluetoothGatt.GetService(BluetoothSensorAttributes.AccelerometerService).GetCharacteristic(BluetoothSensorAttributes.AccelerometerPeriod);
            accelerometerPeriod.SetValue(new byte[] { 10 });
            BluetoothGatt.WriteCharacteristic(accelerometerPeriod);
        }

        public void ConfigureGyroscopePeriod()
        {
            var accelerometerPeriod = BluetoothGatt.GetService(BluetoothSensorAttributes.GyroscopeService).GetCharacteristic(BluetoothSensorAttributes.GyroscopePeriod);
            accelerometerPeriod.SetValue(new byte[] { 10 });
            BluetoothGatt.WriteCharacteristic(accelerometerPeriod);
        }

        public override void OnDescriptorWrite(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, [GeneratedEnum] GattStatus status)
        {
            BluetoothGatt = gatt;
            if (ConfigurationStep == ConfigurationStep.AccelerometerNotification)
                ConfigureGyroscope(gatt);


            base.OnDescriptorWrite(gatt, descriptor, status);
        }

        DateTime gyroLastTime, accLastTime;
        bool gyroSetted, accSetted;
        int gyroscopeY, gyroscopeX, gyroscopeZ, accelerometerX, accelerometerY, accelerometerZ;
        public override void OnCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic)
        {
            BluetoothGatt = gatt;
            try
            {

                //if (LastCharacteristicChange.HasValue)
                //Console.WriteLine(DateTime.Now.Subtract(LastCharacteristicChange.Value).TotalMilliseconds + " " + FlowManager.CurrentStep1);


                if (Sensor.Instance.Status != SensorStatus.Connected)
                {
                    lock (Sensor.Instance)
                        Sensor.Instance.Status = SensorStatus.Connected;
                }

                if (ConnectionVerification == null || !ConnectionVerification.IsAlive)
                    StartConnectionVerification();

                LastCharacteristicChange = DateTime.Now;

                if (BluetoothSensorAttributes.GyroscopeData.ToString() == characteristic.Uuid.ToString())
                {
                    int millisecondsPassedg = 0;

                    if (gyroLastTime != DateTime.MinValue)
                        millisecondsPassedg = (int)DateTime.Now.Subtract(gyroLastTime).TotalMilliseconds;

                    gyroLastTime = DateTime.Now;

                    gyroscopeX = (int)characteristic.GetIntValue(GattFormat.Sint16, 0);
                    gyroscopeY = (int)characteristic.GetIntValue(GattFormat.Sint16, 2);
                    gyroscopeZ = (int)characteristic.GetIntValue(GattFormat.Sint16, 4);
                    accelerometerX = (int)characteristic.GetIntValue(GattFormat.Sint8, 6);
                    accelerometerY = (int)characteristic.GetIntValue(GattFormat.Sint8, 7);
                    accelerometerZ = (int)characteristic.GetIntValue(GattFormat.Sint8, 8);

                    Sensor.Instance.SetGyro(gyroscopeX, gyroscopeY, gyroscopeZ);
                    Sensor.Instance.SetAcc(accelerometerX, accelerometerY, accelerometerZ);
                }

                //if (BluetoothSensorAttributes.AccelerometerData.ToString() == characteristic.Uuid.ToString())
                //{
                //    int millisecondsPasseda = 0;

                //    if (accLastTime != DateTime.MinValue)
                //        millisecondsPasseda = (int)DateTime.Now.Subtract(accLastTime).TotalMilliseconds;

                //    accLastTime = DateTime.Now;

                //    accelerometerX = (int)characteristic.GetIntValue(GattFormat.Sint8, 0);
                //    accelerometerY = (int)characteristic.GetIntValue(GattFormat.Sint8, 1);
                //    accelerometerZ = (int)characteristic.GetIntValue(GattFormat.Sint8, 2);

                //    accSetted = true;
                //    //Console.WriteLine($"ACC: {millisecondsPasseda} ms");

                //    if(gyroSetted && accSetted)
                //    {
                //        accSetted = gyroSetted = false;
                //        Sensor.Instance.SetAcc(accelerometerX, accelerometerY, accelerometerZ);
                //        Sensor.Instance.SetGyro(gyroscopeX, gyroscopeY, gyroscopeZ);
                //    }

                //}
                base.OnCharacteristicChanged(gatt, characteristic);
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        private void StartConnectionVerification()
        {
            timeoutVerification = true;
            ConnectionVerification = new Thread(() =>
            {
                try
                {
                    while (timeoutVerification)
                    {
                        if (LastCharacteristicChange is null)
                            return;

                        lock (Sensor.Instance)
                        {
                            TimeSpan difference = DateTime.Now.Subtract(LastCharacteristicChange.Value);

                            if (difference.TotalSeconds > 2 && Sensor.Instance.Status != SensorStatus.Disconnected)
                                Sensor.Instance.Status = SensorStatus.Disconnected;
                            else if (difference.TotalSeconds <= 2 && Sensor.Instance.Status != SensorStatus.Connected)
                                Sensor.Instance.Status = SensorStatus.Connected;
                        }
                        Thread.Sleep(2000);
                    }
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }

            });
            ConnectionVerification.Name = nameof(ConnectionVerification);
            ConnectionVerification.Priority = ThreadPriority.Highest;
            ConnectionVerification.IsBackground = true;

            ConnectionVerification.Start();
        }
    }
}