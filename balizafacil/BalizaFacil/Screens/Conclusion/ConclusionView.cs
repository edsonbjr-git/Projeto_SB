using BalizaFacil.Models;
using BalizaFacil.Services;
using BalizaFacil.UI;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class ConclusionView : StackLayout, IAppearView
    {
        ConclusionViewModel ViewModel { get; set; }

        public ConclusionView()
        {
            ViewModel = new ConclusionViewModel();
            ConfigureScreen();
        }

        public void OnAppearing()
        {
            ServicesManager.Instance.SoundPlayer.PlaySound(VoiceType.Finished);
        }

        private void ConfigureScreen()
        {
            BackgroundColor = Color.White;
            var navBar = BaseContentPage.Instance.ConfigureNavBar("", false, true);
            var Title = "Conseguimos!!!";
            var Description = "Baliza finalizada com sucesso";
            var Img = "heart.svg";
            if (StepViewModel.curbColisionTrue == true)
            {
                Img = "x.svg";
                Title = "Eita, não cabe!";
                Description = "Teremos que procurar outra vaga";
            }
            var centeredStack = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = 10,
                Children =
                {
                    new Icon()
                    {
                        FileName = Img,
                        WidthRequest = App.ScreenWidth / 2.5 / 1.5,
                        HeightRequest = App.ScreenWidth / 2.5 / 1.5,
                        Margin = new Thickness(20),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Color = ColorPalette.Pink
                    },
                    new Label()
                    {
                        Text = Title.ToUpper(),
                        TextColor = Color.Black,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 21,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    new Label()
                    {
                        TextColor = ColorPalette.LightBlue,
                        Text = Description,
                        FontSize = 19,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Center,
                    }
                }
            };

            RoundedButton finalizeButton = new RoundedButton()
            {
                Text = "Finalizar".ToUpper(),
                BackgroundColor = ColorPalette.Pink,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.End,
                Margin = 30,
                Command = ViewModel.CloseApp,
            };
            RoundedButton returnintoButton = new RoundedButton()
            {
                Text = "Voltar para inicio".ToUpper(),
                BackgroundColor = ColorPalette.Pink,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.End,
                Margin = 30,
                Command = ViewModel.Continue,
            };
            Children.Add(navBar);
            Children.Add(centeredStack);
            Children.Add(finalizeButton);
            Children.Add(returnintoButton);
        }
    }
}
