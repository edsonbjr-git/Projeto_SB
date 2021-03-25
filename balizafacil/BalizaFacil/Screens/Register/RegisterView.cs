using BalizaFacil.Core;
using BalizaFacil.Services;
using BalizaFacil.UI;
using System;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class RegisterView : ScrollView
    {
        public RegisterViewModel ViewModel { get; set; }

        public RegisterView()
        {
            ViewModel = new RegisterViewModel();
            ConfigureScreen();
        }
        public RoundedEntry name { get; set; }
        public RoundedEntry birthday { get; set; }
        public DatePicker datePicker { get; set; }

        private void ConfigureScreen()
        {
            StackLayout socialMediaLoginButtons = new StackLayout()
            {
                Children =
                {
                    new SocialMediaButton()
                    {
                        HeightRequest = 60,
                        Margin = new Thickness(10, -10, 10, 0),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Button = new RoundedButton ()
                        {
                            Text = "Entre com o Facebook",
                            TextColor = Color.White,
                            FontAttributes = FontAttributes.Bold,
                            BackgroundColor = ColorPalette.LightBlue,
                            FontSize = 15,
                               Command = ViewModel.Register
                        },
                        Icon = new Icon()
                        {
                            FileName = "facebook.svg",
                            CentralizeHorizontally = false,
                            Color = Color.White
                        }
                    },
                    new SocialMediaButton()
                    {
                        HeightRequest = 60,
                        Margin = new Thickness(10, -20, 10, 0),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Button = new RoundedButton ()
                        {
                            Text = "Entre com o Google",
                            TextColor = Color.White,
                            FontAttributes = FontAttributes.Bold,
                            BackgroundColor = ColorPalette.LightBlue,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            FontSize = 15,
                               Command = ViewModel.Register
                        },
                        Icon = new Icon()
                        {
                            FileName = "gmail.svg",
                            CentralizeHorizontally = false,
                            //Margin = 5,
                            Color = Color.White
                        },

                    }
                }
            };
            Margin = new Thickness(15, 25);
            VerticalOptions = LayoutOptions.Fill;


            name = new RoundedEntry()
            {
                Placeholder = "Nome",
                Margin = new Thickness(20, 0, 20, 0),
                PlaceholderColor = ColorPalette.DarkBlue,
                TextAreaColor = Color.White,
                FontSize = 13,
                FontAttributes = FontAttributes.Bold
            };

             datePicker = new DatePicker
            {
                MinimumDate = new DateTime(2018, 1, 1),
                MaximumDate = new DateTime(2018, 12, 31),
                Date = new DateTime(2018, 6, 21)
            };
            
            birthday = new RoundedEntry()
            {
                Placeholder = "Nascimento",
                Margin = new Thickness(20, 0, 20, 0),
                PlaceholderColor = ColorPalette.DarkBlue,
                TextAreaColor = Color.White,
                FontSize = 13,
                
                FontAttributes = FontAttributes.Bold
            };

            Content = new StackLayout()
            {
                Children =
                    {
                        new Icon()
                        {
                            Margin = new Thickness(0, 8, 0, 0),
                            FileName = "logo_baliza.svg",
                            WidthRequest = App.ScreenWidth / 3,
                            HeightRequest = App.ScreenWidth / 3,
                            Color = Color.White,
                            HorizontalOptions = LayoutOptions.Center,
                        },
                        new Label()
                        {
                            TextColor = Color.White,
                            Text = "cadastro".ToUpper(),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            FontSize = 16,
                            Margin = new Thickness(10)
                        },
                        name,
                        birthday,
                        //new Label()
                        //{
                        //    TextColor = Color.White,
                        //    Text = "ou",
                        //    HorizontalTextAlignment = TextAlignment.Center,
                        //    HorizontalOptions = LayoutOptions.CenterAndExpand,
                        //    FontSize = 15
                        //},
                         //socialMediaLoginButtons,
                        new Button()
                        {
                            BackgroundColor = ColorPalette.Pink,
                            TextColor = Color.White,
                            Text = "Cadastrar".ToUpper(),
                            FontAttributes = FontAttributes.Bold,
                                FontSize = 17,
                            BorderRadius = 18,
                            Margin = 10,
                            HeightRequest = 55,
                            //Padding = new Thickness(13),
                            VerticalOptions = LayoutOptions.EndAndExpand,
                            Command = ViewModel.Register
                        },
                    }
            };
            name.TextChanged += CompletedText;
            
        }

        private void CompletedText(object sender, EventArgs e)
        {
            if (sender.Equals(name))
            {
                ViewModel.Username = name.Text;
            }
            else
                ViewModel.Birthday = datePicker.Date;


        }

        Entry CreateEntry(string placeholder = "", int marginBottom = 0, string bindingPropName = "")
        {
            RoundedEntry entry = new RoundedEntry()
            {
                Placeholder = placeholder,
                Margin = new Thickness(20, 0, 20, marginBottom),
                PlaceholderColor = ColorPalette.DarkBlue,
                TextAreaColor = Color.White,
                FontSize = 13,
                FontAttributes = FontAttributes.Bold
            };

            if (!string.IsNullOrWhiteSpace(bindingPropName))
                entry.SetBinding(Entry.TextProperty, new Binding(bindingPropName, source: ViewModel));

            return entry;
        }

    }
}
