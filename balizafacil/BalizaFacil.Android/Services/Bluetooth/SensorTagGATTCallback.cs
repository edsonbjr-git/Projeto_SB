using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BalizaFacil.Core;
using BalizaFacil.Models;

namespace BalizaFacil.Droid.Services.Bluetooth
{
    public class SensorTagGATTCallback : BluetoothGattCallback
    {
        private SensorTagConfigurationStep SensorTagConfigurationStep { get; set; }
        private BluetoothGatt BluetoothGatt { get; set; }

        public DateTime? LastCharacteristicChange { get; private set; }
        Thread ConnectionVerification { get; set; }

        private volatile bool timeoutVerification;
        public void ConfigureKeyData(BluetoothGatt gatt)
        {
            try
            {
                var characteristic = gatt.GetService(BluetoothSensorTagAttributes.KeyService).GetCharacteristic(BluetoothSensorTagAttributes.KeyData);
                gatt.SetCharacteristicNotification(characteristic, true);

                var descriptor = characteristic.GetDescriptor(BluetoothSensorTagAttributes.ClientConfigurationDescriptor);
                descriptor.SetValue(new byte[] { 1, 0 });
                gatt.WriteDescriptor(descriptor);

                SensorTagConfigurationStep = SensorTagConfigurationStep.KeyData;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void ConfigureMovement(BluetoothGatt gatt)
        {
            try
            {
                var characteristic = gatt.GetService(BluetoothSensorTagAttributes.MovementService).GetCharacteristic(BluetoothSensorTagAttributes.MovementConfiguration);
                characteristic.SetValue(new byte[] { 0x7F, 0x02 });
                gatt.WriteCharacteristic(characteristic);

                SensorTagConfigurationStep = SensorTagConfigurationStep.Movement;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        private void ConfigureMovementData(BluetoothGatt gatt)
        {
            try
            {
                var characteristic = gatt.GetService(BluetoothSensorTagAttributes.MovementService).GetCharacteristic(BluetoothSensorTagAttributes.MovementData);
                gatt.SetCharacteristicNotification(characteristic, true);

                var descriptor = characteristic.GetDescriptor(BluetoothSensorTagAttributes.ClientConfigurationDescriptor);
                descriptor.SetValue(new byte[] { 1, 0 });
                gatt.WriteDescriptor(descriptor);

                SensorTagConfigurationStep = SensorTagConfigurationStep.MovementData;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }
        //static readonly int PeriodSensor = 90;
        public void ConfigureMovementPeriod(BluetoothGatt gatt)
        {
            try
            {
                var characteristic = gatt.GetService(BluetoothSensorTagAttributes.MovementService).GetCharacteristic(BluetoothSensorTagAttributes.MovementPeriod);
                //if (!App.sensorTime)
                //    characteristic.SetValue(new byte[] { 20 });
                //else 
                characteristic.SetValue(new byte[] { 20 });

                gatt.WriteCharacteristic(characteristic);

                SensorTagConfigurationStep = SensorTagConfigurationStep.MovementPeriod;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public override void OnConnectionStateChange(BluetoothGatt gatt, [GeneratedEnum] GattStatus status, [GeneratedEnum] ProfileState newState)
        {
            try
            {
                base.OnConnectionStateChange(gatt, status, newState);

                if (status == GattStatus.Success && newState == ProfileState.Connected) // && FlowManager.CurrentStep1 >= ApplicationStep.MeasureSpace)
                {
                    //Console.WriteLine("\n\n\n\n\n\n---> " + FlowManager.CurrentStep1 + "\n\n\n\n\n\n\n\n");

                    SensorTagConfigurationStep = SensorTagConfigurationStep.None;
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
            if (SensorTagConfigurationStep == SensorTagConfigurationStep.None)
                ConfigureKeyData(gatt);

            base.OnServicesDiscovered(gatt, status);
        }
        public override void OnCharacteristicWrite(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, [GeneratedEnum] GattStatus status)
        {
            if (SensorTagConfigurationStep == SensorTagConfigurationStep.Movement)
                ConfigureMovementData(gatt);

            base.OnCharacteristicWrite(gatt, characteristic, status);
        }
        public override void OnDescriptorWrite(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, [GeneratedEnum] GattStatus status)
        {
            if (SensorTagConfigurationStep == SensorTagConfigurationStep.KeyData)
                ConfigureMovement(gatt);
            else if (SensorTagConfigurationStep == SensorTagConfigurationStep.MovementData)
                ConfigureMovementPeriod(gatt);

            base.OnDescriptorWrite(gatt, descriptor, status);
        }
        public override void OnCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic)
        {
            try
            {

                //if (LastCharacteristicChange.HasValue)
                //    Console.WriteLine(DateTime.Now.Subtract(LastCharacteristicChange.Value).TotalMilliseconds + " " + FlowManager.CurrentStep1);

                if (BluetoothSensorTagAttributes.MovementData.ToString() != characteristic.Uuid.ToString())
                    return;

                if (Sensor.Instance.Status != SensorStatus.Connected)
                {
                    lock (Sensor.Instance)
                        Sensor.Instance.Status = SensorStatus.Connected;
                }

                if (ConnectionVerification == null || !ConnectionVerification.IsAlive)
                    StartConnectionVerification();

                LastCharacteristicChange = DateTime.Now;


                var gyroscopeX = (int)characteristic.GetIntValue(GattFormat.Sint16, 0);
                var gyroscopeY = (int)characteristic.GetIntValue(GattFormat.Sint16, 2);
                var gyroscopeZ = (int)characteristic.GetIntValue(GattFormat.Sint16, 4);

                var accelerometerX = (int)characteristic.GetIntValue(GattFormat.Sint16, 6);
                var accelerometerY = (int)characteristic.GetIntValue(GattFormat.Sint16, 8);
                var accelerometerZ = (int)characteristic.GetIntValue(GattFormat.Sint16, 10);

                var magnetometerX = (int)characteristic.GetIntValue(GattFormat.Sint16, 12);
                var magnetometerY = (int)characteristic.GetIntValue(GattFormat.Sint16, 14);
                var magnetometerZ = (int)characteristic.GetIntValue(GattFormat.Sint16, 16);

                Sensor.Instance.SetAcc(accelerometerX, accelerometerY, accelerometerZ);
                Sensor.Instance.SetGyro(gyroscopeX, gyroscopeY, gyroscopeZ);

                base.OnCharacteristicChanged(gatt, characteristic);
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        internal void ResetSensorPeriod()
        {
            
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
            ConnectionVerification.Priority = System.Threading.ThreadPriority.Highest;
            ConnectionVerification.IsBackground = true;

            ConnectionVerification.Start();
        }
    }
}