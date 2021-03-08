using BalizaFacil.Core;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class RegisterViewModel
    {
        public string Username { get; set; }
        public DateTime Birthday { get; set; }

        public ICommand Register { get; set; }

        public RegisterViewModel()
        {
            Register = new Command(() =>
            {
                Services.ServicesManager.Instance.Storage.Username = Username;
                Services.ServicesManager.Instance.Storage.Birthday = Birthday.ToString();
                FlowManager.Instance.ChangeStep(ApplicationStep.ConfigureCarBrand);
            });
        }
    }
}
