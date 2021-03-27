using BalizaFacil.Core;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class WelcomeViewModel// : INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        public ICommand Continue { get; set; } = new Command(() =>
        {
            App.CurrentLogBaliza = new Models.LogBaliza();
            App.CurrentLogBaliza.Data = DateTime.Now.Date;
            App.CurrentLogBaliza.Inicio = DateTime.Now.TimeOfDay;
            App.CurrentLogBaliza.Id = Guid.NewGuid().ToString();

            FlowManager.Instance.ChangeStep(ApplicationStep.ChooseDirection);
        });


        //private double Velocidade;

        //public double velocidade
        //{
        //    get => Velocidade;
        //    set
        //    {
        //        Velocidade = DistanceManager.Instance.CurrentSpeed;
        //        OnPropertyChanged();
        //    }
        //}
        //void OnPropertyChanged([CallerMemberName] string name = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //}        //private double Velocidade;

        //public double velocidade
        //{
        //    get => Velocidade;
        //    set
        //    {
        //        Velocidade = DistanceManager.Instance.CurrentSpeed;
        //        OnPropertyChanged();
        //    }
        //}
        //void OnPropertyChanged([CallerMemberName] string name = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //}
    }

}
