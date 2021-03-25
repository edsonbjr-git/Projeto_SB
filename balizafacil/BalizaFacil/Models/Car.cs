using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace BalizaFacil.Models
{
    public class Car : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double id;
        public double Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        private double wheelRadiusExternal;
        public double WheelRadiusExternal
        {
            get => wheelRadiusExternal;
            set
            {
                wheelRadiusExternal = value;
                //DistanceManager.fator_correcao = value;
                OnPropertyChanged();
            }
        }

        private double wheelRadiusInternal;
        public double WheelRadiusInternal
        {
            get => wheelRadiusInternal;
            set
            {
                wheelRadiusInternal = value;
                OnPropertyChanged();
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private double parkingSpace;
        public double ParkingSpace
        {
            get => parkingSpace;
            set
            {
                parkingSpace = value;
                OnPropertyChanged();
            }
        }

        private string style;
        public string Style
        {
            get => style;
            set
            {
                style = value;
                OnPropertyChanged();
            }
        }
        #region Maneuver total space
        private double firstLeft;
        public double FirstLeft
        {
            get
            {
                return firstLeft;
            }
            set
            {
                if (FirstLeft == value)
                    return;

                firstLeft = value;
                LeftSteps[0] = value;
                OnPropertyChanged();
            }
        }

        private double secondLeft;
        public double SecondLeft
        {
            get
            {
                return secondLeft;
            }
            set
            {
                if (SecondLeft == value)
                    return;

                secondLeft = value;
                LeftSteps[1] = value;
                OnPropertyChanged();
            }
        }

        private double thirdLeft;
        public double ThirdLeft
        {
            get
            {
                return thirdLeft;
            }
            set
            {
                if (ThirdLeft == value)
                    return;

                thirdLeft = value;
                LeftSteps[2] = value;
                OnPropertyChanged();
            }
        }

        private double fourthLeft;
        public double FourthLeft
        {
            get
            {
                return fourthLeft;
            }
            set
            {
                if (FourthLeft == value)
                    return;

                fourthLeft = value;
                LeftSteps[3] = value;
                OnPropertyChanged();
            }
        }

        private double fifthLeft;
        public double FifthLeft
        {
            get
            {
                return fifthLeft;
            }
            set
            {
                if (FifthLeft == value)
                    return;

                fifthLeft = value;
                LeftSteps[4] = value;
                OnPropertyChanged();
            }
        }
        private double sixthLeft;
        public double SixthLeft
        {
            get
            {
                return sixthLeft;
            }
            set
            {
                if (SixthLeft == value)
                    return;

                sixthLeft = value;
                LeftSteps[5] = value;
                OnPropertyChanged();
            }
        }


        private double firstRight;
        public double FirstRight
        {
            get
            {
                return firstRight;
            }
            set
            {
                if (FirstRight == value)
                    return;

                firstRight = value;
                RightSteps[0] = value;
                OnPropertyChanged();
            }
        }

        private double secondRight;
        public double SecondRight
        {
            get
            {
                return secondRight;
            }
            set
            {
                if (SecondRight == value)
                    return;

                secondRight = value;
                RightSteps[1] = value;
                OnPropertyChanged();
            }
        }

        private double thirdRight;
        public double ThirdRight
        {
            get
            {
                return thirdRight;
            }
            set
            {
                if (ThirdRight == value)
                    return;

                thirdRight = value;
                RightSteps[2] = value;
                OnPropertyChanged();
            }
        }

        private double fourthRight;
        public double FourthRight
        {
            get
            {
                return fourthRight;
            }
            set
            {
                if (FourthRight == value)
                    return;

                fourthRight = value;
                RightSteps[3] = value;
                OnPropertyChanged();
            }
        }

        private double fifthRight;
        public double FifthRight
        {
            get
            {
                return fifthRight;
            }
            set
            {
                if (FifthRight == value)
                    return;

                fifthRight = value;
                RightSteps[4] = value;
                OnPropertyChanged();
            }
        }
        private double sixrhRight;
        public double SixthRight
        {
            get
            {
                return sixrhRight;
            }
            set
            {
                if (SixthRight == value)
                    return;

                sixrhRight = value;
                RightSteps[5] = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public ObservableCollection<double> LeftSteps { get; internal set; }
        public ObservableCollection<double> RightSteps { get; internal set; }

        private double parkingSpaceBigger;
        public double ParkingSpaceBigger
        {
            get => parkingSpaceBigger;
            set
            {
                parkingSpaceBigger = value;
                OnPropertyChanged();
            }
        }

        private double fatorRI;
        public double FatorRI
        {
            get => fatorRI;
            set
            {
                fatorRI = value;
                OnPropertyChanged();
            }
        }

        //Para Alterar o nome é só selecionar a variavel e dar "Ctrl+R" duas vezes.
        //e depois adicionar no Constructor da classe caso precise.
        private double terceiraEtapaMinima;
        public double TerceitaEtapaMinima
        {
            get => terceiraEtapaMinima;
            set
            {
                terceiraEtapaMinima = value;
                OnPropertyChanged();
            }
        }
        private double variavel2;
        public double Variavel2
        {
            get => variavel2;
            set
            {
                variavel2 = value;
                OnPropertyChanged();
            }
        }
        private double variavel3;
        public double Variavel3
        {
            get => variavel3;
            set
            {
                variavel3 = value;
                OnPropertyChanged();
            }
        }
        public Car(double id, string name, string style, double wheelRadius, 
            double wheelRaiusExternal, double parkingSpace, double parkingSpaceBigger, 
            ObservableCollection<double> leftSteps, ObservableCollection<double> rightSteps, double terceiraEtapaMinima = 0)
        {
            Id = id;
            Name = name;
            WheelRadiusInternal = wheelRadius;
            WheelRadiusExternal = wheelRaiusExternal;
            TerceitaEtapaMinima = terceiraEtapaMinima;

            ParkingSpace = parkingSpace;
            ParkingSpaceBigger = parkingSpaceBigger;
            Style = style;

            while (leftSteps.Count <= 6)
            {
                leftSteps.Add(0);
            }
            while (rightSteps.Count <= 6)
            {
                rightSteps.Add(0);
            }

            LeftSteps = new ObservableCollection<double>(leftSteps);
            RightSteps = new ObservableCollection<double>(rightSteps);

            FirstLeft = leftSteps[0];
            SecondLeft = leftSteps[1];
            ThirdLeft = leftSteps[2];
            FourthLeft = leftSteps[3];
            FifthLeft = leftSteps[4];
            SixthLeft = leftSteps[5];

            FirstRight = rightSteps[0];
            SecondRight = rightSteps[1];
            ThirdRight = rightSteps[2];
            FourthRight = rightSteps[3];
            FifthRight = rightSteps[4];
            SixthRight = rightSteps[5];
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Car FromJSON(string json)
        {
            Car car = JsonConvert.DeserializeObject<Car>(json);

            return car;
        }
       
        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public bool compareCars(Car carNew)
        {
            return ((this.ParkingSpace.Equals(carNew.ParkingSpace)) &&
                    (this.ParkingSpaceBigger.Equals(carNew.ParkingSpaceBigger)) &&
                    (this.FirstLeft.Equals(carNew.FirstLeft)) && (this.FirstRight.Equals(carNew.FirstRight)) &&
                    (this.SecondLeft.Equals(carNew.SecondLeft)) && (this.SecondRight.Equals(carNew.SecondRight)) &&
                    (this.ThirdLeft.Equals(carNew.ThirdLeft)) && (this.ThirdRight.Equals(carNew.ThirdRight)) &&
                    (this.FourthLeft.Equals(carNew.FourthLeft)) && (this.FourthRight.Equals(carNew.FourthRight)) &&
                    (this.FifthLeft.Equals(carNew.FifthLeft)) && (this.FifthRight.Equals(carNew.FifthRight)) &&
                    (this.SixthLeft.Equals(carNew.SixthLeft)) && (this.SixthRight.Equals(carNew.SixthRight)) &&
                    (this.FatorRI.Equals(carNew.FatorRI))
                    );

            //return (this.LeftSteps.Equals(carNew.LeftSteps) && this.RightSteps.Equals(carNew.RightSteps));
        }
    }
}
