using BalizaFacil.UI;
using BalizaFacil.Utils;
using System;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class CarTypeView : ContentPage
        //: BaseContentPage
    {
        public CarTypeViewModel ViewModel { get; set; }

        public Icon SearchIcon { get; set; }
        public RoundedBorderedEntry SearchEntry { get; set; }
        public Grid SearchGrid { get; set; }

        public CarTypeView()
        {
            ViewModel = new CarTypeViewModel();
            ConfigureScreen();
        }

        void ConfigureScreen()
        {
            //var navBar = ConfigureNavBar("Adicionar carro", true, true);
            var navBar = new StackLayout();

            SearchEntry = new RoundedBorderedEntry()
            {
                CenteredPlaceholder = true,
                PlaceholderColor = ColorPalette.DarkBlue,
                Placeholder = "Qual o tipo do seu carro?".ToUpper(),
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                TextColor = ColorPalette.DarkBlue,
                Padding = new Thickness(50, 30),
                BorderColor = ColorPalette.DarkBlue,
                BorderWidth = App.ScreenWidth / 72,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            this.SearchEntry.SizeChanged += OnSearchSizeChanged;
            SearchIcon = new Icon()
            {
                Color = ColorPalette.DarkBlue,
                FileName = "search.svg",
                HeightRequest = 25,
                WidthRequest = 25,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(15, 0, 0, 0)
            };

            SearchIcon.SetBinding(VisualElement.IsVisibleProperty, new Binding(nameof(SearchEntry.Text), source: SearchEntry, converter: new ValueConverter((text) => string.IsNullOrWhiteSpace((string)text))));

            SearchGrid = new Grid()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(20, 30, 20, 30),
            };

            SearchGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50, GridUnitType.Absolute) });
            SearchGrid.ColumnDefinitions.Add(new ColumnDefinition());

            SearchGrid.Children.Add(SearchEntry, 0, 0);
            SearchGrid.Children.Add(SearchIcon, 0, 0);

            Label textSubtitleLabel = new Label()
            {
                Text = "Tipos populares",
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
                ItemsSource = ViewModel.TypeOptions,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            optionsList.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(ViewModel.SelectedType)));


            RoundedButton confirmatioButton = new RoundedButton()
            {
                BackgroundColor = ColorPalette.LightBlue,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                Text = "Continuar".ToUpper(),
                Margin = new Thickness(50, 0, 50, 30),
                FontSize = 17,
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
                    confirmatioButton
                }
            };
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
