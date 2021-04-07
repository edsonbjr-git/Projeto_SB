using BalizaFacil.Models;
using BalizaFacil.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;

namespace BalizaFacil.Core
{
    public class DistanceManager
    {
        public event Action<double> DistanceChanged;
        public event Action<double> SpeedChanged;


        private static DistanceManager instance;
        public static DistanceManager Instance
        {
            get
            {
                if (instance is null)
                    instance = new DistanceManager();

                return instance;
            }
        }

        #region Acceleration

        private double FilteredAccelerationA { get; set; }
        private double FilteredAccelerationB { get; set; }
        private bool KeepAccelerationUnfiltered { get; set; }

        private double currentSpeed;
        public double CurrentSpeed
        {
            get => currentSpeed;
            set
            {
                if (currentSpeed == value)
                    return;

                currentSpeed = value;
                SpeedChanged?.Invoke(currentSpeed);
            }
        }

        #endregion

        #region AccelerationAngle

        private double OldAccelerationAngle { get; set; }
        private double CurrentAccelerationAngle { get; set; }
        private double AUX_AccelerationAngle { get; set; } //sergio AjusteMov        
        private double CurrentStepAccelerationInitialAngle { get; set; }
        private bool IsOldAccelerationAngleDefined { get; set; }

        #endregion

        #region Distances
        private double OldTotalDistanceOnDegress { get; set; }
        private double TotalDistanceOnDegress { get; set; }

        private double OldTotalDistanceOnMeters { get; set; }
        private double totalDistanceOnMeters;
        public double TotalDistanceOnMeters
        {
            get => totalDistanceOnMeters;
            set
            {
                if (value == totalDistanceOnMeters)
                    return;

                totalDistanceOnMeters = value;
                DistanceChanged?.Invoke(totalDistanceOnMeters);
            }
        }
        private double FilteredTotalDistanceOnDegress { get; set; }

        private double TotalDistanceOnOneRotationForward { get; set; }
        private double TotalDistanceOnOneRotationBackward { get; set; }

        private bool equalifyFilteredAccelerations;

        // DateTime oldIterationTime = new DateTime();

        #endregion

        #region Process
        
        private bool Reset { get; set; }
        private Sensor Sensor => Sensor.Instance;
        private Car Car => FlowManager.Instance.Car;
        private double WheelRotatations { get; set; }
        public double AppliedCorrection { get; set; }

        #endregion

        #region UpdateThread

        private int UpdateDistancesInterval { get; set; } = 30;
        public Thread UpdateDistances { get; set; }

        public volatile bool Updating;

        #endregion

        private DistanceManager()
        {
            Updating = true;
            UpdateDistances = new Thread(() =>
            {

                try
                {
                    while (Updating)
                    {
                        UpdateDistanceValues();
                        Thread.Sleep(UpdateDistancesInterval);
                    }
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }
            });
            UpdateDistances.Priority = ThreadPriority.AboveNormal;
            UpdateDistances.Name = nameof(UpdateDistances);
            UpdateDistances.IsBackground = true;

            UpdateDistances.Start();
        }
        public event Action<double, double, double, double, double> Data;
        private void UpdateDistanceValues()
        {
            try
            {
                if (Reset)
                {
                    _ResetDistances();
                    Reset = false;
                }

                //oldIterationTime = DateTime.Now;

                //fatores de conversão
                double accelerationScale = FlowManager.Instance.UseSensorBaliza ? 16 :4096; 
                
                double gyroscopeScale = 32;

                double gyro, accelerationA, totalAccelerationB, accelerationB;

                if (!FlowManager.Instance.UseSensorBaliza)
                {

                    //CC2650
                    gyro = Sensor.Gyroscope.Y / gyroscopeScale;

                    accelerationA = -1 * Sensor.Accelerometer.X / accelerationScale;

                    //Acceleration angle with centrifugal  force.
                    totalAccelerationB = -1 * Sensor.Accelerometer.Z / accelerationScale;
                    accelerationB = RemoveCentrifugalAcceleration(totalAccelerationB, gyro);
                    ///////////////////////
                }
                else
                {
                    //CC2541
                    gyro = Sensor.Gyroscope.Z / gyroscopeScale;

                    accelerationA = -1 * Sensor.Accelerometer.Y / accelerationScale;

                    totalAccelerationB = -1 * Sensor.Accelerometer.X / accelerationScale;
                    accelerationB = RemoveCentrifugalAcceleration(totalAccelerationB, gyro);
                }

                Data?.Invoke(accelerationScale, gyroscopeScale, accelerationA, accelerationB, gyro);
                //Debug.WriteLine($"ACCX {Sensor.Accelerometer.X}, ACCY {Sensor.Accelerometer.Y}, ACCZ {Sensor.Accelerometer.Z}, GYRX {Sensor.Gyroscope.X}, GYRY {Sensor.Gyroscope.Y}, GYRZ {Sensor.Gyroscope.Z}");
                //Console.WriteLine($"ACCX {Sensor.Accelerometer.X}, ACCY {Sensor.Accelerometer.Y}, ACCZ {Sensor.Accelerometer.Z}, GYRX {Sensor.Gyroscope.X}, GYRY {Sensor.Gyroscope.Y}, GYRZ {Sensor.Gyroscope.Z}, GyroScaled {gyro}, ACCScaledA {accelerationA}, ACCScaledB {accelerationB}");

                if (equalifyFilteredAccelerations)
                {
                    equalifyFilteredAccelerations = false;
                    FilteredAccelerationA = accelerationA;
                    FilteredAccelerationB = accelerationB;
                }
                else
                {
                    FilteredAccelerationA = FilterNoise(accelerationA, FilteredAccelerationA, 0.421875);
                    FilteredAccelerationB = FilterNoise(accelerationB, FilteredAccelerationB, 0.421875);
                    //FilteredAccelerationX = FilterNoise(accelerationX, FilteredAccelerationX, 0.75);
                    //FilteredAccelerationZ = FilterNoise(accelerationZ, FilteredAccelerationZ, 0.75);
                }

                if (KeepAccelerationUnfiltered)
                    CurrentAccelerationAngle = GetAccelerationAngle(accelerationB, accelerationA);
                else
                    CurrentAccelerationAngle = GetAccelerationAngle(FilteredAccelerationB, FilteredAccelerationA);

                if (!IsOldAccelerationAngleDefined)
                {
                    OldAccelerationAngle = CurrentAccelerationAngle;
                    IsOldAccelerationAngleDefined = true;
                }

                double accelerationRate = GetAccelerationRate(CurrentAccelerationAngle, OldAccelerationAngle, UpdateDistancesInterval);
                OldAccelerationAngle = CurrentAccelerationAngle;

                WheelRotatations = CheckWheelRotation(accelerationRate, gyro, WheelRotatations);

                TotalDistanceOnDegress = CurrentAccelerationAngle + 360 * WheelRotatations - CurrentStepAccelerationInitialAngle;
                CorrectWheelRotationAndTotalDistance();
                OldTotalDistanceOnDegress = TotalDistanceOnDegress;

                FilteredTotalDistanceOnDegress = FilterNoise(TotalDistanceOnDegress, FilteredTotalDistanceOnDegress, 0.75);

                //Validate what RW means since it can changes when the Car model changes
                //double rw = 1f * fator_correcao;

                TotalDistanceOnMeters = -100 * Car.WheelRadiusExternal * FilteredTotalDistanceOnDegress * Math.PI / 180;
                double speed = GetCurrentSpeed(TotalDistanceOnMeters, OldTotalDistanceOnMeters, UpdateDistancesInterval);
                CurrentSpeed = FilterNoise(CurrentSpeed, speed, 0.5);
                //Console.WriteLine($"SPEED {speed}, SPEED_F {CurrentSpeed}");

                OldTotalDistanceOnMeters = TotalDistanceOnMeters;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void ResetDistances()
        {
            this.Reset = true;
        }
        
        private void _ResetDistances()
        {
            FilteredTotalDistanceOnDegress = 0;
            WheelRotatations = 0;
            CurrentStepAccelerationInitialAngle = CurrentAccelerationAngle;
            TotalDistanceOnMeters = 0;
            TotalDistanceOnOneRotationForward = 99;
            TotalDistanceOnOneRotationBackward = -99;
            equalifyFilteredAccelerations = false;
            AUX_AccelerationAngle = 0;//sergio AjusteMov
        }

/*        /// <summary>
        /// Get the current speed based on the diference between how much the Car moved since the last execution
        /// </summary>
        private double GetCurrentSpeed(double totalDistanceOnMeters, double oldTotalDistanceOnMeters, double millisecondsPassed)
        {
            if (millisecondsPassed == 0)
                return 0;
            var unconvertedSpeed = (totalDistanceOnMeters - oldTotalDistanceOnMeters) * 1000 / millisecondsPassed;

            return 0; // sergio AjusteMov * 3.6 * unconvertedSpeed / 100; 
        }*/

        /// <summary>
        /// Compares the current distance with the distance if the wheel had fully rotated one time forward and backward to correct the number of rotations and the current distance itself
        /// </summary>
        private void CorrectWheelRotationAndTotalDistance()
        {
            if (TotalDistanceOnOneRotationForward == 99 && TotalDistanceOnOneRotationBackward == -99)
            {
                TotalDistanceOnOneRotationForward = 0;
                TotalDistanceOnOneRotationBackward = 0;
            }
            else
            {
                TotalDistanceOnOneRotationForward = CurrentAccelerationAngle + 360 * (WheelRotatations + 1) - CurrentStepAccelerationInitialAngle;
                TotalDistanceOnOneRotationBackward = CurrentAccelerationAngle + 360 * (WheelRotatations - 1) - CurrentStepAccelerationInitialAngle;
            }

            double totalDistanceDifference = Math.Abs(TotalDistanceOnDegress - OldTotalDistanceOnDegress);
            double totalDistanceDifferenceOnOneRotationForward = Math.Abs(TotalDistanceOnOneRotationForward - OldTotalDistanceOnDegress);
            double totalDistanceDifferenceOnOneRotationBackward = Math.Abs(TotalDistanceOnOneRotationBackward - OldTotalDistanceOnDegress);

            if (totalDistanceDifference > totalDistanceDifferenceOnOneRotationForward)
            {
                TotalDistanceOnDegress = TotalDistanceOnOneRotationForward;
                WheelRotatations++;
            }

            if (totalDistanceDifference > totalDistanceDifferenceOnOneRotationBackward)
            {
                TotalDistanceOnDegress = TotalDistanceOnOneRotationBackward;
                WheelRotatations--;
            }
        }

        /// <summary>
        /// Uses the acceleration rate and gyroscope Y-Axis data to define if the wheel make a complete rotation between this and last calculation
        /// </summary>
        private double CheckWheelRotation(double accelerationRate, double gyroscopeZ, double wheelRotatations)
        {
            //The wheel has fully turned forward
            if ((accelerationRate < -300 && gyroscopeZ > 200) || (accelerationRate < -2000 && gyroscopeZ > -20))
                wheelRotatations++;

            //The wheel has fully turned once to back
            if ((accelerationRate > 300 && gyroscopeZ < -200) || (accelerationRate > 2000 && gyroscopeZ < 20))
                wheelRotatations--;

            return wheelRotatations;
        }

        /// <summary>
        /// Generates the acceleration rate in seconds based on the current acceleration angle, the last acceleration angle and how much milliseconds passed between them. 
        /// </summary>
        private double GetAccelerationRate(double currentAccelerationAngle, double oldAccelerationAngle, double millisecondsPassed)
        {
            if (millisecondsPassed == 0)
                return 0; 

            double accelerationRateInSeconds = 1000 * (currentAccelerationAngle - oldAccelerationAngle) / millisecondsPassed;

            return accelerationRateInSeconds;
        }

        //AjusteMov Inicio ====================================================================
        // ====================================================================================

        private double GetCurrentSpeed(double totalDistanceOnMeters, double oldTotalDistanceOnMeters, double millisecondsPassed)
        {
            if (millisecondsPassed == 0)
                return 0;
            var unconvertedSpeed = (totalDistanceOnMeters - oldTotalDistanceOnMeters) * 1000 / millisecondsPassed;

            return 3.6 * unconvertedSpeed / 100;
        }

        private double GetAccelerationAngle(double accelerationA, double accelerationB)
        {
            double accelerationAngle = Math.Atan2(accelerationA, accelerationB);
            double accelerationAngleOnDregress = accelerationAngle * 180 / Math.PI;

            return accelerationAngleOnDregress;
        }

        /// <summary>
        /// Removes the centrifugal acceleration from the acceleration 
        /// </summary>
        /// <param name="acceleration">The acceleration of angle who contains centrifugal acceleration</param>
        /// <returns>acceleration without centrifugal force</returns>
        private double RemoveCentrifugalAcceleration(double acceleration, double gyroscope)
        {
            double PolToMeters = 0.0254;
            double gravityAcceleration = 9.81;

            double sensorRadiusPositionOnPol = 0.9 * Car.WheelRadiusInternal;

            double sensorRadiusPositionOnMeters = sensorRadiusPositionOnPol * PolToMeters;

            double centrifugalAccelerationOnMPS2 = sensorRadiusPositionOnMeters * Math.Pow((Math.PI / 180) * gyroscope, 2);
            double centrifugalAccelerationOnG = centrifugalAccelerationOnMPS2 / gravityAcceleration;

            return acceleration + centrifugalAccelerationOnG;
        }

        /// <summary>
        /// Reduces the noise from factors.
        /// </summary>
        private double FilterNoise(double factor, double filtredFactor, double alpha)
        {
            return (1 - alpha) * factor + alpha * filtredFactor;
        }



        // =================================================================================
        //AjusteMov Fim ====================================================================

        public IStorageService Storage => ServicesManager.Instance.Storage;
        public int index = 0;
        public double GetManeuverLength(ApplicationStep step, Direction direction)
        {
            int AppliedCorrection = 0;

            if (step == ApplicationStep.ManeuverI && TotalDistanceOnMeters >= Car.ParkingSpace)
                AppliedCorrection = (int)(TotalDistanceOnMeters - Car.ParkingSpace) / 2;

            ObservableCollection<double> steps = direction == Direction.Left ? Car.LeftSteps : Car.RightSteps;
            if (sizePark == ParkingSpaceSizeType.VeryEasy)
                index = 6;
            else
                index = 0;
            int stepIndex = (int)step - (int)ApplicationStep.ManeuverI;

            double result = steps[stepIndex+index];
             
            if (step == ApplicationStep.ManeuverI)
            {
                //result += ((double)Storage.marginSize / 2);
                if ((result - AppliedCorrection) * result > 0)
                    result = result - AppliedCorrection;
                else
                    result = 50; //[guilherme/sergio] antes era 20, mas dava pau quando a vaga era grande demais tentar descobrir porque
                                 // o ideal eh permitir que pule a primeira etapa se a vaga for grande
            }

            return result;
        }

        /// <summary>
        /// Correct the step sizes and return if the parking size is enough to Carry on.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="step"></param>
        public bool CorrectOnCurbColision(Direction direction, ApplicationStep step)
        {

            try
            {
                int stepIndex = (int)step - (int)ApplicationStep.ManeuverI;
                ObservableCollection<double> steps = direction == Direction.Left ? Car.LeftSteps : Car.RightSteps;
                double stepSize = steps[stepIndex];  // Etapa em re com volante centralizado que toca na guia

                double MarginAdded = 20; // diferenca entre as primeiras etapas do calculo com 150 e 190cm de deslocamento lateral.
                double deltaS = -(-stepSize - TotalDistanceOnMeters + AppliedCorrection * Math.Cos(0.785) + MarginAdded); // 45 graus eh uma inclinacao media/alta para balizas
                double correctionFactor = 1.0;
                //Console.WriteLine("DeltaS {0:N}\n", deltaS);
                if (deltaS <= 20)  
                    correctionFactor = 1.0;
                else if (deltaS > 20 && deltaS < 40) 
                    correctionFactor = 1.0;// (deltaS - 35) * 0.02 + 1; // sergio, troquei para 1 em 06/12/2020
                else
                    return false;

                //Double deltaS = -(-stepSize - TotalDistanceOnMeters + AppliedCorrection * Math.Cos(0.6108652382)); // 35 graus eh uma inclinacao media/alta para balizas
                //Double correctionFactor = 1.0;
                //if (deltaS <= 50)
                //    correctionFactor = 1.0;
                //else if (deltaS > 50 && deltaS < 75)
                //    correctionFactor = (deltaS - 50) * 0.02 + 1;
                //else
                //    return false;

                for (int i = stepIndex; i < steps.Count; i++)
                {
                    steps[i] = (int)(steps[i] / correctionFactor);
                }
                correctionFactor = 1.0;

                if (direction == Direction.Left)
                    Car.LeftSteps = steps;
                else
                    Car.RightSteps = steps;

                return true;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }

            return false;
        }
        public int CorrectOnCurbColisionTeste(Direction direction, ApplicationStep step)
        {
            try
            {
                int stepIndex = (int)step - (int)ApplicationStep.ManeuverI;
                ObservableCollection<double> steps = direction == Direction.Left ? Car.LeftSteps : Car.RightSteps;
                double stepSize = steps[stepIndex];  // Etapa em re com volante centralizado que toca na guia



                double MarginAdded = 20; // diferenca entre as primeiras etapas do calculo com 150 e 190cm de deslocamento lateral.
                double deltaS = -(-stepSize - TotalDistanceOnMeters + AppliedCorrection * Math.Cos(0.785) + MarginAdded); // 45 graus eh uma inclinacao media/alta para balizas
                double correctionFactor = 1.0;


                if (deltaS < 20)  
                    return 1;
                else if (deltaS >= 20 && deltaS < 40) 
                    return 2;
                else
                    return 0;

            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }

            return 0;
        }
        public static ParkingSpaceSizeType sizePark = ParkingSpaceSizeType.None;
        public ParkingSpaceSizeType CalculateParkingSpaceSizeType(double parkingSpaceSize, bool endMeasure = false)
        {
            
            if (Car.ParkingSpaceBigger != (double)ParkingSpaceSizeType.Bigger)
            {
                if (parkingSpaceSize >= Car.ParkingSpaceBigger)
                {
                    if(endMeasure)
                        sizePark = ParkingSpaceSizeType.VeryEasy;
                    return ParkingSpaceSizeType.VeryEasy;
                }
            }

            double percentual = (100 * parkingSpaceSize / Car.ParkingSpace ) - 100;
            var oldParkingSpaceSize = parkingSpaceSize;

            if (percentual >= 20)
                return ParkingSpaceSizeType.Easy;
            else if (percentual >= 10 && percentual < 20)
                return ParkingSpaceSizeType.Average;
            else if ((percentual >= 0 && percentual < 10) || double.IsInfinity(percentual))
                return ParkingSpaceSizeType.Hard;
            else if ((percentual >= -5.5 && percentual < 0) || double.IsInfinity(percentual))
                return ParkingSpaceSizeType.VeryHard;
            else
                return ParkingSpaceSizeType.Insuficient;

        }
    }
}
