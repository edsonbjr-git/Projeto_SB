using BalizaFacil.Core;
using BalizaFacil.Screens;
using BalizaFacil.UI;
using BalizaFacil.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    class ReportsView :  StackLayout
    {
        ReportsViewModel viewModel { get; set; }

        public ReportsView()
        {
            viewModel = new ReportsViewModel();
            viewModel.GetHistorical();
           
            
            ConfigureScreen();
        }
        void ConfigureScreen()
        {
            var navBar = BaseContentPage.Instance.ConfigureNavBar("", true, true);
            
            var stack = new StackLayout{
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children = {
                  
                    new Label {
                        Margin = 15,
                        Text = viewModel.ToString(),
                        FontSize = 16,
                        TextColor = Color.Black,
                        HorizontalTextAlignment = TextAlignment.Center,
                    } 
                } 
            };
            ScrollView sv = new ScrollView();

            sv.Content = stack;
            

            Button enviar = new Button
            {
                Text = "Enviar",
                BackgroundColor = ColorPalette.LightBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                BorderRadius = 18,
                Margin = 10,
                HeightRequest = 55,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.End,
                //Margin = new Thickness(40, 0, 40, 20),
                Command = viewModel.Continue,
            };
            
            Label qt = new Label()
            {
                Text = "Balizas realizadas: " + viewModel.reports.Count,
                FontSize = 18,
                TextColor = Color.Black,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            
            Children.Clear();
            Children.Add(navBar);
            Children.Add(qt);
            Children.Add(sv);
            Children.Add(enviar);
            
        }
       

    }

}
