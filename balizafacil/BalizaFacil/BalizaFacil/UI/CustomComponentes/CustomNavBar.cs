using System;
using Xamarin.Forms;

namespace BalizaFacil.UI
{
    public class CustomNavBar : Grid
    {
        public event Action BackButtonClicked;
        public event Action MenuButtonClicked;

        public string Title { get => TitleLabel?.Text; set => TitleLabel.Text = value.ToUpper(); }
        public bool ShowBackButton { get => BackButton.Children[0].IsVisible; set => BackButton.Children[0].IsVisible = value; }
        public bool ShowMenuButton { get => MenuButton.Children[0].IsVisible; set => MenuButton.Children[0].IsVisible = value; }

        public Label TitleLabel { get; private set; }
        public StackLayout MenuButton { get; private set; }
        public StackLayout BackButton { get; private set; }

        private string BackButtonIconPath = "back.svg";
        private string MenuButtonIconPath = "menu.svg";

        public CustomNavBar(bool showBackButton, string title, bool showMenuButton)
        {
            int NavHeight = 60;
            int ImageHeight = 30;

            this.HeightRequest = NavHeight;
            this.BackgroundColor = ColorPalette.LightBlue;
            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.RowDefinitions.Add(new RowDefinition() { Height = NavHeight });
            this.ColumnDefinitions.Add(new ColumnDefinition() { Width = NavHeight });
            this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(10, GridUnitType.Star) });
            this.ColumnDefinitions.Add(new ColumnDefinition() { Width = NavHeight });

            BackButton = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.Start,
                Children = {
                    new Icon()
                    {
                        WidthRequest = ImageHeight,
                        HeightRequest = ImageHeight,
                        FileName = BackButtonIconPath,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        IsVisible = showBackButton,
                        Color = Color.White
                    }
                },
                HeightRequest = NavHeight,
                WidthRequest = NavHeight
            };
            BackButton.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    try
                    {
                        if (BackButton.IsVisible) BackButtonClicked?.Invoke();
                    }
                    catch (System.Exception ex)
                    {
                        string s = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                        BalizaFacil.App.Instance.UnhandledException(s, ex);
                    }
                })
            });


            MenuButton = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.End,
                Children = {
                    new Icon()
                    {
                        WidthRequest = ImageHeight,
                        HeightRequest = ImageHeight,
                        FileName = MenuButtonIconPath,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        IsVisible = showMenuButton,
                        Color = Color.White
                    }
                },

                HeightRequest = NavHeight,
                WidthRequest = NavHeight
            };
            MenuButton.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    try
                    {
                        if (MenuButton.IsVisible) MenuButtonClicked?.Invoke();
                    }
                    catch (System.Exception ex)
                    {
                        string _title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                        BalizaFacil.App.Instance.UnhandledException(_title, ex);
                    }

                })
            });

            TitleLabel = new Label()
            {
                Text = title.ToUpper(),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            if (showBackButton)
            {
                this.Children.Add(BackButton, 0, 0);
            }
            this.Children.Add(TitleLabel, 1, 0);
            this.Children.Add(MenuButton, 2, 0);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            try
            {
                base.LayoutChildren(x, y, width, height);

                TitleLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
                TitleLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void OnBackButtonClicked()
        {
            BackButtonClicked?.Invoke();
        }
    }
}
