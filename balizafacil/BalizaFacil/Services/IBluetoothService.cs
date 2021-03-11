using BalizaFacil.Models;
using System;

namespace BalizaFacil.Services
{
    public interface IBluetoothService
    {
        event Action<BluetoothDeviceInfo> DeviceDiscovered;
        bool IsEnabled { get; }
        void ConnectToSensor(string address, bool retryConnection = false);
        void StartDiscoverDevices();
        void StopDiscoverDevices();
        void Enable();
        void CloseConnection();

        void ResetSensorPeriod();
        void bluetoothActivate();
        void pairDevice();
    }
}
