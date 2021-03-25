using BalizaFacil.Core;
using BalizaFacil.Models;
using BalizaFacil.Services;
using BalizaFacil.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class CarBrandViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<CarFilterInfo> BrandOptions { get; set; }

        private List<Car> Options = new List<Car>();

        private CarFilterInfo selectedBrand;
        public CarFilterInfo SelectedBrand
        {
            get => selectedBrand; set
            {
                selectedBrand = value;
                OnPropertyChanged();
            }
        }

        private CarFilterInfo tappedBrand;
        public CarFilterInfo TappedBrand
        {
            get => tappedBrand; set
            {
                tappedBrand = value;
                OnPropertyChanged();
            }
        }

        public bool CloseOnComplete { get; private set; }
        public ICommand SelectOption { get; private set; }

        public Cars cars = new Cars();
        public Cars carsDefault;


        public CarBrandViewModel(bool closeOnComplete)
        {
            cars = carsDefault = CarsRequestJSON.Instance.getCars(true);
            CloseOnComplete = closeOnComplete;
            SelectOption = new Command(OnSelectOptionClicked);
            configureDefault();    
        }

        public void configureDefault()
        {
            FillOptions();
            foreach (Car car in cars.cars)
            {
                Options.Add(car);
                Debug.Write(car.Name);
            }
        }

        
        private void FillOptions()
        {
            BrandOptions = new ObservableCollection<CarFilterInfo>();
            foreach (Car car in cars.cars)
            {
                BrandOptions.Add(new CarFilterInfo() { Id = (int)car.Id, Name = car.Name + " " + car.Style});
            }
            BrandOptions.Add(new CarFilterInfo() { Id = 1, Name = "Outros" });
        }

        public void configureByCar(string name)
        {
            FillOptionsSearchCar(name);

            try
            {
                cars = CarsRequestJSON.Instance.searchCars(name); ;

            }
            catch (Exception ex)
            {
                cars = carsDefault;
            }
            foreach (Car car in cars.cars)
            {
                Options.Add(car);
                Debug.Write(car.Name);
            }
        }

        private void FillOptionsSearchCar(string name)
        {
            BrandOptions = new ObservableCollection<CarFilterInfo>();
            try
            {
                cars = CarsRequestJSON.Instance.searchCars(name);
            }
            catch
            {
                cars = carsDefault;
            }
            foreach (Car car in cars.cars)
            {
                BrandOptions.Add(new CarFilterInfo() { Id = (int)car.Id, Name = car.Name + " " + car.Style});
            }
            BrandOptions.Add(new CarFilterInfo() { Id = 1, Name = "Outros" });
        }

        void OnSelectOptionClicked()
        {
            if (SelectedBrand is null)
                return;

            if (SelectedBrand.Id > 1)
            {
                Services.ServicesManager.Instance.Storage.Car = Options.First(d => d.Id == SelectedBrand.Id);
                FlowManager.Instance.Car = Services.ServicesManager.Instance.Storage.Car;

                if (CloseOnComplete)
                    BaseContentPage.Instance.PopView();
                else
                    FlowManager.Instance.ChangeStep(ApplicationStep.ConfigureSensor);
                return;
            }

            NewCarView newCarView = new NewCarView(CloseOnComplete);

            BaseContentPage.Instance.PushModal(newCarView);
        }

        public void searchCar(string text)
        {
            configureByCar(text);      

        }
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}