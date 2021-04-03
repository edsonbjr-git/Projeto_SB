using BalizaFacil.Core;
using BalizaFacil.Models;
using BalizaFacil.Services;
using BalizaFacil.UI;
using BalizaFacil.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

using ZXing;
using ZXing.Net.Mobile.Forms;
using ZXing.QrCode;

namespace BalizaFacil.Screens.Configuration
{
    class ConfigurationView : ScrollView, IAppearView
    {
        public ConfigurationViewModel ViewModel { get; set; }
        IStorageService Storage => ServicesManager.Instance.Storage;
        public Car Car { get; set; }


        #region Components
        Label description { get; set; }
        RoundedEntry TotaldePares { get; set; }
        CustomLabel trainingLabel { get; set; }
        Switch trainingSwitcher { get; set; }
        Button updateCar { get; set; }
        CustomLabel updateLabel { get; set; }
        Label fatorMultiLabel { get; set; }
        Entry fatorMulti { get; set; }
        
        public ValueConverter Converter { get; set; }
        public CustomNavBar NavBar { get; set; }
        #endregion


        public ConfigurationView(bool closeOnComplete = false)
        {
            ViewModel = new ConfigurationViewModel(closeOnComplete);    
            BindingContext = ViewModel;
            Converter = new ValueConverter(ConvertValue, ConvertBack);
            Car = Storage.Car;
            ConfigureScreen();
        }

        private void ConfigureScreen()
        {
            NavBar = BaseContentPage.Instance.ConfigureNavBar("Configurações".ToUpper(), true, true);
            

            StackLayout StopSteps = GetStepEntries("Pares por etapa", 
                                                $"{nameof(ViewModel.Storage)}.{nameof(ViewModel.Storage.TotalStops)}",1);
            StackLayout reactionTime = GetStepEntries("Tempo de reação em milisegundos",
                                                $"{nameof(ViewModel.Storage)}.{nameof(ViewModel.Storage.reactionTime)}",2);
            StackLayout userName = GetStepEntries("Usuario",
                                                $"{nameof(ViewModel.Storage)}.{nameof(ViewModel.Storage.Username)}",3);

            StackLayout training = GetModeEntries("Modo de treinamento",
                                                 $"{ViewModel.Storage.TrainingMode}".ToLower(),1);
            StackLayout finalStep = GetModeEntries("Ultima etapa livre",
                                                  $"{ViewModel.Storage.FinalStepFree}".ToLower(),2);


            updateLabel = new CustomLabel()
            {
                Text = "Você tem atualizações no modelo do seu carro!",
                FontSize = 18,
                TextColor = Color.Black,
                Margin = new Thickness(60, 0, 60, 20),
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,

            };

            updateCar = new Button()
            {
                Text = "Atualizar",
                BackgroundColor = ColorPalette.Pink,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                Margin = new Thickness(20, 0),
                //Padding = new Thickness(12),
                BorderRadius = 18,
                HeightRequest = 55,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.End,
                Command = ViewModel.UpdateCar,

            };

            description = new Label()
                {
                    Text = "Carro: " + Car.Name + " " + Car.Style + ", \nRW: " + Math.Round(Convert.ToDecimal(Car.WheelRadiusExternal), 4),// + "\n MAC Adress: " + Storage.Address,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontSize = 18,
                    Margin = new Thickness(0, 0, 0, 10)
                };

            //new CustomLabel()
            //{
            //    Text = "Carro: " + Car.Name + " " + Car.Style + ", \nRW: " + Math.Round(Convert.ToDecimal(Car.WheelRadiusExternal), 4),// + "\n MAC Adress: " + Storage.Address,
            //    FontSize = 18,
            //    TextColor = Color.Black,
            //    Margin = new Thickness(60, 0, 60, 20),
            //    FontAttributes = FontAttributes.Bold,
            //    HorizontalTextAlignment = TextAlignment.Center,
            //};
            
            Content = new StackLayout()
            {
                Children =
                {
                    NavBar,
                    userName,
                    StopSteps,
                    reactionTime,
                    finalStep,
                    training,
                    description,
                    
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


        public void trainingSwitcher_Toggled(object sender, ToggledEventArgs e)
        {
            Storage.TrainingMode = !Storage.TrainingMode;

        }
        public void finalStepSwitcher_Toggled(object sender, ToggledEventArgs e)
        {
            Storage.FinalStepFree = !Storage.FinalStepFree;

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
       
        StackLayout GetModeEntries(string labelText, string bindingPropRight, int type)
        {
            Label headLabel = new Label()
            {
                Text = labelText,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 18,
                Margin = new Thickness(0, 0, 0, 10)
            };
            bool typeToggled = type == 1 ? Storage.TrainingMode : Storage.FinalStepFree;
            Switch right = new Switch
            {
                //Margin = 20,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsToggled = typeToggled

            };
            
            if(type == 1)
                right.Toggled += trainingSwitcher_Toggled;
            else
                right.Toggled += finalStepSwitcher_Toggled;

            right.SetBinding(RoundedEntry.TextProperty, new Binding(bindingPropRight));

            StackLayout stack = new StackLayout()
            {
                Children = {
                    
                    new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children = { headLabel, right }
                    }
                },
                Margin = 10
            };

            stack.BindingContext = ViewModel;

            return stack;
        }

        StackLayout GetStepEntries(string labelText, string bindingPropRight, int n)
        {
            Label headLabel = new Label()
            {
                Text = labelText,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 18,
                Margin = new Thickness(0, 0, 0, 10)
            };

            RoundedBorderedEntry right = n == 1 ?  GetEntry("Total de pares") : n == 2 ? GetEntry("Tempo de reação") : GetEntryText("Usuario");

            right.SetBinding(RoundedEntry.TextProperty, new Binding(bindingPropRight));

            StackLayout stack = new StackLayout()
            {
                Children = {
                    headLabel,
                    new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children = { right}
                    }
                },
                Margin = 10
            };

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
