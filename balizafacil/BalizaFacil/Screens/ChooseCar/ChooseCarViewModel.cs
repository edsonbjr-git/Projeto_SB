using BalizaFacil.Core;
using BalizaFacil.Models;
using BalizaFacil.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class ChooseCarViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Car> Options { get; set; }

        private Car selectedOption;
        public Car SelectedOption
        {
            get => selectedOption; set
            {
                selectedOption = value;
                OnPropertyChanged();
            }
        }
        public ICommand SelectOption { get; private set; }


        public ChooseCarViewModel()
        {
            Options = new ObservableCollection<Car>();
            SelectOption = new Command(OnSelectOptionClicked);
            FillCarOptions();
        }



        void OnSelectOptionClicked()
        {
            ServicesManager.Instance.Storage.Car = SelectedOption;
            FlowManager.Instance.ChangeStep(ApplicationStep.ConfigureSensor);
        }
        /// <summary>
        /// No futuro, usará a HttpRequestService pra pegar dados de opções de carros do back-end
        /// </summary>
        public void FillCarOptions()
        {

            var clioSStep = new ObservableCollection<double> { 155, 360, 195, 134, 100, 80 };

            Options.Add(new Car(1, "Peugeot", "408", 8.5,1, 640, -10, new ObservableCollection<double>  { 260, 153, 406, 51, 72, 41 }, new ObservableCollection<double>  { 260, 230, 406, 77, 48, 62 }));
            Options.Add(new Car(2, "Renault", "Clio H", 7,1, 563, -10, new ObservableCollection<double> { 205, 234, 250, 87, 154, 33 }, new ObservableCollection<double> { 175, 360, 250, 134, 100, 80 }));
            Options.Add(new Car(3, "Ford", "Fiesta", 7,1, 535, -10, new ObservableCollection<double> { 206, 180, 338, 60, 81, 63 }, new ObservableCollection<double> { 206, 257, 338, 86, 57, 71 }));
            Options.Add(new Car(4, "Renault", "Clio S", 7,1, 495, -10, clioSStep, clioSStep));
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
