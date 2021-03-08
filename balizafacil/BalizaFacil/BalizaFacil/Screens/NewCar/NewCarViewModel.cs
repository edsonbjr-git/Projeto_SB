using BalizaFacil.Core;
using BalizaFacil.Models;
using BalizaFacil.Screens;
using BalizaFacil.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BalizaFacil
{
    [Preserve (AllMembers = true)]
    public class NewCarViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IStorageService storage => ServicesManager.Instance.Storage;

        private Car car { get; set; }
        public Car Car
        {
            get => car;
            set
            {
                car = value;
                OnPropertyChanged();
            }
        }

        private double firstLeft;
        public double FirstLeft
        {
            get
            {
                return car.FirstLeft;
            }
            set
            {
                firstLeft = value;
                car.FirstLeft = value;
                OnPropertyChanged();
            }
        }

        private double secondLeft;
        public double SecondLeft
        {
            get
            {
                return car.SecondLeft;
            }
            set
            {
                secondLeft = value;
                car.SecondLeft = value;
                OnPropertyChanged();
            }
        }

        private double thirdLeft;
        public double ThirdLeft
        {
            get
            {
                return car.ThirdLeft;
            }
            set
            {
                thirdLeft = value;
                car.ThirdLeft = value;
                OnPropertyChanged();
            }
        }

        private double fourthLeft;
        public double FourthLeft
        {
            get
            {
                return car.LeftSteps[3];
            }
            set
            {
                fourthLeft = value;
                car.FourthLeft = value;
                OnPropertyChanged();
            }
        }

        private double fifthLeft;
        public double FifthLeft
        {
            get
            {
                return car.FifthLeft;
            }
            set
            {
                fifthLeft = value;
                car.FifthLeft = value;
                OnPropertyChanged();
            }
        }

        private double sixthLeft;
        public double SixthLeft
        {
            get
            {
                return car.SixthLeft;
            }
            set
            {
                sixthLeft = value;
                car.SixthLeft = value;
                OnPropertyChanged();
            }
        }


        private double firstRight;
        public double FirstRight
        {
            get
            {
                return car.FirstRight;
            }
            set
            {
                firstRight = value;
                car.FirstRight = value;
                OnPropertyChanged();
            }
        }

        private double secondRight;
        public double SecondRight
        {
            get
            {
                return car.SecondRight;
            }
            set
            {
                secondRight = value;
                car.SecondRight = value;
                OnPropertyChanged();
            }
        }

        private double thirdRight;
        public double ThirdRight
        {
            get
            {
                return car.ThirdRight ;
            }
            set
            {
                thirdRight = value;
                car.ThirdRight = value;
                OnPropertyChanged();
            }
        }

        private double fourthRight;
        public double FourthRight
        {
            get
            {
                return car.FourthRight;
            }
            set
            {
                fourthRight = value;
                car.FourthRight = value;
                OnPropertyChanged();
            }
        }

        private double fifthRight;
        public double FifthRight
        {
            get
            {
                return car.FifthRight;
            }
            set
            {
                fifthRight = value;
                car.FifthRight = value;
                OnPropertyChanged();
            }
        }

        private double sixthRight;
        public double SixthRight
        {
            get
            {
                return car.SixthRight;
            }
            set
            {
                sixthRight = value;
                car.SixthRight = value;
                OnPropertyChanged();
            }
        } 

        public ICommand Continue { get; internal set; }

        public NewCarViewModel(bool closeOnComplete)
        {
            Car = FlowManager.Instance.Car ?? new Car(0, "Outros","s", 0, 1, 0, -10, new ObservableCollection<double>(), new ObservableCollection<double>());
            Continue = new Command(() =>
            {
                CarsRequestJSON.Instance.Cars.cars.Add(Car);
                storage.Cars = CarsRequestJSON.Instance.Cars;
                storage.Car = Car;
                FlowManager.Instance.Car = Car;
                BaseContentPage.Instance.PopModal();

                if (closeOnComplete)
                    BaseContentPage.Instance.PopView();
                else
                    FlowManager.Instance.ChangeStep(ApplicationStep.ConfigureSensor);
            });
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }



}
