//using BalizaFacil.UI;
//using Xamarin.Forms;

//namespace BalizaFacil.Screens
//{
//    public class InitialWarningView : BaseContentPage
//    {
//        public InitialWarningView()
//        {
//            ConfigureScreen();
//        }

//        private void ConfigureScreen()
//        {
//            this.Content = new StackLayout()
//            {
//                VerticalOptions = LayoutOptions.Center,
//                HorizontalOptions = LayoutOptions.FillAndExpand,
//                Margin = 10,
//                Children =
//                {
//                    new Image
//                    {
//                        WidthRequest = App.ScreenWidth * 0.25,
//                        HeightRequest = App.ScreenWidth * 0.25,
//                        Source = "initial_warning_icon.jpg"
//                    },
//                    new Label()
//                    {
//                        Text = "Cuidado com objetos\npróximos à calçada".ToUpper(),
//                        TextColor = Color.Black,
//                        FontAttributes = FontAttributes.Bold,
//                        HorizontalTextAlignment = TextAlignment.Center,
//                        FontSize = 19
//                    },
//                    new Label()
//                    {
//                        Text = "VOCÊ POSSUI UM CARRO SEDAN\ne não queremos que o danifique",
//                        TextColor = ColorPalette.LightBlue,
//                        FontAttributes = FontAttributes.Bold,
//                        HorizontalTextAlignment = TextAlignment.Center,
//                        FontSize = 18,
//                        Margin = new Thickness(0, 10)
//                    }
//                }
//            };
//        }
//    }
//}
