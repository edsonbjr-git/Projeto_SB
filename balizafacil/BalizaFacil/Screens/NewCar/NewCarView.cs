using BalizaFacil.UI;
using BalizaFacil.Utils;
using System;
using System.Globalization;
using Xamarin.Forms;
using BalizaFacil.Core;

namespace BalizaFacil.Screens
{
    public class NewCarView : ScrollView, IAppearView
    {
        public NewCarViewModel ViewModel { get; set; }
        public ValueConverter Converter { get; set; }
        public CustomNavBar NavBar { get; set; }

        public NewCarView(bool closeOnComplete = false)
        {
            ViewModel = new NewCarViewModel(closeOnComplete);
            BindingContext = ViewModel;
            Converter = new ValueConverter(ConvertValue, ConvertBack);
            ConfigureScreen();
        }

        private void ConfigureScreen()
        {
            NavBar = BaseContentPage.Instance.ConfigureNavBar("Carro personalizado".ToUpper(), true, true);
            NavBar.BackButton.GestureRecognizers.Clear();
            NavBar.BackButton.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    BaseContentPage.Instance.PopModal();
                })
            });

            RoundedBorderedEntry car = GetEntryText("Marca");
            car.SetBinding(RoundedEntry.TextProperty, new Binding($"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.Name)}"));

            RoundedBorderedEntry style = GetEntryText("Modelo");
            style.SetBinding(RoundedEntry.TextProperty, new Binding($"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.Style)}"));

            RoundedBorderedEntry parkingSpace = GetEntry("Tamanho minimo de vaga (CM)");
            parkingSpace.SetBinding(RoundedEntry.TextProperty, new Binding($"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.ParkingSpace)}", converter: Converter));

            RoundedBorderedEntry wheelRadiusInt = GetEntry("Raio da roda Interno");
            wheelRadiusInt.SetBinding(RoundedEntry.TextProperty, new Binding($"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.WheelRadiusInternal)}"));

            RoundedBorderedEntry wheelRadiusExt = GetEntryText("Raio da roda Externo");
            wheelRadiusExt.SetBinding(RoundedEntry.TextProperty, new Binding($"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.WheelRadiusExternal)}"));



            Label passosDaBaliza = new Label() { FontSize = 16, Text = "Passos da baliza (em CM, sempre positivo)" };

            StackLayout first = GetStepEntries("Primeiro passo - Centralizado/Frente", 
                                                $"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.FirstLeft)}", 
                                                $"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.FirstRight)}");
            StackLayout second = GetStepEntries("Segundo passo - Esquerda/Trás", 
                                                $"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.SecondLeft)}", 
                                                $"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.SecondRight)}");
            StackLayout third = GetStepEntries("Terceiro passo - Centralizado/Trás", 
                                                $"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.ThirdLeft)}", 
                                                $"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.ThirdRight)}");
            StackLayout fourth = GetStepEntries("Quarto passo - Esquerda/Frente", 
                                                $"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.FourthLeft)}", 
                                                $"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.FourthRight)}");
            StackLayout fifth = GetStepEntries("Quinto passo - Direita/Frente", 
                                                $"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.FifthLeft)}", 
                                                $"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.FifthRight)}");
            StackLayout sixth = GetStepEntries("Sexto passo - Esquerda/Frente", 
                                                $"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.SixthLeft)}", 
                                                $"{nameof(ViewModel.Car)}.{nameof(ViewModel.Car.SixthRight)}");

            var button = new Button()
            {
                Text = "Concluido",
                BackgroundColor = ColorPalette.LightBlue,
                TextColor = Color.White,
                BorderRadius = 18,
                Margin = 10,
                HeightRequest = 55,
                FontSize = 17,
                Command = ViewModel.Continue,
            };

            Content = new StackLayout()
            {
                Children =
                {
                    NavBar,
                    car,
                    style,
                    parkingSpace,
                    wheelRadiusExt,
                    wheelRadiusInt,
                    first,
                    second,
                    third,
                    fourth,
                    fifth,
                    sixth,
                    button
                }
            };
        }

        private object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return double.Parse(value.ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private object ConvertValue(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = double.Parse(value.ToString());
            if (val == 0)
                return "";
            else
                return val.ToString();
        }

        RoundedBorderedEntry GetEntry(string placeholder)
        {
            return new RoundedBorderedEntry()
            {
                PlaceholderColor = ColorPalette.DarkBlue,
                Placeholder = placeholder,
                FontAttributes = FontAttributes.Bold,
                FontSize = 16,
                TextColor = ColorPalette.DarkBlue,
                Padding = new Thickness(50, 20),
                BorderColor = ColorPalette.DarkBlue,
                BorderWidth = App.ScreenWidth / 154,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Keyboard = Keyboard.Numeric
            };
        }
        RoundedBorderedEntry GetEntryText(string placeholder)
        {
            return new RoundedBorderedEntry()
            {
                PlaceholderColor = ColorPalette.DarkBlue,
                Placeholder = placeholder,
                FontAttributes = FontAttributes.Bold,
                FontSize = 16,
                TextColor = ColorPalette.DarkBlue,
                Padding = new Thickness(50, 20),
                BorderColor = ColorPalette.DarkBlue,
                BorderWidth = App.ScreenWidth / 154,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
        }
        

        StackLayout GetStepEntries(string labelText, string bindingPropLeft, string bindingPropRight)
        {
            Label headLabel = new Label()
            {
                Text = labelText,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 18,
                Margin = new Thickness(0, 0, 0, 10)
            };

            RoundedBorderedEntry left = GetEntry("Esquerda");
            RoundedBorderedEntry right = GetEntry("Direita");

            left.SetBinding(RoundedEntry.TextProperty, new Binding(bindingPropLeft));
            right.SetBinding(RoundedEntry.TextProperty, new Binding(bindingPropRight));

            StackLayout stack = new StackLayout()
            {
                Children = {
                    headLabel,
                    new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children = { left, right}
                    }
                },
                Margin = 10
            };
            /*
            if (App.IsAndroidSDKBelowMarshmallow)
            {

                var propLeft = bindingPropLeft.Split('.')[1];
                var propRight = bindingPropRight.Split('.')[1];

                ViewModel.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == propLeft)
                        left.Text = ViewModel.GetType().GetProperty(propLeft).GetValue(ViewModel).ToString();
                    else if (args.PropertyName == propRight)
                        right.Text = ViewModel.GetType().GetProperty(propLeft).GetValue(ViewModel).ToString();
                };

                left.TextChanged += (sender, args) =>
                {
                    double value = 0;
                    double.TryParse(left.Text, out value);
                    var viewmodelType = typeof(NewCarViewModel);
                    var prop = viewmodelType.GetProperty(propLeft);
                    prop.SetValue(ViewModel, value);
                };

                right.TextChanged += (sender, args) =>
                {
                    double value = 0;
                    double.TryParse(right.Text, out value);
                    var viewmodelType = typeof(NewCarViewModel);
                    var prop = viewmodelType.GetProperty(propRight);
                    prop.SetValue(ViewModel, value);
                    //ViewModel.GetType().GetProperty(propRight).SetValue(ViewModel, value);
                };

            }
            */
            stack.BindingContext = ViewModel;

            return stack;
        }


        public void OnAppearing()
        {
            NavBar.TitleLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
            NavBar.TitleLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
        }
    }
}
