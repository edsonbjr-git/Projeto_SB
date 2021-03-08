using BalizaFacil.Core;
using BalizaFacil.Models;
using BalizaFacil.Services;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using BalizaFacil.UI;
using BalizaFacil.Utils;
using System;
using System.Diagnostics;

namespace BalizaFacil.Screens
{
    public class MeasureSpaceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region View Properties

        private bool measuringSpace;
        public bool MeasuringSpace
        {
            get => measuringSpace;
            set
            {
                measuringSpace = value;
                OnPropertyChanged();
            }
        }

        private bool insuficientSpace;
        public bool InsuficientSpace
        {
            get => insuficientSpace;
            set
            {
                insuficientSpace = value;
                OnPropertyChanged();
            }
        }

        private string instruction;
        public static bool measuring = false;

        public string Instruction
        {
            get => instruction;
            set
            {
                instruction = value;
                OnPropertyChanged();
            }
        }

        private string instructionGIFSource;
        public string InstructionGIFSource
        {
            get => instructionGIFSource;
            set
            {
                instructionGIFSource = value;
                OnPropertyChanged();
            }
        }


        private string directionInstruction;
        public string DirectionInstruction
        {
            get => directionInstruction;
            set
            {
                directionInstruction = value;
                OnPropertyChanged();
            }
        }


        private double parkingSpaceSize;
        public double ParkingSpaceSize
        {
            get => parkingSpaceSize;
            set
            {
                parkingSpaceSize = value;
                OnPropertyChanged();
            }
        }

        public ICommand MeasureSpaceCommand { get; private set; }
        public ICommand EndMeasurementCommand { get; private set; }
        public ICommand CancelMeasurementCommand { get; internal set; }
        public ICommand JumpMeasurementCommand { get; internal set; }

        #endregion

        private FlowManager FlowManager => FlowManager.Instance;
        private ServicesManager Services => ServicesManager.Instance;

        private string FirstInstruction { get => "Alinhe o retrovisor com\n o para-choque frontal do carro de"; }
        private string SecondInstruction { get => "Alinhe o retrovisor com\n o para-choque traseiro do carro da"; }
        public Progress Progress { get; set; }

        //public double Speed => DistanceManager.Instance.CurrentSpeed;

        public MeasureSpaceViewModel()
        {
            timeLastMove = DateTime.Now;
            ParkingSpaceSize = 0;
            InitialConfiguration();
            DistanceManager.sizePark = ParkingSpaceSizeType.None;
            //Só pra teste DistanceManager.sizePark = ParkingSpaceSizeType.Hard;

            Services.SoundPlayer.StopSound();
            endMeasurement = false;
            timeLastMove = DateTime.Now;
            DistanceManager.Instance.ResetDistances();
            DistanceManager.Instance.DistanceChanged += OnDistanceChanged;

            MeasureSpaceCommand = new Command(MeasureSpace);
            EndMeasurementCommand = new Command(EndMeasurement);
            CancelMeasurementCommand = new Command(CancelMeasurement);
            JumpMeasurementCommand = new Command(JumpMeasureSpace);
            measuring = false;
            if (Sensor.Instance.Status == SensorStatus.Connected)
                Services.SoundPlayer.PlaySound(Models.VoiceType.MeasureSpaceI);
            else
                Sensor.Instance.StatusChanged += PlayInstructionOnSensorConnected;
        }

        private void JumpMeasureSpace()
        {
            measuring = false;
            endMeasurement = true;
            InitialConfiguration();
            DistanceManager.sizePark = ParkingSpaceSizeType.Hard;
            ParkingSpaceSizeType sizeType = DistanceManager.Instance.CalculateParkingSpaceSizeType(Car.ParkingSpace);
            BaseContentPage.Instance.PushModal(new MeasurementResultView(sizeType));
            threadMeasure(sizeType);

        }

        private void CancelMeasurement()
        {
            FlowManager.CurrentStep1 -= 1;
            measuring = false;
            endMeasurement = false;
            App.sensorTime = false;
            DistanceManager.Instance.ResetDistances();
            ServicesManager.Instance.SoundPlayer.StopSound();
            if (MeasuringSpace == true)
                InitialConfiguration();
            else
            {
                FlowManager.Instance.Reset();
                FlowManager.Instance.ChangeStep(ApplicationStep.ChooseDirection);
            }
        }

        private void MeasureSpace()
        {
            measuring = true;
            App.sensorTime = true;
            MeasuringConfiguration();

            endMeasurement = false;
            DistanceManager.Instance.ResetDistances();
            DistanceManager.Instance.DistanceChanged += OnDistanceChanged;
            Services.SoundPlayer.StopSound();
            DistanceManager.sizePark = ParkingSpaceSizeType.None;
            Services.SoundPlayer.PlaySound(Models.VoiceType.MeasureSpaceII);

        }

        void InitialConfiguration()
        {
            Instruction = FirstInstruction;

            InstructionGIFSource = $"measureI_{FlowManager.Instance.Direction.ToString().ToLower()}";
            DirectionInstruction = "Trás";
            MeasuringSpace = false;

            if (Progress != null)
                Progress.CurrentStep = 0;
        }

        void MeasuringConfiguration()
        {
            endMeasurement = false;
            MeasuringSpace = true;
            Instruction = SecondInstruction;
            InstructionGIFSource = $"measureII_{FlowManager.Instance.Direction.ToString().ToLower()}";
            DirectionInstruction = "Frente";

            Progress?.Advance();
        }
        public bool endMeasurement = false;
        private void EndMeasurement()
        {
            endMeasurement = true;
            measuring = false;
            InitialConfiguration();
            ParkingSpaceSizeType sizeType = DistanceManager.Instance.CalculateParkingSpaceSizeType(ParkingSpaceSize,true);
            BaseContentPage.Instance.PushModal(new MeasurementResultView(sizeType));
            threadMeasure(sizeType);

        }

        private void threadMeasure(ParkingSpaceSizeType sizeType)
        {
            Thread DismissThread = new Thread(() =>
            {
                if (sizeType == ParkingSpaceSizeType.Insuficient)
                    Services.SoundPlayer.PlaySound(VoiceType.Insuficient);
                Thread.Sleep(1200);
                Device.BeginInvokeOnMainThread(() =>
                {

                    try
                    {
                        BaseContentPage.Instance.PopModal();

                        if (sizeType == ParkingSpaceSizeType.Insuficient)
                            return;

                        FlowManager.ParkingSpaceSize = ParkingSpaceSize;
                        FlowManager.ParkingSpaceSizeType = sizeType;
                        FlowManager.ChangeStep(ApplicationStep.ManeuverI);
                    }
                    catch (System.Exception ex)
                    {
                        string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                        BalizaFacil.App.Instance.UnhandledException(title, ex);
                    }
                });
            });
            DismissThread.Priority = ThreadPriority.AboveNormal;
            DismissThread.Name = nameof(DismissThread);
            DismissThread.IsBackground = true;

            DismissThread.Start();
        }
        private void PlayInstructionOnSensorConnected(SensorStatus status)
        {
            if (status != SensorStatus.Connected)
                return;
            Services.SoundPlayer.StopSound();
            Services.SoundPlayer.PlaySound(Models.VoiceType.MeasureSpaceI);
        }
        private void OnDistanceChanged(double ParkingSpaceSize)
        {
            this.ParkingSpaceSize = ParkingSpaceSize;
            //Debug.WriteLine(DistanceManager.Instance.CurrentSpeed);
            if (!measuring && ParkingSpaceSize > 10 && DistanceManager.Instance.CurrentSpeed > 3f && FlowManager.CurrentStep1 == ApplicationStep.MeasureSpace)
            {
                Services.SoundPlayer.PlaySound(VoiceType.NotMeasuring, 5000, ApplicationStep.None);//Criar audio "nao iniciou a medicao"
            }
            if (FlowManager.CurrentStep1 == ApplicationStep.MeasureSpace && measuring)
            {
               CountMetters(ParkingSpaceSize);
            
            }

            if (FlowManager.CurrentStep1 == ApplicationStep.MeasureSpace && false && !measuring && (DateTime.Now.Subtract(timeLastMove).TotalSeconds > 5) && !endMeasurement)
            {
                measuring = false;
                resetMetters();
                timeLastMove = DateTime.Now;
                DistanceManager.Instance.ResetDistances();
                DistanceManager.Instance.DistanceChanged += OnDistanceChanged;
            }

        }
        DateTime timeLastMove { get; set; }

            
        private Car Car => FlowManager.Instance.Car;

        private bool measure = false, oneMetter = false, twoMetters = false, threeMetters = false, fourMetters = false, fiveMetters = false, sixMetters = false, sevenMetters = false, over = false;
        public void CountMetters(double ParkingSpaceSize)
        {

            if (FlowManager.CurrentStep1 == ApplicationStep.MeasureSpace && measuring)
            {
                if (ParkingSpaceSize == Car.ParkingSpaceBigger && !over)
                {
                    over = true;
                    Services.SoundPlayer.StopSound();
                    Services.SoundPlayer.PlaySound(Models.VoiceType.ItsOver);
                }
                if (ParkingSpaceSize >= 45  && !measure)
                {
                    Services.SoundPlayer.StopSound();
                    Services.SoundPlayer.PlaySound(Models.VoiceType.Measuring );
                    measure = true;
                }
                else if (ParkingSpaceSize >= 100 && !oneMetter)
                {
                    oneMetter = true;
                    Services.SoundPlayer.StopSound();
                    Services.SoundPlayer.PlaySound(Models.VoiceType.OneMetter);
                }
                else if (ParkingSpaceSize >= 200 && !twoMetters)
                {
                    twoMetters = true;
                    Services.SoundPlayer.StopSound();
                    Services.SoundPlayer.PlaySound(Models.VoiceType.TwoMetters );
                }
                else if (ParkingSpaceSize >= 300 && !threeMetters)
                {
                    threeMetters = true;
                    Services.SoundPlayer.StopSound();
                    Services.SoundPlayer.PlaySound(Models.VoiceType.ThreeMetters);
                }
                else if (ParkingSpaceSize >= 400 && !fourMetters)
                {
                    fourMetters = true;
                    Services.SoundPlayer.StopSound();
                    Services.SoundPlayer.PlaySound(Models.VoiceType.FourMetters);
                }
                else if (ParkingSpaceSize >= 500 && !fiveMetters)
                {
                    fiveMetters = true;
                    Services.SoundPlayer.StopSound();
                    Services.SoundPlayer.PlaySound(Models.VoiceType.FiveMetters);
                }
                else if (ParkingSpaceSize >= 600 && !sixMetters)
                {
                    sixMetters = true;
                    Services.SoundPlayer.StopSound();
                    Services.SoundPlayer.PlaySound(Models.VoiceType.SixMetters);
                }
                else if (ParkingSpaceSize >= 700 && !sevenMetters)
                {
                    sevenMetters = true;
                    Services.SoundPlayer.StopSound();
                    Services.SoundPlayer.PlaySound(Models.VoiceType.SevenMetters);
                }
            }
        }

        public void resetMetters()
        {
            oneMetter = false;
            twoMetters = false;
            threeMetters = false;
            fourMetters = false;
            fiveMetters = false;
            sixMetters = false;
            sevenMetters = false;
        }

        void OnPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
