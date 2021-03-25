using Android.App;
using Android.Views;
using BalizaFacil.Droid.Services;
using BalizaFacil.Services;
using Java.Lang;
using Xamarin.Forms;

[assembly: Dependency(typeof(UtilsService))]
namespace BalizaFacil.Droid.Services
{
    public class UtilsService : IUtilsService
    {
        public void CloseApp()
        {
            try
            {
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void HideStatusBar()
        {
            try
            {
                MainActivity.Instance.Window.AddFlags(WindowManagerFlags.Fullscreen);
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void SetLandscape()
        {
            try
            {
                MainActivity.Instance.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void SetPortrait()
        {
            try
            {
                MainActivity.Instance.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void ShowStatusBar()
        {
            try
            {
                MainActivity.Instance.Window.ClearFlags(WindowManagerFlags.Fullscreen);
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }
    }
}