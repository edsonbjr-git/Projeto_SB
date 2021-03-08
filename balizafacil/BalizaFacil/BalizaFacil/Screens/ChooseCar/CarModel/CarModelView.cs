using BalizaFacil.UI;
using BalizaFacil.Utils;
using System;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class CarModelView : ContentPage
        //: BaseContentPage
    {
        public CarModelViewModel ViewModel { get; set; }
        public Icon SearchIcon { get; set; }
        public RoundedBorderedEntry SearchEntry { get; set; }
        public Grid SearchGrid { get; set; }


        public CarModelView()
        {
            ViewModel = new CarModelViewModel();
            ConfigureScreen();
        }

        void ConfigureScreen()
        {
            var navBar = BaseContentPage.Instance.ConfigureNavBar("Adicionar carro", true, true);

            SearchEntry = new RoundedBorderedEntry()
            {
                CenteredPlaceholder = true,
                PlaceholderColor = ColorPalette.DarkBlue,
                Placeholder = "Qual o modelo do seu carro?".ToUpper(),
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                TextColor = ColorPalette.DarkBlue,
                BorderColor = ColorPalette.DarkBlue,
                BorderWidth = App.ScreenWidth / 154,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            
            SearchIcon = new Icon()
            {
                Color = ColorPalette.DarkBlue,
                FileName = "search.svg",
                HeightRequest = 25,
                WidthRequest = 25,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(15, 10, 0, 10)
            };

            SearchIcon.SetBinding(VisualElement.IsVisibleProperty, new Binding(nameof(SearchEntry.Text), source: SearchEntry, converter: new ValueConverter((text) => string.IsNullOrWhiteSpace((string)text))));

            SearchGrid = new Grid()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(20, 30, 20, 30),
            };

            SearchGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50, GridUnitType.Absolute) });
            SearchGrid.ColumnDefinitions.Add(new ColumnDefinition());


            Label textSubtitleLabel = new Label()
            {
                Text = "Modelos populares",
                FontSize = 14,
                TextColor = ColorPalette.Gray,
                Margin = new Thickness(20, 0, 0, 0),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            RoundedBorderedLabel subtitleLabel = new RoundedBorderedLabel(textSubtitleLabel, Color.Transparent, 0, ColorPalette.ExtraLightGray, new Thickness(0, 5), 0)
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            subtitleLabel.Content.HorizontalOptions = subtitleLabel.HorizontalOptions;

            CarSelectionList optionsList = new CarSelectionList()
            {
                ItemsSource = ViewModel.ModelOptions,
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            optionsList.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(ViewModel.SelectedModel)));

            RoundedBorderedEntry yearEntry = new RoundedBorderedEntry()
            {
                PlaceholderColor = ColorPalette.DarkBlue,
                Placeholder = "Ano",
                FontAttributes = FontAttributes.Bold,
                FontSize = 16,
                TextColor = ColorPalette.DarkBlue,
                Margin = new Thickness(50, 0, 50, 20),
                Padding = new Thickness(50, 20),
                BorderColor = ColorPalette.DarkBlue,
                BorderWidth = App.ScreenWidth / 72,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Keyboard = Keyboard.Numeric
            };

            RoundedButton confirmatioButton = new RoundedButton()
            {
                BackgroundColor = ColorPalette.LightBlue,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                Text = "Continuar".ToUpper(),
                Margin = new Thickness(50, 0, 50, 20),
                FontSize = 16,
                VerticalOptions = LayoutOptions.End,
                Padding = new Thickness(10),
                Command = ViewModel.SelectOption
            };



            Content = new StackLayout()
            {
                Children =
                {
                    navBar,
                    SearchGrid,
                    SearchEntry,
                    subtitleLabel,
                    optionsList,
                    yearEntry,
                    confirmatioButton
                }
            };

            SearchEntry.SizeChanged += OnSearchSizeChanged;
        }

        private void OnSearchSizeChanged(object sender, EventArgs e)
        {
            SearchGrid.RowDefinitions[0].Height = SearchEntry.Height;

            (Content as StackLayout).Children.Remove(SearchEntry);

            SearchGrid.Children.Add(SearchIcon, 0, 0);
            SearchEntry.SizeChanged -= OnSearchSizeChanged;

            SearchGrid.Children.Add(SearchEntry, 0, 0);
        }

    }
}
