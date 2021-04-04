using BalizaFacil.Droid.Services;
using BalizaFacil.Models;
using BalizaFacil.Services;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;


[assembly: Xamarin.Forms.Dependency(typeof(StorageService))]
namespace BalizaFacil.Droid.Services
{
    public class StorageService : IStorageService
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static ISettings AppSettings => CrossSettings.Current;
        public Cars Cars
        {
            get
            {
                try
                {
                    string carString = AppSettings.GetValueOrDefault(nameof(Cars), string.Empty);

                    return Cars.FromJSON(carString);
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }

                return null;
            }
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(Cars), value.ToJSON());
                    OnPropertyChanged();
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }
            }
        }
        public double reactionTime
        {
            get
            {
                try
                {
                    return AppSettings.GetValueOrDefault(nameof(reactionTime),40.0);
                }
                catch (System.Exception ex)
                {
                    string title = "Speed Value";// this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }

                return 1.0;
            }
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(reactionTime), value);
                    OnPropertyChanged();
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }
            }
        }
        public Car Car
        {
            get
            {
                try
                {
                    string carString = AppSettings.GetValueOrDefault(nameof(Car), string.Empty);

                    return Car.FromJSON(carString);
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }

                return null;
            }
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(Car), value.ToJSON());
                    OnPropertyChanged();
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }
            }
        }

        public string GUID
        {
            get
            {
                try
                {
                    return AppSettings.GetValueOrDefault(nameof(GUID), string.Empty);
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }

                return "";
            }
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(GUID), value);
                    OnPropertyChanged();
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }
            }
        }

        public string Address
        {
            get
            {
                try
                {
                    return AppSettings.GetValueOrDefault(nameof(Address), string.Empty);
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }

                return "";
            }
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(Address), value);
                    OnPropertyChanged();
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }
            }
        }
        public string Username
        {
            get
            {
                try
                {
                    return AppSettings.GetValueOrDefault(nameof(Username), string.Empty);
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }

                return "";
            }
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(Username), value);
                    OnPropertyChanged();
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }
            }
        }
        public string Birthday
        {
            get
            {
                try
                {
                    return AppSettings.GetValueOrDefault(nameof(Birthday), string.Empty);
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }

                return "";
            }
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(Birthday), value);
                    OnPropertyChanged();
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }
            }
        }
        public bool TrainingMode
        {
            get
            {
                try
                {
                    return AppSettings.GetValueOrDefault(nameof(TrainingMode), false);
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }

                return false;
            }
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(TrainingMode), value);
                    OnPropertyChanged();
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }
            }
        }
        public bool FinalStepFree
        {
            get
            {
                try
                {
                    return AppSettings.GetValueOrDefault(nameof(FinalStepFree), false);
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }

                return false;
            }
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(FinalStepFree), value);
                    OnPropertyChanged();
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }
            }
        }
        public string TotalStops
        {
            get
            {
                try
                {
                    return AppSettings.GetValueOrDefault(nameof(TotalStops), "1");
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }

                return "1";
            }
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(TotalStops), value);
                    OnPropertyChanged();
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }
            }
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}