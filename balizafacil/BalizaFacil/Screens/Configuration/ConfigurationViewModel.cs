using BalizaFacil.Models;
using BalizaFacil.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BalizaFacil.Screens.Configuration
{
    class ConfigurationViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand UpdateCar { get; internal set; }
        public IStorageService Storage => ServicesManager.Instance.Storage;
        public Car car => Storage.Car;

        public ConfigurationViewModel(bool closeOnComplete)
        {
             
            UpdateCar = new Command(() =>
            {
                CarsRequestJSON.Instance.updateCar();

            });

        }


        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
