//using Android.Bluetooth;
using BalizaFacil.Core;
using BalizaFacil.Services;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class ConclusionViewModel
    {
        private IBluetoothService bluetooth => ServicesManager.Instance.Bluetooth;

        public ConclusionViewModel()
        {
            //if (FlowManager.Instance.Status == Models.ApplicationStatus.Background)
            //{
            //    Thread CloseAppTiemout = new Thread(() =>
            //    {

            //        try
            //        {

            //            Thread.Sleep(5000);
            //            if (FlowManager.Instance.Status == Models.ApplicationStatus.Background)
            //            {
            //                Services.ServicesManager.Instance.Utils.CloseApp();
            //                bluetooth.pairDevice();
            //            }
            //        }
            //        catch (System.Exception ex)
            //        {
            //            string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
            //            BalizaFacil.App.Instance.UnhandledException(title, ex);
            //        }
            //    });

            //    CloseAppTiemout.IsBackground = true;
            //    CloseAppTiemout.Priority = ThreadPriority.Highest;
            //    CloseAppTiemout.Name = nameof(CloseAppTiemout);

            //    CloseAppTiemout.Start();
            //}

        CloseApp = new Command(Services.ServicesManager.Instance.Utils.CloseApp);
        }

        public ICommand CloseApp { get; internal set; } = new Command(Services.ServicesManager.Instance.Utils.CloseApp);
        //public ICommand Continue { get; set; } = new Command(() => { FlowManager.Instance.ChangeStep(ApplicationStep.ChooseDirection); });
        
        //sergio, AjusteMov
        public ICommand Continue { get; set; } = new Command(() => { FlowManager.Instance.ChangeStep(ApplicationStep.Welcome); });

    }
}
