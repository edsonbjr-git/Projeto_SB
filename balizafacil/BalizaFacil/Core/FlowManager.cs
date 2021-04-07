using System;
using System.Linq;
using System.Threading;
using BalizaFacil.Models;
using BalizaFacil.Screens;
using BalizaFacil.Services;
using Xamarin.Forms;
using BalizaFacil.Utils;
using BalizaFacil.UI;
using System.Collections.Generic;
//using  Android.Bluetooth;

namespace BalizaFacil.Core
{
    public class FlowManager
    {

        //public static List<BluetoothDevice> devices { get; set; }
        private static FlowManager instance { get; set; }
        public static FlowManager Instance
        {
            get
            {
                if (instance is null)
                    instance = new FlowManager();

                return instance;
            }
        }

        //IStorageService Storage = Services.ServicesManager.Instance.Storage;

        public static VoiceType globalPlaying { get; set; }
        public App App { get; private set; }
        public BaseContentPage MainPage { get; set; }

        
        #region Application Flow Management

        private ApplicationStep CurrentStep { get; set; }
        public ApplicationStep CurrentStepAfterBackStep { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Foreground;
        public static ApplicationStep CurrentStep1;

        public AccelerationDirection acel { get; set; }
        public SteeringWheel direc { get; set; }
        public float dist { get; set; }

        #endregion

        #region Proccess Definitions

        public CarFilterInfo CarBrand { get; set; }
        public CarFilterInfo CarModel { get; set; }
        public CarFilterInfo CarType { get; set; }
        public Car Car { get; set; }

        public double ParkingSpaceSize { get; set; }

        public ParkingSpaceSizeType ParkingSpaceSizeType { get; set; }

        public Direction Direction { get; set; }

        private DistanceManager DistanceManager => DistanceManager.Instance;

        #endregion

        #region Bluetooth Connection Process

        public bool Waiting { get; private set; } = false;
        private Sensor Sensor => Sensor.Instance;

        #endregion
        public bool UseSensorBaliza { get; set; } = false;

        #region Services
        IStorageService Storage => ServicesManager.Instance.Storage;
        ISoundPlayerService Sound => ServicesManager.Instance.SoundPlayer;
        IBluetoothService Bluetooth => ServicesManager.Instance.Bluetooth;
        #endregion

        public void Start(App app)
        {

            //devices = new List<BluetoothDevice>();
            App = app;
            MainPage = BaseContentPage.Instance;
            app.MainPage = MainPage;
            MainPage.BackButtonPressed += OnBackButtonPressed;

            try
            {
                Car = Storage.Car;
            }
            catch(Exception ef)
            {

            }

            globalPlaying = VoiceType.None;
            totalSteps = 5;
            if (Car is null || string.IsNullOrWhiteSpace(Storage.Address))
                ChangeStep(ApplicationStep.Register);
            else
            {
                ChangeStep(ApplicationStep.Welcome);
            }
            
            if (Sensor.Status == SensorStatus.Connected)
                Sound.PlaySound(VoiceType.SensorConnected);


            Sensor.StatusChanged += OnSensorStatusChanged;
        }

        private void OnBackButtonPressed()
        {

            try
            {
                switch (CurrentStep)
                {
                    case ApplicationStep.Register:
                    case ApplicationStep.Welcome:
                        ServicesManager.Instance.Utils.CloseApp();
                        break;

                    case ApplicationStep.ConfigureCarBrand:
                    case ApplicationStep.ConfigureCarModel:
                    case ApplicationStep.ConfigureCarType:
                    case ApplicationStep.ConfigureSensor:
                    case ApplicationStep.ChooseDirection:
                    case ApplicationStep.MeasureSpace:
                    case ApplicationStep.PersonalizeCar:
                        MainPage.PopView();
                        break;
                }
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void ConnectToSensor()
        {

            try
            {
                //Sound.StopSound(); //sergio, melhorar funcionameno no xiaomi
                if (string.IsNullOrWhiteSpace(Storage.Address))
                    return;

                Bluetooth.ConnectToSensor(Storage.Address, true);
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public static bool isPlayed = false;
        public static bool isPlayed2 = false;
        public static int delay = 7;
        public static int totalSteps = 5;
        public void ChangeStep(ApplicationStep step)
        {

            try
            {
                if (DistanceManager.sizePark == ParkingSpaceSizeType.VeryEasy)
                    totalSteps = 4;
                else
                    totalSteps = 5;
                CurrentStep1 = step;
                isPlayed = false;
                isPlayed2 = false;
                delay = 7;
                //Sound.ResetPlayers();

                switch (step)
                {
                    case ApplicationStep.Register:
                        MainPage.PushView(new RegisterView());
                        break;
                    case ApplicationStep.ConfigureCarBrand:
                        MainPage.PushView(new CarBrandView());
                        break;
                    case ApplicationStep.ConfigureSensor:
                        MainPage.PushView(new ConfigureSensorAddressView());
                        this.Car = Storage.Car;
                        break;
                    case ApplicationStep.Welcome:
                        
                        MainPage.PushView(new WelcomeView());
                        break;
                    case ApplicationStep.ChooseDirection:
                        MainPage.PushView(new ChooseDirectionView());
                        break;
                    case ApplicationStep.MeasureSpace:
                        Services.ServicesManager.Instance.Bluetooth.ResetSensorPeriod();
                        //ServicesManager.Instance.SoundPlayer .StopSound();
                        MainPage.PushView(new MeasureSpaceView());
                        break;
                    case ApplicationStep.ManeuverI:

                        // sergio, AjusteMov inicio
                        Reports.Instance.timeStart = DateTime.Now;
                        Console.WriteLine($"timeStart {Reports.Instance.timeStart}"); 
                        Reports.Instance.diffDistance = new double[6];
                        Reports.Instance.maxSpeed = new double[6];
                        //sergio, AjusteMov fim

                        //ServicesManager.Instance.SoundPlayer.StopSound();
                        MainPage.PushView(new StepView(AccelerationDirection.Forward, SteeringWheel.CentrallyAligned, step, 200, totalSteps, 0));
                        break;
                    case ApplicationStep.ManeuverII:
                        MainPage.PushView(new StepView(AccelerationDirection.Forward, SteeringWheel.CentrallyAligned, step, 150, totalSteps, 1));
                        break;
                    case ApplicationStep.ManeuverIII:
                        MainPage.PushView(new StepView(AccelerationDirection.Forward, SteeringWheel.CentrallyAligned, step, 100, totalSteps, 2));
                        break;
                    case ApplicationStep.ManeuverIV:
                        MainPage.PushView(new StepView(AccelerationDirection.Backward, SteeringWheel.CentrallyAligned, step, 200, totalSteps, 3));
                        break;
                    case ApplicationStep.ManeuverV:
                        MainPage.PushView(new StepView(AccelerationDirection.Backward, SteeringWheel.CentrallyAligned, step, 150, totalSteps, 4));
                        break;
                    case ApplicationStep.ManeuverVI:
                        MainPage.PushView(new StepView(AccelerationDirection.Forward, SteeringWheel.CentrallyAligned, step, 100, 5, 5));
                        break;
                    case ApplicationStep.Conclusion:
                        MainPage.PushView(new ConclusionView());
                        break;
                }

                if (step == ApplicationStep.MeasureSpace && Sensor.Status != SensorStatus.Connected)
                    WaitConnection();

                CurrentStep = step;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }

        }

        private void WaitConnection()
        {
            if (!Waiting && Sensor.Status != SensorStatus.Connected)
            {
                Waiting = true;
                var modal = new WaitingSensorConnectionView();
                modal.Cancelled += Reset;
                BaseContentPage.Instance.PushModal(modal);
            }
        }

        public void Reset()
        {

            try
            {
                Sound.StopBeep();
                Waiting = false;
                Direction = Direction.Undefined;
                ParkingSpaceSize = 0;
                
                MainPage.Pages.Clear();
                MainPage.Modals.Clear();
                ServicesManager.Instance.SoundPlayer.StopSound();
                ServicesManager.Instance.SoundPlayer.StopBeep();
                if (Car is null || string.IsNullOrWhiteSpace(Storage.Address))
                {
                    CurrentStep = ApplicationStep.Register;
                    MainPage.PushView(new RegisterView());
                }
                else
                {
                    CurrentStep = ApplicationStep.Welcome;
                    MainPage.PushView(new WelcomeView());
                }
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }

        }

        public void CloseApp()
        {
            ServicesManager.Instance.Utils.CloseApp();
        }

        private void OnSensorStatusChanged(SensorStatus status)
        {

            try
            {
                switch (status)
                {
                    case SensorStatus.Connected:
                        Sound.PlaySound(VoiceType.SensorConnected);

                        if (!Waiting)
                            break;

                        Waiting = false;
                        Device.BeginInvokeOnMainThread(() => { MainPage.PopModal(); });

                        break;
                    case SensorStatus.Disconnected:
                        ConnectToSensor();

                        if (CurrentStep == ApplicationStep.MeasureSpace)
                            Device.BeginInvokeOnMainThread(WaitConnection);

                        break;
                }
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public bool OnCurbColision()
        {
            try
            {
                return DistanceManager.CorrectOnCurbColision(Direction, CurrentStep);
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }

            return false;
        }

    }
}
