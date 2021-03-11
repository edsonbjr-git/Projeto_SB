using BalizaFacil.Core;
using BalizaFacil.Models;
using BalizaFacil.Services;
using BalizaFacil.UI;
using System;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class WaitingSensorConnectionView : Grid
    {
        public CustomNavBar NavBar { get; set; }

        public event Action Cancelled;

        public WaitingSensorConnectionView()
        {
            ConfigureScreen();

            FlowManager.Instance.ConnectToSensor();
        }

        private void ConfigureScreen()
        {
            //ServicesManager.Instance.SoundPlayer.StopSound();
            //BackgroundColor = Color.White;
            NavBar = BaseContentPage.Instance.ConfigureNavBar("Conectando");

            BoxView whiteBox = new BoxView()
            {
                BackgroundColor = Color.White
            };
            BoxView lightBlueBox = new BoxView()
            {
                Color = ColorPalette.LightBlue
            };

            CustomLabel text = new CustomLabel()
            {
                Margin = new Thickness(70, 15),
                BackgroundColor = Color.White,
                Text = "Conectando o sensor ao bluetooth".ToUpper(),
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 15,
                TextColor = Color.Black,
            };
            
            
            //UI.Animation directionGIF = new UI.Animation(AnimationType.Wainting);

            GIF gif = new GIF()
            {
                //Source = ImageSource.FromFile("wainting.gif"),
                GIFSource = "loading",
                Aspect = Aspect.AspectFill,
                IsOpaque = true
            };


            Button cancelButton = new Button()
            {
                TextColor = ColorPalette.Pink,
                Text = "Cancelar".ToLower(),
                HeightRequest = 60,
                BackgroundColor = Color.Transparent,
                Command = new Command(() =>
                {
                    Cancelled?.Invoke();
                })
            };


            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(60, GridUnitType.Auto) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });


            Children.Add(gif, 0, 2);
            //Children.Add(directionGIF, 0, 2);

            Children.Add(whiteBox, 0, 0);
            Children.Add(NavBar, 0, 0);
            Children.Add(text, 0, 1);
            Children.Add(lightBlueBox, 0, 3);
            Children.Add(cancelButton, 1, 3);

            Grid.SetRowSpan(whiteBox, 2);
            Grid.SetColumnSpan(gif, 2);
            Grid.SetColumnSpan(whiteBox, 2);
            Grid.SetColumnSpan(NavBar, 2);
            Grid.SetColumnSpan(text, 2);
            Grid.SetColumnSpan(lightBlueBox, 2);

            NavBar.TitleLabel.HorizontalTextAlignment = TextAlignment.Center;
            NavBar.TitleLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
            NavBar.TitleLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;

        }
        
    }
}
