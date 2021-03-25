using BalizaFacil.Services;
using BalizaFacil.UI;
using System;
using Xamarin.Forms;
using ZXing;
using ZXing.Net.Mobile.Forms;
using ZXing.QrCode;

namespace BalizaFacil.Screens
{
    public class ConfigureSensorAddressView : StackLayout
    {
        public ConfigureSensorAddressViewModel ViewModel { get; set; }
        public ConfigureSensorAddressView(bool closeOnComplete = false)
        {
            ViewModel = new ConfigureSensorAddressViewModel(closeOnComplete);

            ConfigureScreen();
        }
        ZXingBarcodeImageView qrCode;
        CustomLabel description { get; set; }

        IStorageService Storage => ServicesManager.Instance.Storage;
        void ConfigureScreen()
        {
            var navBar = BaseContentPage.Instance.ConfigureNavBar("Configurar sensor", true, true);

            description = new CustomLabel()
            {
                Text =" MAC Adress: " + Storage.Address,
                FontSize = 18,
                TextColor = Color.Black,
                Margin = new Thickness(60, 0, 60, 20),
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            qrCode = new ZXingBarcodeImageView
            {
                BackgroundColor = Color.White,
                BarcodeFormat = BarcodeFormat.QR_CODE,
                WidthRequest = App.ScreenWidth / 2f,
                HeightRequest = App.ScreenWidth / 2f,
                BarcodeOptions = new QrCodeEncodingOptions
                {
                    Height = 250,
                    Width = 250
                },
                BarcodeValue = Storage.Address,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            Label label = new Label()
            {
                TextColor = Color.Black,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                Margin = new Thickness(60, 0, 60, 20),
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Qual o mecanismo de conexão do sensor?".ToUpper()
            };

            StackLayout buttonsStack = new StackLayout()
            {
                Margin = new Thickness(20, 0),
                Children =
                {
                    new Button()
                    {
                        BackgroundColor = ColorPalette.LightBlue,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 18,
                        BorderRadius = 18,
                        Margin = 10,
                        HeightRequest = 55,
                        WidthRequest = App.ScreenWidth,
                        Text = "bluetooth".ToUpper(),
                        TextColor = Color.White,
                        Command = ViewModel.SearchByBluetooth
                    },
                    new Label()
                    {
                        Margin = 10,
                        HorizontalTextAlignment = TextAlignment.Center,
                        Text = "ou",
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 16
                    },
                    new Button()
                    {
                        FontAttributes = FontAttributes.Bold,
                        WidthRequest = App.ScreenWidth,
                        FontSize = 18,
                        BorderRadius = 18,
                        Margin = 10,
                        HeightRequest = 55,
                        BackgroundColor = ColorPalette.LightBlue,
                        Text = "qrcode".ToUpper(),
                        TextColor = Color.White,
                        Command = ViewModel.SearchByQRCode
                    }
                },
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            StackLayout centeredStack = new StackLayout()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    label,
                    buttonsStack
                }
            };


            Children.Add(navBar);
            if (!string.IsNullOrWhiteSpace(Storage.Address))
            {
                Children.Add(description);
                Children.Add(qrCode);
            }
                Children.Add(centeredStack);
        }

    }
}