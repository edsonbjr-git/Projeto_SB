using BalizaFacil.Core;
using BalizaFacil.Services;
using BalizaFacil.UI;
using System;
using System.Threading;
using Xamarin.Forms;
using BalizaFacil.Models;


namespace BalizaFacil.Screens
{
    public class WelcomeView : StackLayout//, IAppearView
    {
        public WelcomeViewModel ViewModel { get; set; }

        public Car Car;

        IStorageService Storage => ServicesManager.Instance.Storage;

        public WelcomeView()
        {
            ViewModel = new WelcomeViewModel();
            ServicesManager.Instance.SoundPlayer.StopSound();
            BindingContext = ViewModel;
            Car = Storage.Car;
            ConfigureScreen();
        }

        public void ConfigureScreen()
        {
            var navBar = BaseContentPage.Instance.ConfigureNavBar("", false, true);
            navBar.BackgroundColor = Color.Transparent;
            navBar.Margin = new Thickness(0, 20, 0, 0);
            Icon bluetooth = new Icon()
            {
                Color = Color.White,
                BackgroundColor = Color.FromRgba(0, 0, 0, .65),
                Margin = 20,
                FileName = "connection.svg",
                WidthRequest = App.ScreenWidth / 8,
                HeightRequest = App.ScreenHeight / 6,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            Icon logotipo = new Icon()
            {
                Color = Color.White,
                FileName = "logotipo.svg",
                WidthRequest = App.ScreenWidth / 2.25f,
                HeightRequest = App.ScreenWidth / 2.25f,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };

            Button button = new Button()
            {
                Text = "Começar".ToUpper(),
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                //Padding = 12,
                BorderRadius = 18,
                BackgroundColor = ColorPalette.Pink,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.End,
                Margin = 10,
                HeightRequest = 55,
                //Margin = new Thickness(40, 0, 40, 20),
                Command = ViewModel.Continue,

            };
            Children.Clear();
            Children.Add(navBar);
            Children.Add(logotipo);
            Children.Add(button);
        }
    }
}