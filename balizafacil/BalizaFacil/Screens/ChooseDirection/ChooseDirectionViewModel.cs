using BalizaFacil.Core;
using BalizaFacil.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class ChooseDirectionViewModel
    {
        public ICommand DirectionChoosed { get; internal set; }

        public ChooseDirectionViewModel()
        {
            DirectionChoosed = new Command<Direction>((direction) =>
            {
                //Só pra Teste  FlowManager.Instance.Direction = Direction.Left;
                FlowManager.Instance.Direction = direction;
                //FlowManager.Instance.ChangeStep(ApplicationStep.MeasureSpace); modo treinamento
                FlowManager.Instance.ChangeStep(ApplicationStep.ManeuverI);
            });
        }
    }
}
