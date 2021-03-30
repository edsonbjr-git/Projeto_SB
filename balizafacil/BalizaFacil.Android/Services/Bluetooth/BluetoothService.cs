using System;
using BalizaFacil.Services;
using BalizaFacil.Droid.Services;
using BalizaFacil.Models;
using Xamarin.Forms;
using Android.Bluetooth;
using System.Threading;
using BalizaFacil.Utils;
using Android.Bluetooth.LE;
using System.Collections.Generic;
using System.Diagnostics;
using BalizaFacil.Core;
using BalizaFacil.Droid.Services.Bluetooth;

[assembly: Dependency(typeof(BluetoothService))]
namespace BalizaFacil.Droid.Services
{
    public class BluetoothService : IBluetoothService
    {
        public event Action<BluetoothDeviceInfo> DeviceDiscovered;
        private BluetoothManager BluetoothManager { get; set; }
        private SensorGATTCallback SensorGATTCallback { get; set; }

        private SensorTagGATTCallback SensorTagGATTCallback { get; set; }
        private ScanCallback ScanCallback { get; set; }
        private BluetoothDevice Device { get; set; }

        public bool IsEnabled
        {
            get
            {
                try
                {
                   return BluetoothManager.Adapter.IsEnabled;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static List<BluetoothDevice> _devices { get; set; } = new List<BluetoothDevice>();

        public BluetoothService()
        {
            bluetoothActivate();
        }

        public void bluetoothActivate()
        {
            try
            {

                BluetoothManager = (BluetoothManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.BluetoothService);
                ScanCallback = new ScanCallback();
                ScanCallback.DeviceDiscovered += OnDeviceDiscovered;

                SensorGATTCallback = new SensorGATTCallback();
                SensorTagGATTCallback = new SensorTagGATTCallback();
                //Desparea todos os dispositivos conectados no celular
                    //UnPairedDevices();
                    //Thread.Sleep(100);
                    //PairDevice();
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }



        public void UnPairedDevices()
        {
            BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            _devices = new List<BluetoothDevice>();
            foreach (var bd in bluetoothAdapter.BondedDevices)
            {
                if (bd.Name != Sensor.DefaultName)
                {
                    _devices.Add(bd);
                    BluetoothDevice bluetoothDevice = bluetoothAdapter.GetRemoteDevice(bd.Address);
                    var mi = bluetoothDevice.Class.GetMethod("removeBond", null);
                    mi.Invoke(bluetoothDevice, null);
                }
            }
        }

        public void pairDevice()
        {
            List<BluetoothDevice> devices = _devices;
            BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            foreach (var bd in devices)
            {
                devices.Add(bd);
                BluetoothDevice bluetoothDevice = bluetoothAdapter.GetRemoteDevice(bd.Address);
                var mi = bluetoothDevice.Class.GetMethod("createBond", null);
                mi.Invoke(bluetoothDevice, null);

            }

        }

        public void ConnectToSensor(string address, bool retryConnection = false)
        {
            try
            {
                if (Device is null)
                    Device = BluetoothManager.Adapter.GetRemoteDevice(address);

                Thread ReceiveSensorDataThread = new Thread(() =>
                {
                    try
                    {
                        if (!IsEnabled)
                            return;


                        Device.ConnectGatt(Android.App.Application.Context, retryConnection, FlowManager.Instance.UseSensorBaliza ? SensorGATTCallback as BluetoothGattCallback : SensorTagGATTCallback);
                    }
                    catch (System.Exception ex)
                    {
                        string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                        BalizaFacil.App.Instance.UnhandledException(title, ex);
                    }
                })
                {
                    Priority = ThreadPriority.Highest,
                    IsBackground = true,
                    Name = nameof(ReceiveSensorDataThread)
                };
                ReceiveSensorDataThread.Start();
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void StartDiscoverDevices()
        {
            Thread DiscoverThread = new Thread(() =>
            {
                try
                {
                    ScanSettings settings = new ScanSettings.Builder().SetScanMode(Android.Bluetooth.LE.ScanMode.LowLatency).Build();

                    var item = new ScanFilter.Builder().SetServiceUuid(Android.OS.ParcelUuid.FromString(BluetoothSensorTagAttributes.ClientConfigurationDescriptor.ToString())).Build();

                    var scanFilter = new List<ScanFilter>() { };

                    BluetoothManager.Adapter.BluetoothLeScanner.StartScan(scanFilter, settings, ScanCallback);

                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }
            })
            {
                IsBackground = true,
                Priority = ThreadPriority.Highest,
                Name = nameof(DiscoverThread)
            };
            DiscoverThread.Start();
        }
        private void OnDeviceDiscovered(Android.Bluetooth.LE.ScanResult obj)
        {
            try
            {
                //Debug.Write(obj.Rssi);

                //if (obj.Rssi >= -50)
                //{
                    DeviceDiscovered?.Invoke(new BluetoothDeviceInfo() { Name = obj.Device.Name, Address = obj.Device.Address });
                //}
                //else
                //{
                //    DeviceDiscovered?.Invoke(new BluetoothDeviceInfo() { Name = obj.Device.Name, Address = obj.Device.Address });
                //}
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void StopDiscoverDevices()
        {
            try
            {
                BluetoothManager.Adapter.BluetoothLeScanner.StopScan(ScanCallback);
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void Enable()
        {
            try
            {
                BluetoothManager.Adapter.Enable();
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void CloseConnection()
        {
            try
            {
                //Device.Dispose();
                if (!FlowManager.Instance.UseSensorBaliza)
                    SensorTagGATTCallback.Disconnect();
                else
                    SensorGATTCallback.Disconnect();
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void ResetSensorPeriod()
        {
            if (!FlowManager.Instance.UseSensorBaliza)
                SensorTagGATTCallback.ResetSensorPeriod();
            else
                SensorGATTCallback.ResetSensorPeriod();
        }
    }
}

