//using BalizaFacil.UI;
//using BalizaFacil.Utils;
//using Xamarin.Forms;

//namespace BalizaFacil.Screens
//{
//    public class ChooseCarView : BaseContentPage
//    {
//        public ChooseCarViewModel ViewModel { get; set; }
//        public ChooseCarView()
//        {
//            ViewModel = new ChooseCarViewModel();
//            this.BindingContext = ViewModel;

//            ConfigureScreen();
//        }

//        private void ConfigureScreen()
//        {

//            Label subtitleLabel = new Label()
//            {
//                Text = "Modelos populares",
//                VerticalOptions = LayoutOptions.Center,
//                FontSize = 13,
//                TextColor = ColorPalette.Gray,
//                Margin = new Thickness(20, 0, 0, 0)
//            };

//            Frame subtitleFrame = new Frame()
//            {
//                BackgroundColor = ColorPalette.ExtraLightGray,
//                Content = subtitleLabel,
//                Margin = new Thickness(0, 30, 0,0),
//                Padding = new Thickness(5)
//            };

//            ListView carOptions = new ListView()
//            {
//                Margin = new Thickness(0, -6 , 0, 0),
//                ItemsSource = ViewModel.Options,
//                SeparatorColor = ColorPalette.LightGray,
//                ItemTemplate = new DataTemplate(() =>
//                {
//                    Label label = new Label()
//                    {
//                        VerticalTextAlignment = TextAlignment.Center,
//                        Margin = new Thickness(10, 0, 0, 0),
//                        FontSize = 16,
//                        TextColor = ColorPalette.Gray,
//                        FontAttributes = FontAttributes.Bold
//                    };
//                    label.SetBinding(Label.TextProperty, new Binding("Name"));

//                    return new CustomViewCell(label)
//                    {
//                        View = label,
//                        SelectedTextColor = ColorPalette.Pink,
//                        NormalTextColor = label.TextColor,
//                    };
//                })
//            };
//            carOptions.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(ViewModel.SelectedOption)));

//            Button selectOption = new Button()
//            {
//                Text = "Selecionar carro",
//                Command = ViewModel.SelectOption,
//                IsEnabled = false,
//                VerticalOptions = LayoutOptions.End
//            };
//            selectOption.SetBinding(Button.IsEnabledProperty, new Binding(nameof(ViewModel.SelectedOption), converter: new ValueConverter((value) => value as Models.Car != null)));

//            Content = new StackLayout
//            {
//                Children =
//                {
//                    //new Label() {Text = "Configurar carro", HorizontalOptions = LayoutOptions.FillAndExpand, FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.Center, Margin = new Thickness(10)},
//                    //new Label() {Text = "Vamos começar configurando seu carro, selecione uma das opções abaixo", HorizontalOptions = LayoutOptions.FillAndExpand,  FontSize = 15, HorizontalTextAlignment = TextAlignment.Center, Margin = new Thickness(0,0,0,10)},
//                    subtitleFrame,
//                    carOptions,
//                    selectOption
//                }
//            };
//        }
//    }
//}