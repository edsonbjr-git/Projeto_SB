using BalizaFacil.UI;
using BalizaFacil.Utils;
using System;
using Xamarin.Forms;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace BalizaFacil.Screens
{
    public class ScanningView : StackLayout
    {

        public event Action<string> Scanned;

        public ScanningView()
        {
            ConfigureScreen();
        }

        private void ConfigureScreen()
        {
            var navBar = BaseContentPage.Instance.ConfigureNavBar("Escanear Código");

            CustomLabel label = new CustomLabel()
            {
                Margin = new Thickness(70, 20),
                Text = "Na embalagem recebida encontra-se o código qr".ToUpper(),
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = 15,
                TextColor = Color.Black
            };

            var scanner = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 0, 0, 60),
            };

            scanner.OnScanResult += OnScanResult;
            scanner.IsScanning = true;

            StackLayout footer = new StackLayout()
            {
                BackgroundColor = ColorPalette.LightBlue,
                Orientation = StackOrientation.Horizontal,
                HeightRequest = 60,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.End,
                Children =
                {
                    new Button()
                    {
                        TextColor = ColorPalette.Pink,
                        Text = "Cancelar".ToLower(),
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        HeightRequest = 60,
                        BackgroundColor = Color.Transparent,
                        Command = new Command( () =>
                        {
                            BaseContentPage.Instance.PopView();
                            scanner.OnScanResult -= OnScanResult;
                            scanner.IsScanning = false;
                        })
                    }
                },
            };

            Grid wrapperGrid = new Grid()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            wrapperGrid.RowDefinitions.Add(new RowDefinition());
            wrapperGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            wrapperGrid.Children.Add(scanner, 0, 0);
            wrapperGrid.Children.Add(footer, 0, 0);


            Children.Add(navBar);
            Children.Add(label);
            Children.Add(wrapperGrid);

        }

        private void OnScanResult(Result result)
        {
            OnScanned(result.Text);
        }

        internal void OnScanned(string result)
        {
            Scanned?.Invoke(result);
        }
    }
}
