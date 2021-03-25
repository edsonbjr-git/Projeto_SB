using System;
using Android.Bluetooth.LE;
using Android.Runtime;

namespace BalizaFacil.Droid.Services
{
    public class ScanCallback : Android.Bluetooth.LE.ScanCallback
    {
        public event Action<ScanResult> DeviceDiscovered;

        public override void OnScanResult([GeneratedEnum] ScanCallbackType callbackType, ScanResult result)
        {
            try
            {
                DeviceDiscovered?.Invoke(result);
                base.OnScanResult(callbackType, result);
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

    }
}