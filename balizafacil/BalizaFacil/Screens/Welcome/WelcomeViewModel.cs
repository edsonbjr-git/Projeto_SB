using BalizaFacil.Core;
//using BalizaFacil.Models;
//using System.ComponentModel;
//using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class WelcomeViewModel// : INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        public ICommand Continue { get; set; } = new Command(() => { FlowManager.Instance.ChangeStep(ApplicationStep.ChooseDirection); });
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
