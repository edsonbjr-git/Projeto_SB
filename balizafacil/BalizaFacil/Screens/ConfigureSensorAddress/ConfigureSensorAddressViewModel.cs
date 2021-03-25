using BalizaFacil.Core;
using BalizaFacil.Models;
using BalizaFacil.Services;
using BalizaFacil.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class ConfigureSensorAddressViewModel
    {
        public bool CloseOnComplete { get; private set; }
        public ICommand SearchByQRCode { get; internal set; }
        public ICommand SearchByBluetooth { get; internal set; }

        private IBluetoothService bluetooth => ServicesManager.Instance.Bluetooth;
        private IStorageService storage => ServicesManager.Instance.Storage;

        public ConfigureSensorAddressViewModel(bool closeOnComplete)
        {
            CloseOnComplete = closeOnComplete;
            SearchByQRCode = new Command(SearchQRCode);
            SearchByBluetooth = new Command(SearchBluetooth);
        }

        private void SearchBluetooth()
        {
            if (!(string.IsNullOrWhiteSpace(Storage.Address)))
            {
                bluetooth.CloseConnection();
                Storage.Address = "";
                //Storage.Car = null;
                //FlowManager.Instance.Reset();
            }
            bluetooth.bluetoothActivate();
            var waitingView = new WaitingSensorConnectionView();
            waitingView.Cancelled += () => { BaseContentPage.Instance.PopModal(); };
            BaseContentPage.Instance.PushModal(waitingView);

            bluetooth.DeviceDiscovered += OnDeviceDiscovered;
            bluetooth.StartDiscoverDevices();
        }
        IStorageService Storage => ServicesManager.Instance.Storage;

        private void OnDeviceDiscovered(BluetoothDeviceInfo obj)
        {
            if (obj.Name != Sensor.DefaultName)
                return;
            

            storage.Address = obj.Address;

            bluetooth.StopDiscoverDevices();
            bluetooth.DeviceDiscovered -= OnDeviceDiscovered;
            Sensor.Instance.StatusChanged += OnSensorStatusChanged;
            FlowManager.Instance.ConnectToSensor();
            Services.ServicesManager.Instance.SoundPlayer.PlaySound(VoiceType.SensorConnected);
            Task.Delay(5000);
            Services.ServicesManager.Instance.Utils.CloseApp();
            //BaseContentPage.Instance.PopModal();


        }

        private void OnSensorStatusChanged(SensorStatus status)
        {
            if (status == SensorStatus.Connected)
            {
                Sensor.Instance.StatusChanged -= OnSensorStatusChanged;
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (!CloseOnComplete)
                        FlowManager.Instance.Reset();
                    else
                    {
                        BaseContentPage.Instance.PopModal();
                        BaseContentPage.Instance.PopView();
                        //FlowManager.Instance.ChangeStep(ApplicationStep.Welcome);
                    }
                });
            }
        }

        private void SearchQRCode()
        {

            ScanningView scanner = new ScanningView();
            scanner.Scanned += (result) =>
            {
                storage.Address = result;
                Device.BeginInvokeOnMainThread(() =>
                {

                    try
                    {
                        BaseContentPage.Instance.PopView();
                        var waitingView = new WaitingSensorConnectionView();
                        waitingView.Cancelled += BaseContentPage.Instance.PopModal;
                        BaseContentPage.Instance.PushModal(waitingView);
                        Sensor.Instance.StatusChanged += OnSensorStatusChangedQRCode;
                        FlowManager.Instance.ConnectToSensor();
                    }
                    catch (System.Exception ex)
                    {
                        string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                        BalizaFacil.App.Instance.UnhandledException(title, ex);
                    }
                });
            };

            BaseContentPage.Instance.PushView(scanner);
        }

        private void OnSensorStatusChangedQRCode(SensorStatus status)
        {
            if (status == SensorStatus.Connected)
            {
                Sensor.Instance.StatusChanged -= OnSensorStatusChangedQRCode;
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        BaseContentPage.Instance.PopModal();

                        if (CloseOnComplete)
                            BaseContentPage.Instance.PopView();
                        else
                            FlowManager.Instance.Reset();
                    }
                    catch (System.Exception ex)
                    {
                        string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                        BalizaFacil.App.Instance.UnhandledException(title, ex);
                    }
                });
            }
        }
    }
}
