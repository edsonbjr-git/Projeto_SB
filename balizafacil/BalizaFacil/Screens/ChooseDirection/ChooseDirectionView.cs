using BalizaFacil.Models;
using BalizaFacil.UI;
using System;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class ChooseDirectionView : Grid
    {
        public ChooseDirectionViewModel ViewModel { get; set; }
        public ChooseDirectionView()
        {
            ViewModel = new ChooseDirectionViewModel();
            ConfigureScreen();
        }
        

        private void ConfigureScreen()
        {
            BackgroundColor = Color.White;
            float GIFRatio = 0.78409f;
            float GIFWidth = App.ScreenWidth * 0.5f;
            float GIFHeight = App.ScreenWidth * 0.5f / GIFRatio;
            
            RowDefinitions.Add(new RowDefinition() { Height = 60 }); // NavBar
            RowDefinitions.Add(new RowDefinition() { Height = (App.ScreenHeight - 60 - GIFHeight - 50) / 2 }); // Titulo
            //RowDefinitions.Add(new RowDefinition() { Height = 30 }); // 
            RowDefinitions.Add(new RowDefinition() { Height = 50 });
            RowDefinitions.Add(new RowDefinition() { Height = GIFHeight });
            RowDefinitions.Add(new RowDefinition() { Height = (App.ScreenHeight - 60 - GIFHeight - 50) / 2 });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = App.ScreenWidth / 2 });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = App.ScreenWidth / 2 });

            CustomNavBar navBar = BaseContentPage.Instance.ConfigureNavBar("", true, true);

            Label title = new Label()
            {
                Text = "Escolha sua vaga".ToUpper(),
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.End,
                Margin = new Thickness(70, 40),
                FontSize = 20,
                TextColor = Color.Black
            };
            
            GIF left = new GIF()
            {
                WidthRequest = GIFWidth,
                HeightRequest = GIFHeight,
                GIFSource = "left_choice",
                Margin = new Thickness(5, 0, 0, 0),
                OnClickParameter = Direction.Left,
                OnClick = ViewModel.DirectionChoosed
            };

            GIF right = new GIF()
            {
                WidthRequest = GIFWidth,
                HeightRequest = GIFHeight,
                GIFSource = "right_choice",
                Margin = new Thickness(0, 0, 5, 0),
                OnClickParameter = Direction.Right,
                OnClick = ViewModel.DirectionChoosed
            };

            Label rightLabel = new Label()
            {
                Text = "Direita".ToUpper(),
                TextColor = Color.Black,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.End,
                FontSize = 16,
            };

            Label leftLabel = new Label()
            {
                Text = "Esquerda".ToUpper(),
                TextColor = Color.Black,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.End,
                FontSize = 16,
            };

            Button checkTest = new Button
            {
                Text = "Modo de treinamento".ToUpper(),
                //Margin = new Thickness(70, 50),
                BackgroundColor = ColorPalette.LightBlue,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End, 
                FontSize = 16,
                //Command = ChooseCarViewModel.checkTest,
            };
            checkTest.Clicked += (sender, e) =>
            {
                if (checkTest.Text == "Modo de treinamento".ToUpper())
                {
                    checkTest.Text = "Modo padrão".ToUpper();
                    StepViewModel.TrainingMode = true;
                }
                else
                {
                    checkTest.Text = "Modo de treinamento".ToUpper();
                    StepViewModel.TrainingMode = false;
                }
            };
            Children.Add(navBar, 0, 0);
            Children.Add(title, 0, 1);
            //Children.Add(checkTest);
            Children.Add(leftLabel, 0, 2);
            Children.Add(rightLabel, 1, 2);
            Children.Add(left, 0, 3);
            Children.Add(right, 1, 3);

            Grid.SetColumnSpan(navBar, 2);
            Grid.SetColumnSpan(title, 2);
        }

    }
}