using BalizaFacil.Core;
using BalizaFacil.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class CarTypeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<CarFilterInfo> TypeOptions { get; set; }

        private CarFilterInfo selectedType;
        public CarFilterInfo SelectedType
        {
            get => selectedType; set
            {
                selectedType = value;
                OnPropertyChanged();
            }
        }
        public ICommand SelectOption { get; private set; }


        public CarTypeViewModel()
        {
            this.SelectOption = new Command(OnSelectOptionClicked);
            FillOptions();
        }

        private void FillOptions()
        {
            this.TypeOptions = new ObservableCollection<CarFilterInfo>();
            this.TypeOptions.Add(new CarFilterInfo() { Id = 1, Name = "Hatch" });
            this.TypeOptions.Add(new CarFilterInfo() { Id = 2, Name = "Van" });
            this.TypeOptions.Add(new CarFilterInfo() { Id = 3, Name = "Sedan" });
            this.TypeOptions.Add(new CarFilterInfo() { Id = 4, Name = "Perua" });
            this.TypeOptions.Add(new CarFilterInfo() { Id = 5, Name = "Conversível" });
            this.TypeOptions.Add(new CarFilterInfo() { Id = 6, Name = "Suv" });
            this.TypeOptions.Add(new CarFilterInfo() { Id = 7, Name = "Mpv" });
        }

        void OnSelectOptionClicked()
        {
            FlowManager.Instance.CarType = SelectedType;
            Services.ServicesManager.Instance.Storage.Car = new Car(1, "Volkswagen", "Voyage 2020", 7, 0.291f, 623, -404, new ObservableCollection<double> { 142, 210, 227, 62, 127, 62 }, new ObservableCollection<double> { 142, 313, 227, 93, 86, 93 });
            FlowManager.Instance.ChangeStep(ApplicationStep.ConfigureSensor);
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
