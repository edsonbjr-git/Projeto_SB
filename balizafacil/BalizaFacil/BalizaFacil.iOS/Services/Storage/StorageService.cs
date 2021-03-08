using BalizaFacil.iOS.Services;
using BalizaFacil.Models;
using BalizaFacil.Services;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;


[assembly: Xamarin.Forms.Dependency(typeof(StorageService))]
namespace BalizaFacil.iOS.Services
{
    public class StorageService : IStorageService
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static ISettings AppSettings => CrossSettings.Current;
        private Car car { get; set; }
        public Car Car
        {
            get
            {
                string carString = AppSettings.GetValueOrDefault(nameof(Car), string.Empty);

                return Car.FromJSON(carString);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Car), value.ToJSON());
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => AppSettings.GetValueOrDefault(nameof(Address), string.Empty);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Address), value);
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get => AppSettings.GetValueOrDefault(nameof(Username), string.Empty);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Address), value);
                OnPropertyChanged();
            }
        }

        public bool BackMode { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool TrainingMode { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool UseSensorBaliza { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public double reactionTime { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public MarginSize marginSize { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}