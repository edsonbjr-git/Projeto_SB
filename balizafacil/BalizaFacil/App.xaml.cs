using System;
using Xamarin.Forms;
using BalizaFacil.Core;
using BalizaFacil.Screens;
using System.Threading.Tasks;
using System.Collections.Generic;
using BalizaFacil.Models;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using BalizaFacil.Library;
using BalizaFacil.Services;
using Xamarin.Essentials;
using System.Threading;

namespace BalizaFacil
{
    public partial class App : Application
    {
        public static App Instance { get; private set; }
        public static bool sensorTime = false;
        public static DataStoreContainer DataStoreContainer { get; private set; }

        public static LogBaliza CurrentLogBaliza { get; set; }

        public App()
        {
            App.Instance = this;
           // FlowManager.Instance.Start(this);

           // DataStoreContainer = new DataStoreContainer(new FirebaseAuthService());

        }

        public void UnhandledException(string title, Exception exception)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                Crashes.TrackError(exception);
                // MainPage.DisplayAlert(title, $"{exception?.Message}   (  {exception?.StackTrace}  )", "Close");
                MainPage.DisplayAlert("Erro", exception.Message, "Fechar");
                FlowManager.Instance.Reset();
                FlowManager.Instance.ConnectToSensor();

                //Chamar o metodo para fechar 
                //Thread.Sleep(3000);
                FlowManager.Instance.CloseApp();
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

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
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

        public static bool IsAndroidSDKBelowMarshmallow { get; set; }
        public static bool BluetoothStartedEnabled { get; set; }


        protected override async void OnStart()
        {
            base.OnStart();

            AppCenter.Start("android=77dd3af8-350a-40a8-b8cc-c868fa0a4768;",
                  typeof(Analytics), typeof(Crashes));


            await Permissions.RequestAsync<Permissions.Camera>();

            await Permissions.RequestAsync<Permissions.Flashlight>();

            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();


            FlowManager.Instance.Start(this);
            DataStoreContainer = new DataStoreContainer(new FirebaseAuthService());


            /*Task.Run(() =>
            {

                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                        {
                            await LogAppCenter.TrackEventAsync("App Start", null);
                        });
            
            });*/
        }

    }
}