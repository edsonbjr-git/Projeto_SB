using BalizaFacil.Core;
using BalizaFacil.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class CarModelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<CarFilterInfo> ModelOptions { get; set; }

        private CarFilterInfo selectedModel;
        public CarFilterInfo SelectedModel
        {
            get => selectedModel; set
            {
                selectedModel = value;
                OnPropertyChanged();
            }
        }
        public ICommand SelectOption { get; private set; }


        public CarModelViewModel()
        {
            this.SelectOption = new Command(OnSelectOptionClicked);
            FillOptions();
        }

        private void FillOptions()
        {
            this.ModelOptions = new ObservableCollection<CarFilterInfo>();
            this.ModelOptions.Add(new CarFilterInfo() { Id = 1, Name = "Logan" });
            this.ModelOptions.Add(new CarFilterInfo() { Id = 2, Name = "Clio" });
            this.ModelOptions.Add(new CarFilterInfo() { Id = 3, Name = "Duster" });
        }

        void OnSelectOptionClicked()
        {
            FlowManager.Instance.CarModel = SelectedModel;
            FlowManager.Instance.ChangeStep(ApplicationStep.ConfigureCarType);
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
