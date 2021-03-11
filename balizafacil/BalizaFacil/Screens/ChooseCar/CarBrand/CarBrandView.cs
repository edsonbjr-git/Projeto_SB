using BalizaFacil.Core;
using BalizaFacil.Models;
using BalizaFacil.UI;
using BalizaFacil.Utils;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class CarBrandView : StackLayout, IAppearView
    {
        public CarBrandViewModel ViewModel { get; set; }

        public Icon SearchIcon { get; set; }
        public RoundedBorderedEntry SearchEntry { get; set; }
        CarSelectionList optionsList { get; set; }
        public Grid SearchGrid { get; set; }

        public CustomNavBar NavBar { get; set; }
        public CarBrandView(bool closeOnComplete = false)
        {
            ViewModel = new CarBrandViewModel(closeOnComplete);
            BindingContext = ViewModel;
            ConfigureScreen();
        }

        
        void ConfigureScreen()
        {
            NavBar = BaseContentPage.Instance.ConfigureNavBar("Adicionar carro", true);

            SearchEntry = new RoundedBorderedEntry()
            {
                CenteredPlaceholder = true,
                PlaceholderColor = ColorPalette.DarkBlue,
                Placeholder = "Qual a marca do seu carro?".ToUpper(),
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                TextColor = ColorPalette.DarkBlue,
                BorderColor = ColorPalette.DarkBlue,
                BorderWidth = App.ScreenWidth / 72,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            

            SearchIcon = new Icon()
            {
                HeightRequest = 25,
                WidthRequest = 25,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(15, 0, 0, 0),
                Color = ColorPalette.DarkBlue,
                FileName = "search.svg",

            };
            SearchIcon.SetBinding(VisualElement.IsVisibleProperty, new Binding(nameof(SearchEntry.Text), source: SearchEntry, converter: new ValueConverter((text) => string.IsNullOrWhiteSpace((string)text))));

            SearchGrid = new Grid()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(20, 30, 20, 30),
            };

            SearchGrid.RowDefinitions.Add(new RowDefinition());
            SearchGrid.ColumnDefinitions.Add(new ColumnDefinition());

            Label textSubtitleLabel = new Label()
            {
                Text = "Marcas populares",
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
            int counter = 0;
            optionsList = new CarSelectionList()
            {
                ItemsSource = ViewModel.BrandOptions,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ItemTemplate = new DataTemplate(() =>
                {
                    var option = ViewModel.BrandOptions[counter];
                    CustomLabel label = new CustomLabel()
                    {
                        VerticalTextAlignment = TextAlignment.Center,
                        CustomPadding = new Thickness(10, 0, 0, 0),
                        Text = option.Name,
                        FontSize = 16,
                        TextColor = option == ViewModel.SelectedBrand ? ColorPalette.Pink : ColorPalette.Gray,
                        FontAttributes = FontAttributes.Bold,
                        BackgroundColor = Color.White
                    };
                    CustomViewCell cell = new CustomViewCell(label)
                    {
                        View = label,
                        SelectedTextColor = ColorPalette.Pink,
                        NormalTextColor = label.TextColor,
                    };

                    counter++;
                    return cell;
                })
                
            };
            optionsList.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(ViewModel.SelectedBrand)));
            //optionsList.ItemTapped += ViewModel.OnItemTapped;
            if (App.IsAndroidSDKBelowMarshmallow)
                optionsList.ItemSelected += (sender, args) =>
                {
                    ViewModel.SelectedBrand = args.SelectedItem as CarFilterInfo;
                    optionsList.Layout(optionsList.Bounds);

                };



            Button confirmatioButton = new Button()
            {
                
                BackgroundColor = ColorPalette.LightBlue,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                Text = "Continuar".ToUpper(),
                //Margin = new Thickness(50, 0, 50, 30),
                FontSize = 18,
                VerticalOptions = LayoutOptions.End,
                //Padding = new Thickness(10),
                BorderRadius = 18,
                Margin = 10,
                HeightRequest = 55,
                Command = ViewModel.SelectOption
            };

            Children.Clear();
            Children.Add(NavBar);
            Children.Add(SearchEntry);
            Children.Add(SearchGrid);
            Children.Add(subtitleLabel);
            Children.Add(optionsList);
            Children.Add(confirmatioButton);
            SearchEntry.SizeChanged += OnSearchSizeChanged;
            SearchEntry.Completed += OnTextChanged;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            ViewModel.searchCar(SearchEntry.Text);
            ConfigureScreen();

        }
        private void OnSearchSizeChanged(object sender, EventArgs e)
        {
            SearchGrid.RowDefinitions[0].Height = SearchEntry.Height;

            Children.Remove(SearchEntry);

            SearchGrid.Children.Add(SearchIcon, 0, 0);
            SearchEntry.SizeChanged -= OnSearchSizeChanged;
            SearchGrid.Children.Add(SearchEntry, 0, 0);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            base.LayoutChildren(x, y, width, height);
        }
        
        public void OnAppearing()
        {
            ViewModel.SelectedBrand = null;
        }
    }
}
