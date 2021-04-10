using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using BalizaFacil.Core;
using BalizaFacil.Models;
using BalizaFacil.Services;
using BalizaFacil.UI;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class StepViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;



        #region View Properties
        private double _distance { get; set; }

        public double warningAdvancedOnMeters2 { get; set; }
        public double warningAdvancedOnMeters { get; set; }
        private double distance
        {
            get => _distance;

            set
            {
                _distance = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Progress));
                OnPropertyChanged(nameof(DistanceLeft));

                if (FinalizeStepCalled)
                    return;

                double alpha = 0; // 0.85;
                double ReactionTime_s = Storage.reactionTime;
                FilteredSpeed = alpha * FilteredSpeed + (1 - alpha) * Speed;

                warningAdvancedOnMeters2 = Math.Abs(Speed) > 1 ? ((1 * ReactionTime_s) * Math.Abs(Speed)/3.6) : 0.0 * MarginHit; //Removendo 19.5 pq para v - 0 estava dando avanços nao fisicos
                warningAdvancedOnMeters = 0 ;// Math.Abs(Speed) > 1 ? (20 * Math.Abs(Speed)) : 0.0 * MarginHit; //Removendo 19.5 pq para v - 0 estava dando avanços nao fisicos
                                             //Criar variavel para substituir 33.2 e colocar no configurationsView

                //warningAdvancedOnMeters2 = 0;
                //if (Math.Abs(Speed) > 1)
                //warningAdvancedOnMeters2 = (Storage.valueSpeed * Math.Abs(Speed));
                //else
                //    warningAdvancedOnMeters2 = 0.0 * MarginHit; //Removendo 19.5 pq para v - 0 estava dando avanços nao fisicos
                

                if (warningAdvancedOnMeters < 0)
                    warningAdvancedOnMeters = 0;

                if (Math.Abs(DistanceLeft) > 0 && Math.Abs(DistanceLeft) < totalDistance)
                {
                    if (Storage.FinalStepFree && CurrentStep == (ApplicationStep)10 + FlowManager.totalSteps)
                        SoundService.StopBeep();
                    else
                    {
                        if (Math.Abs(DistanceLeft) > 140 + warningAdvancedOnMeters && SoundService.Beeping)
                            SoundService.StopBeep();
                        if (Math.Abs(DistanceLeft) <= 140 + warningAdvancedOnMeters && !SoundService.Beeping)
                            SoundService.StartBeep(BeepSpeed.Slow);
                        else if (DistanceLeft <= 120 + warningAdvancedOnMeters)
                            SoundService.ChangeBeepSpeed(BeepSpeed.Slow);

                        if (Math.Abs(DistanceLeft) <= 110 + warningAdvancedOnMeters)
                            SoundService.ChangeBeepSpeed(BeepSpeed.Normal);

                        if (Math.Abs(DistanceLeft) <= 80 + warningAdvancedOnMeters)
                            SoundService.ChangeBeepSpeed(BeepSpeed.Fast);

                        if (Math.Abs(DistanceLeft) <= 50 + warningAdvancedOnMeters)
                            SoundService.ChangeBeepSpeed(BeepSpeed.VeryFast);
                        if (Math.Abs(DistanceLeft) <= MarginHit && (CurrentStep == ApplicationStep.ManeuverI || CurrentStep == ApplicationStep.ManeuverII))
                            SoundService.ChangeBeepSpeed(BeepSpeed.InsideGreen);
                    }
                }

            }
        }
        DateTime timeLastMove { get; set; }
        DateTime timeLastMove_or_OutGreen { get; set; }
        DateTime timeLastPlayStop { get; set; }
        DateTime timeLastHide_isPassedScreen { get; set; }
        
        private double _totalDistance { get; set; }
        private double totalDistance
        {
            get
            {
                return _totalDistance;
            }
            set
            {
                _totalDistance = value;
                OnPropertyChanged("Progress");
            }
        }

        
        public int countBackWay { get; set; }//Tirar essa variavel
        public static bool TrainingMode = false;
        IStorageService Storage => ServicesManager.Instance.Storage;
        public bool BackMode = false;
        ReportsViewModel historicModel => ReportsViewModel.Instance;
        Reports historic => Reports.Instance;
        public double BiggestDistance { get; set; }
        public double MinimumDistanceOnWrongWay { get; set; }
        public double Progress => distance * 100 / totalDistance;
        public double Speed => DistanceManager.Instance.CurrentSpeed;

        public double DistanceLeft => Math.Round(totalDistance - distance, 0);
        public double distancia => distance;
        public AccelerationDirection Acceleration { get; set; }
        public AccelerationDirection directionAcceleration { get; set; }
        public SteeringWheel Direction { get; set; }
        public ApplicationStep CurrentStep { get; set; }
        public ApplicationStep StepAtual { get; set; }
        public bool isPassed = false;
        #endregion

        #region Commands
        public ICommand CurbColision { get; internal set; }
        public ICommand Cancel { get; internal set; }
        public ICommand NextRound { get; internal set; }
        public ICommand NextStep { get; internal set; }
        #endregion
        public BaseContentPage MainPage { get; set; }

        #region Statistic Properts
        public static float parou1 = 0;
        public static double velocidade = 0;
        public static double FilteredSpeed = 0;
        public static bool curbColisionTrue = false;
        public static double DistanciaTotal { get; set; }
        public double maxSpeed = 0f;

        #endregion

        #region Modal Properties 

        public StepModal ModalType { get; set; } = StepModal.None;
        public DateTime CurbColisionTime { get; private set; }
        public DateTime SpeedChangedTime { get; private set; }
        private double DistanceOnCurbTouch { get; set; }
        public StepModal CurrentModal { get; set; } = StepModal.None;

        #endregion

        private ISoundPlayerService SoundService { get; set; }
        private bool FinalizeStepCalled { get; set; }
        private bool BackStepCalled = false;
        public int TotalSteps { get; internal set; }
        public StepView View { get; set; }
        public int Step { get; internal set; }
        public bool Rounded { get; set; }
        public static int MarginHit = 20;
        public static int Margin = 0;
        public int countStop = 0;
        //public int Stopeds = 1;

        #region Modals
        void ShowModal(StepModal type)
        {
            try
            {
                bool alreadyConfigured = ModalType.HasFlag(type);

                if (alreadyConfigured)
                    return;

                ModalType = ModalType != 0 ? ModalType | type : type;

                Device.BeginInvokeOnMainThread(_ShowModal);
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        void _ShowModal()
        {
            try
            {
                StepModal typeToShow = StepModal.None;

                if (ModalType.HasFlag(StepModal.Conclusion))
                    typeToShow = StepModal.Conclusion;
                else if (ModalType.HasFlag(StepModal.SensorDisconnected))
                    typeToShow = StepModal.SensorDisconnected;
                else if (ModalType.HasFlag(StepModal.CurbTouch))
                    typeToShow = StepModal.CurbTouch;
                else if (ModalType.HasFlag(StepModal.WrongWay))
                    typeToShow = StepModal.WrongWay;
                else if (ModalType.HasFlag(StepModal.LeftWay))
                    typeToShow = StepModal.LeftWay;
                else if (ModalType.HasFlag(StepModal.RightWay))
                    typeToShow = StepModal.RightWay;
                else if (ModalType.HasFlag(StepModal.OkaySpot))
                    typeToShow = StepModal.OkaySpot;
                else if (ModalType.HasFlag(StepModal.FinalizedStep))
                    typeToShow = StepModal.FinalizedStep;

                if (typeToShow == StepModal.None)
                {
                    BaseContentPage.Instance.HidePopup();
                }
                else
                {
                    BaseContentPage.Instance.PopupContainer.BackgroundColor = Color.FromRgba(0, 0, 0, typeToShow == StepModal.Conclusion ? 0.85 : 0);
                    BaseContentPage.Instance.ShowPopup(View.ModalContent[typeToShow]);
                }

            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        void HideModal(StepModal type)
        {
            ModalType = ModalType ^ type;
            Device.BeginInvokeOnMainThread(_ShowModal);
        }
        #endregion
        public bool curbTouch { get; set; }

        public StepViewModel(AccelerationDirection acceleration, SteeringWheel direction, ApplicationStep currentStep, double distance, int totalSteps, int step, StepView view)
        {
            countStop = 0;
            countBackWay = 0;
            TrainingMode = Storage.TrainingMode;
            curbColisionTrue = false;
            curbTouch = false;
            Acceleration = acceleration;
            directionAcceleration = acceleration;
            Direction = direction;
            CurrentStep = currentStep;
            StepAtual = currentStep;
            totalDistance = distance;
            DistanciaTotal = distance;
            TotalSteps = totalSteps;
            timeLastPlayStop = DateTime.Now;
            View = view;
            Step = step;

            Rounded = false;
            SoundService = ServicesManager.Instance.SoundPlayer;
            DistanceManager.Instance.ResetDistances();
            DistanceManager.Instance.DistanceChanged += OnDistanceChanged;
            if (CurrentStep == ApplicationStep.ManeuverIII && !BackStepCalled)
                DistanceManager.Instance.SpeedChanged += OnSpeedChanged;

            Sensor.Instance.StatusChanged += OnSensorStatusChanged;

            MarginHit = (int)(totalDistance * 0.4);
            if (MarginHit > 20)
                MarginHit = 20;
            Margin = CurrentStep == ApplicationStep.ManeuverI || CurrentStep == ApplicationStep.ManeuverII ? MarginHit : 0;

            // sergio, AjusteMov, Ajuste verde todas etapas no modo treinamento, 07/03/2021
            if (StepViewModel.TrainingMode)
                Margin = MarginHit;

            CurbColision = new Command(() =>
            {
                if (CurrentStep == ApplicationStep.ManeuverIII)
                    DistanceManager.Instance.SpeedChanged -= OnSpeedChanged;
                Sensor.Instance.StatusChanged -= OnSensorStatusChanged;
                DistanceManager.Instance.DistanceChanged -= OnDistanceChanged;


                Thread CurbColisionThread = new Thread(() =>
                {
                    try
                    {
                        if (FlowManager.Instance.OnCurbColision())
                        {
                            HideModal(StepModal.CurbTouch);
                            ShowModal(StepModal.Conclusion);
                            Thread.Sleep(200);
                            SoundService.StopBeep();
                            Thread.Sleep(400);
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                try
                                {
                                    FlowManager.Instance.ChangeStep((ApplicationStep)(CurrentStep + 1));
                                }
                                catch (System.Exception ex)
                                {
                                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                                }
                            });
                        }
                        else
                        {
                            HideModal(StepModal.CurbTouch);
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                try
                                {
                                    SoundService.StopBeep();
                                    curbColisionTrue = true;
                                    FlowManager.Instance.ChangeStep(ApplicationStep.Conclusion);
                                    SoundService.StopSound();
                                    SoundService.PlaySound(VoiceType.Insuficient);
                                }
                                catch (System.Exception ex)
                                {
                                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                                }
                            });
                        }
                    }
                    catch (System.Exception ex)
                    {
                        string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                        BalizaFacil.App.Instance.UnhandledException(title, ex);
                    }
                });
                CurbColisionThread.Priority = ThreadPriority.Highest;
                CurbColisionThread.IsBackground = true;
                CurbColisionThread.Name = nameof(CurbColisionThread);
                CurbColisionThread.Start();
            });

            Cancel = new Command(() =>
            {
                historic.completed += "Não completado\n";
                historicModel.GetHistorical();
                historic.parkingTime = DateTime.Now.ToString();
                historic.timeEnd = DateTime.Now;
                Console.WriteLine($"timeEnd {historic.timeEnd}"); // sergio, AjusteMov
                historicModel.reports.Add(historic);
                historicModel.SaveHistorical();
                DistanceManager.Instance.DistanceChanged -= OnDistanceChanged;
                if (CurrentStep == ApplicationStep.ManeuverIII)
                    DistanceManager.Instance.SpeedChanged -= OnSpeedChanged;
                Sensor.Instance.StatusChanged -= OnSensorStatusChanged;
                SoundService.StopSound();
                SoundService.StopBeep();
                historic.Reset();
                FlowManager.Instance.Reset();
                FlowManager.Instance.ChangeStep(ApplicationStep.ChooseDirection);
            });

            NextRound = new Command(() =>
            {
                Rounded = (DistanceLeft < 25 && DistanceLeft > -15) ? true : FlowManager.CurrentStep1 == ApplicationStep.ManeuverIII ? true : false;
            });

            NextStep = new Command(() =>
            {
                FinalizeThreadStep();
            });
        }


        public VoiceType AudioGoBackLeft
        {
            get
            {
                switch (Acceleration)
                {
                    case AccelerationDirection.Backward:
                        return VoiceType.GoForward;
                    case AccelerationDirection.Forward:
                        return VoiceType.GoBackward;
                }
                return VoiceType.GoForward;
            }
        }
        public VoiceType AudioGoBackRight
        {
            get
            {
                switch (Acceleration)
                {
                    case AccelerationDirection.Backward:
                        return VoiceType.GoBackward;
                    case AccelerationDirection.Forward:
                        return VoiceType.GoForward;
                }
                return VoiceType.GoForward;
            }
        }
        //[guilherme - sergio] teste para repetir comando
        private VoiceType VoiceTypeTestStop
        {
            get
            {
                // [sergio] foi necessario criar o acceleration_aux porque a direcao do audio deve ser trocada quando passa do 100%
                AccelerationDirection acceleration_aux = Acceleration;
                if (DistanceLeft < 0)
                {
                    if (Acceleration == AccelerationDirection.Forward)
                        acceleration_aux = AccelerationDirection.Backward;
                    else
                        acceleration_aux = AccelerationDirection.Forward;
                }


                switch (Direction)
                {
                    case SteeringWheel.CentrallyAligned:
                        if (acceleration_aux == AccelerationDirection.Forward && CurrentStep == ApplicationStep.ManeuverI)
                            return VoiceType.CentralizeForward;
                        else if (acceleration_aux == AccelerationDirection.Forward)
                            return VoiceType.CentralizeForward;
                        else
                            return VoiceType.CentralizeBackward; // sergio, teste audio VoiceType.CentralizeBackward1;

                    case SteeringWheel.TotallyLeft:
                        if (acceleration_aux == AccelerationDirection.Forward)
                            return VoiceType.ForwardLeft;
                        else
                            return VoiceType.BackwardLeft;

                    case SteeringWheel.TotallyRight:
                        if (acceleration_aux == AccelerationDirection.Forward)
                            return VoiceType.ForwardRight;
                        else
                            return VoiceType.BackwardRight;
                }
                return VoiceType.Stop;
            }
        }
        private VoiceType VoiceLastStep
        {
            get
            {
                if (Direction == SteeringWheel.TotallyLeft)
                    return VoiceType.LastStepLeft;
                else
                    return VoiceType.LastStepRight;
            }
        }
        private VoiceType VoiceTypeSimple
        {
            get
            {
                if (Progress < 10)
                {
                    return VoiceTypeTestStop;
                }

                AccelerationDirection acceleration_aux = Acceleration;
                if (DistanceLeft < 0)
                {
                    if (Acceleration == AccelerationDirection.Forward)
                        acceleration_aux = AccelerationDirection.Backward;
                    else
                        acceleration_aux = AccelerationDirection.Forward;
                }

                switch (acceleration_aux)
                {
                    case AccelerationDirection.Backward:
                        if (Math.Abs(DistanceLeft) > 70)
                            return VoiceType.ContinueBackward;
                        if (Math.Abs(DistanceLeft) > 40)
                            return VoiceType.ArrivingBackward_short;
                        countStop = 0;
                        return VoiceType.JustOneStepBack;
                    case AccelerationDirection.Forward:
                        if (Math.Abs(DistanceLeft) > 70)
                            return VoiceType.ContinueForward;
                        if (Math.Abs(DistanceLeft) > 40)
                            return VoiceType.ArrivingForward_short;
                        countStop = 0; 
                        return VoiceType.JustOneStepForward;

                }
                countStop = 0;
                return VoiceType.ArrivingForward_short;
            }
        }

        private void OnSensorStatusChanged(SensorStatus status)
        {
            if (status != SensorStatus.Connected && !ModalType.HasFlag(StepModal.SensorDisconnected))
            {
                
                ShowModal(StepModal.SensorDisconnected);
                Thread.Sleep(2000);
                FlowManager.Instance.Reset();

                BalizaFacil.App.Instance.Display("Problema com o Sensor", "Sensor desconectado");
            }
            else if (ModalType.HasFlag(StepModal.SensorDisconnected))
                HideModal(StepModal.SensorDisconnected);
        }

        public void OnDistanceChanged(double distance)
        {
            try
            {

                //Console.Write("Speed/Distance: " + Math.Abs(Speed) +" / "+ distance + "\n\n");
                //Debug.Print("Speed/Distance: " + Math.Abs(Speed) + " / " + distance + "\n\n");
                if (Math.Abs(this.Speed) > Math.Abs(maxSpeed))  //sergio, AjusteMov
                    //if (this.Speed > maxSpeed)
                {
                    maxSpeed = Speed;
                }
                //Console.WriteLine($"Speed {Speed}, maxSpeed {maxSpeed}, CurrentSpeed {DistanceManager.Instance.CurrentSpeed}"); // sergio, AjusteMov

                if (Acceleration == AccelerationDirection.Backward)
                    distance *= -1;

                this.distance = distance;

                if (this.distance > BiggestDistance)
                    BiggestDistance = distance;

                

                if (Storage.FinalStepFree && CurrentStep == (ApplicationStep)10 + FlowManager.totalSteps)
                {
                    SoundService.PlaySound(VoiceLastStep, true);

                    ShowModal(StepModal.FinalizedStep);

                    if (Math.Abs(DistanceLeft) <= Margin && !FinalizeStepCalled)
                    {
                        FinalizeThreadStep();
                    }
                }
                else
                {
                    #region Modals(Open and Close)
                    if (ModalType.HasFlag(StepModal.WrongWay) && distance < MinimumDistanceOnWrongWay && Progress > 5)
                        MinimumDistanceOnWrongWay = distance;

                    //Wrong
                    if (BiggestDistance - distance >= 15 && !ModalType.HasFlag(StepModal.WrongWay) && DistanceLeft > 0 && !isPassed)
                    {
                        BiggestDistance = distance + ((BiggestDistance - distance) / 2);
                        MinimumDistanceOnWrongWay = distance;
                        SoundService.StopSound();
                        SoundService.PlaySound(VoiceType.WrongWay);
                        ShowModal(StepModal.WrongWay);

                    }
                    if (ModalType.HasFlag(StepModal.WrongWay) && (BiggestDistance - distance <= 3 || distance - MinimumDistanceOnWrongWay >= 3))
                    {
                        BiggestDistance = distance;
                        MinimumDistanceOnWrongWay = distance;
                        HideModal(StepModal.WrongWay);
                    }

                    if (CurrentStep == ApplicationStep.ManeuverI || CurrentStep == ApplicationStep.ManeuverII)
                    {
                        //Passou do ponto 1
                        if (DistanceLeft <= -Margin && !ModalType.HasFlag(StepModal.LeftWay) && DistanceLeft > -500 && (DateTime.Now.Subtract(timeLastHide_isPassedScreen).TotalSeconds > 2 || Math.Abs(DistanceLeft) > MarginHit + 10))
                        {
                            FlowManager.isPlayed2 = false;

                            isPassed = true;
                            directionAcceleration = directionAcceleration == AccelerationDirection.Backward ? AccelerationDirection.Forward : AccelerationDirection.Backward;

                            if (countBackWay < 400)//&& DistanceLeft > -30)
                            {
                                SoundService.StopBeep();
                                SoundService.PlaySound(AudioGoBackLeft);
                                countBackWay++;
                            }
                            ShowModal(StepModal.LeftWay);
                            countStop = 0;
                        }
                        if (ModalType.HasFlag(StepModal.LeftWay) && (DistanceLeft > -Margin || DistanceLeft < -500))
                        {
                            HideModal(StepModal.LeftWay);
                        }

                        //Passou do ponto 2
                        if (DistanceLeft >= Margin + 2 && !ModalType.HasFlag(StepModal.RightWay) && isPassed && (DateTime.Now.Subtract(timeLastHide_isPassedScreen).TotalSeconds > 2 || Math.Abs(DistanceLeft) > MarginHit + 10))
                        {
                            FlowManager.isPlayed2 = false;

                            directionAcceleration = directionAcceleration == AccelerationDirection.Backward ? AccelerationDirection.Forward : AccelerationDirection.Backward;
                            ShowModal(StepModal.RightWay);
                            if (countBackWay < 400)
                            {
                                //Mudar audio
                                //Console.WriteLine("xxx");
                                SoundService.StopBeep();
                                SoundService.PlaySound(AudioGoBackRight);
                                countStop = 0;
                                countBackWay++;
                            }
                        }
                        if (ModalType.HasFlag(StepModal.RightWay) && DistanceLeft < Margin)
                        {
                            HideModal(StepModal.RightWay);
                        }
                    }
                    //CurbTouch
                    if (ModalType.HasFlag(StepModal.CurbTouch) && (DateTime.Now.Subtract(CurbColisionTime).TotalSeconds >= 4) && (distance - DistanceOnCurbTouch >= 3))
                    {
                        HideModal(StepModal.CurbTouch);
                        curbTouch = false;
                    }

                    //Area verde
                    if (!Storage.TrainingMode && CurrentStep == ApplicationStep.ManeuverI && CurrentStep == ApplicationStep.ManeuverII)
                    {
                        if ((Math.Abs(DistanceLeft) <= Margin) && !ModalType.HasFlag(StepModal.OkaySpot)) // && DateTime.Now.Subtract(timeLastTwinkle).TotalSeconds > 0.1)
                        {
                            ShowModal(StepModal.OkaySpot);

                            //timeLastTwinkle = DateTime.Now;

                        }
                        if (ModalType.HasFlag(StepModal.OkaySpot) && (Math.Abs(DistanceLeft) > Margin))
                        {
                            HideModal(StepModal.OkaySpot);
                        }
                    }
                    

                    #endregion


                    if (Math.Abs(DistanceLeft) < Margin * 0.8)
                        timeLastHide_isPassedScreen = DateTime.Now; // reseta toda vez que cruza o 100%

                    if (DateTime.Now.Subtract(timeLastPlayStop).TotalSeconds > 0.5)
                    {
                        if ((Math.Abs(DistanceLeft) < Math.Abs(warningAdvancedOnMeters2)) && Math.Abs(DistanceLeft) < 50 && Progress > 10)
                        {
                            if (countStop < int.Parse(Storage.TotalStops))
                            {
                                SoundService.PlaySound(VoiceType.Stop, true);
                                timeLastPlayStop = DateTime.Now;
                                countStop++;
                            }
                        }
                        else if ((Math.Abs(DistanceLeft) < Math.Abs(warningAdvancedOnMeters2)) && Math.Abs(DistanceLeft) > 50 && Progress > 10)
                        {
                            if (countStop < int.Parse(Storage.TotalStops))
                            {
                                SoundService.PlaySound(VoiceType.Stop, true);
                                timeLastPlayStop = DateTime.Now;
                                countStop++;
                            }
                        }
                    }
                    if (carStoped_long() && Math.Abs(DistanceLeft) > Margin && (CurrentStep != ApplicationStep.ManeuverIII || (Progress < 59 || Progress > 100)))
                    {
                        SoundService.PlaySound(VoiceTypeSimple, 2000, CurrentStep);//,true);
                    }

                    if (CurrentStep == ApplicationStep.ManeuverI || CurrentStep == ApplicationStep.ManeuverII)
                    {
                        if (Math.Abs(DistanceLeft) <= Margin && !FinalizeStepCalled && carStoped_green())
                        {
                            FinalizeThreadStep();
                        }
                    }
                    else
                    {
                        if (DistanceLeft <= Margin && !FinalizeStepCalled)
                        {
                            FinalizeThreadStep();
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public bool carStoped_green()
        {
            if (Math.Abs(DistanceManager.Instance.CurrentSpeed) >= 1 || Math.Abs(DistanceLeft) > Margin)
            {
                timeLastMove_or_OutGreen = DateTime.Now;
            }
            if (DateTime.Now.Subtract(timeLastMove_or_OutGreen).TotalSeconds > 1.0 && Math.Abs(DistanceManager.Instance.CurrentSpeed) < 0.05)
            {
                timeLastMove_or_OutGreen = DateTime.Now;
                return true;
            }
            else
                return false;
        }
        public bool carStoped_long()
        {
            if (Math.Abs(DistanceManager.Instance.CurrentSpeed) >= 0.54)
            {
                timeLastMove = DateTime.Now;

                if (isPassed)
                    FlowManager.delay = 1; // Necessario para o audio da repeticao por estar parado na atropelar o audio do "voce passou do ponto.."
                else
                    FlowManager.delay = 1;
            }
            if (DateTime.Now.Subtract(timeLastMove).TotalSeconds > FlowManager.delay)
            {
                timeLastMove = DateTime.Now;
                FlowManager.delay = 7;
                return true;
            }
            else
                return false;
        }

        private void HistoricSave()
        {
                    
/*        // Inicio - variaveis para firebase

            // Parametros associados a baliza como um todo
            public string completed = "";
            public DateTime timeStart;
            public DateTime timeEnd { get; set; }
            public string parkingTime = "";
            // Parametros associados a baliza como um todo


            // Parametros gravados para cada uma das 6 etapas
            public double[] diffDistance;
            public double[] maxSpeed;

            public double[] ElapsedTimeStep;
            public double[] StepEndSpeed;
            public double[] StepInitialSpeed;
            public double[] CurbTouch;
            public string[] dummie_str1;
            public string[] dummie_str2;
            public string[] dummie_str3;
            public double[] dummie_double1;
            public double[] dummie_double2;
            public double[] dummie_double3;
            public DateTime[] dummie_time1;
            // Parametros gravados para cada uma das 6 etapas

        // Fim - variaveis para firebase*/


        historic.diffDistance[(int)(CurrentStep - 10)] = DistanceLeft;
        historic.maxSpeed[(int)(CurrentStep - 10)] = maxSpeed;


        historic.ElapsedTimeStep[(int)(CurrentStep - 10)] = 10.0;
        historic.StepEndSpeed[(int)(CurrentStep - 10)] = 11.0;
        historic.StepInitialSpeed[(int)(CurrentStep - 10)] = 12;
        historic.CurbTouch[(int)(CurrentStep - 10)] = 3;
        historic.dummie_str1[(int)(CurrentStep - 10)] = "str1";
        historic.dummie_str2[(int)(CurrentStep - 10)] = "str2";
        historic.dummie_str3[(int)(CurrentStep - 10)] = "str3";
        historic.dummie_double1[(int)(CurrentStep - 10)] = 91;
        historic.dummie_double2[(int)(CurrentStep - 10)] = 92;
        historic.dummie_double3[(int)(CurrentStep - 10)] = 93;
        historic.dummie_time1[(int)(CurrentStep - 10)] = DateTime.Now;


        //parou1 = DistanceLeft;
        if (CurrentStep + 1 == ApplicationStep.Conclusion)
        {
            historicModel.GetHistorical();
            historic.parkingTime = DateTime.Now.ToString();
            historic.timeEnd = DateTime.Now;
            Console.WriteLine($"timeEnd {historic.timeEnd}"); // sergio, AjusteMov
            historicModel.reports.Add(historic);
            historicModel.SaveHistorical();
            // enviar para firebase daqui

        }
        }

        private void FinalizeThreadStep()
        {
            FinalizeStepCalled = true;
            Thread FinalizeStepThread = new Thread(FinalizeStep);
            FinalizeStepThread.Name = nameof(FinalizeStepThread);
            FinalizeStepThread.Priority = ThreadPriority.AboveNormal;
            FinalizeStepThread.IsBackground = true;

            FinalizeStepThread.Start();
        }



        private void OnSpeedChanged(double speed)
        {
            if (Progress >= 60 && DistanceLeft > Margin && !ModalType.HasFlag(StepModal.CurbTouch) && speed >= 0 && !isPassed)
            {
                curbTouch = true;
                CurbColisionTime = DateTime.Now;
                DistanceOnCurbTouch = distance;
                ShowModal(StepModal.CurbTouch);
                SoundService.StopSound();
                SoundService.PlaySound(VoiceType.CurbTouch_complete);
            }
        }

        private void FinalizeStep()
        {
            try
            {
                SoundService.StopBeep();
                
                DistanceManager.Instance.SpeedChanged -= OnSpeedChanged;
                Sensor.Instance.StatusChanged -= OnSensorStatusChanged;
                velocidade = DistanceManager.Instance.CurrentSpeed;
                HistoricSave();
                if (CurrentStep != ApplicationStep.ManeuverI || CurrentStep != ApplicationStep.ManeuverII)
                    Thread.Sleep(50);
                if (TrainingMode == true)
                {
                    while (Rounded == false);

                    if (Rounded == true)
                    {
                        SoundService.PlaySound(VoiceType.ConclusionStep);
                        ShowModal(StepModal.Conclusion);
                        DistanceManager.Instance.DistanceChanged -= OnDistanceChanged;
                        if (CurrentStep != ApplicationStep.ManeuverI || CurrentStep != ApplicationStep.ManeuverII)
                            Thread.Sleep(400);
                        else
                            Thread.Sleep(400);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (CurrentStep == ApplicationStep.ManeuverV && DistanceManager.sizePark == ParkingSpaceSizeType.VeryEasy)
                                FlowManager.Instance.ChangeStep((ApplicationStep)(CurrentStep + 2));
                            else
                                FlowManager.Instance.ChangeStep((ApplicationStep)(CurrentStep + 1));
                        });
                        Rounded = false;
                    }
                }
                else
                {
                    SoundService.PlaySound(VoiceType.ConclusionStep);
                    ShowModal(StepModal.Conclusion);
                    DistanceManager.Instance.DistanceChanged -= OnDistanceChanged;
                    if (CurrentStep != ApplicationStep.ManeuverI || CurrentStep != ApplicationStep.ManeuverII)
                        Thread.Sleep(400);
                    else
                        Thread.Sleep(400);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (CurrentStep == ApplicationStep.ManeuverV && DistanceManager.sizePark == ParkingSpaceSizeType.VeryEasy)
                            FlowManager.Instance.ChangeStep(ApplicationStep.Conclusion);
                        else
                            FlowManager.Instance.ChangeStep((ApplicationStep)(CurrentStep + 1));
                    });
                }
               
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        internal void StepEnded()
        {
            SoundService.StopBeep();
            if (CurrentStep == ApplicationStep.ManeuverIII)
                DistanceManager.Instance.SpeedChanged -= OnSpeedChanged;

            //ServicesManager.Instance.SoundPlayer.StopSound();
            Sensor.Instance.StatusChanged -= OnSensorStatusChanged;
            DistanceManager.Instance.DistanceChanged -= OnDistanceChanged;
        }
    }
}
