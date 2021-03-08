using System;
using Xamarin.Forms;
using BalizaFacil.Core;
using BalizaFacil.Screens;
using System.Threading.Tasks;
using System.Collections.Generic;
using BalizaFacil.Models;

namespace BalizaFacil
{
    public partial class App : Application
    {
        public static App Instance { get; private set; }
        public static bool sensorTime = false;
        public App()
        {
            App.Instance = this;
            FlowManager.Instance.Start(this);
            
        }

        public void UnhandledException(string title, Exception exception)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                MainPage.DisplayAlert(title, $"{exception?.Message}   (  {exception?.StackTrace}  )", "Close");
                FlowManager.Instance.Reset();
                FlowManager.Instance.ConnectToSensor();
            });
        }

        public async void Options(List<string> title)
        {
            Task s = null;
            string[] array = title.ToArray();


            string action = await MainPage.DisplayActionSheet("Conexoes", "Close", "", array);
            
        }
        public void Display(string title, string text)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                MainPage.DisplayAlert(title, $"{text}", "Close");
                FlowManager.Instance.Reset();
                FlowManager.Instance.ConnectToSensor();
            });
        }


        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }

        public static int HeightPixels { get; set; }

        public static int WidthPixels { get; set; }

        public static bool IsAndroidSDKBelowMarshmallow { get;  set; }
        public static bool BluetoothStartedEnabled { get; set; }
    }
}
