using BalizaFacil.Models;
using BalizaFacil.UI;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class MeasurementResultView : StackLayout
    {
        public ParkingSpaceSizeType SizeType { get; set; }
        public string ImagePath
        {
            get
            {
                switch (SizeType)
                {
                    case ParkingSpaceSizeType.VeryEasy:
                    case ParkingSpaceSizeType.Easy:
                    case ParkingSpaceSizeType.Average:
                        return "meditation.svg";
                    case ParkingSpaceSizeType.Hard:
                    case ParkingSpaceSizeType.VeryHard:
                        return "warning.svg";
                }

                return "x.svg";
            }
        }

        public string Description
        {
            get
            {
                switch (SizeType)
                {
                    case ParkingSpaceSizeType.VeryEasy:
                        return "Eba! Essa vaga a gente faz com uma etapa a menos"; 
                    case ParkingSpaceSizeType.Easy:
                    case ParkingSpaceSizeType.Average:
                        return "Fique tranquilo e confie na gente";
                    case ParkingSpaceSizeType.Hard:
                    case ParkingSpaceSizeType.VeryHard:
                        return "Mas a gente consegue, vamos lá";
                }

                return "Teremos que procurar outra vaga";
            }
        }

        public string TitleLabelText
        {
            get
            {
                switch (SizeType)
                {
                    case ParkingSpaceSizeType.VeryEasy:
                    case ParkingSpaceSizeType.Easy:
                    case ParkingSpaceSizeType.Average:
                        return "Vamos começar";
                    case ParkingSpaceSizeType.Hard:
                    case ParkingSpaceSizeType.VeryHard:
                        return "Vaga Apertada";
                }

                return "Eita, não cabe!";
            }
        }

        public MeasurementResultView(ParkingSpaceSizeType sizeType)
        {
            SizeType = sizeType;
            ConfigureScreen();
        }

        private void ConfigureScreen()
        {
            BackgroundColor = Color.White;

            Icon image = new Icon()
            {
                FileName = ImagePath,
                Margin = new Thickness(20),
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = App.ScreenWidth * 0.25,
                HeightRequest = App.ScreenWidth * 0.25,
                Color = ColorPalette.Pink
            };

            Label titleLabel = new Label()
            {
                TextColor = Color.Black,
                FontAttributes = FontAttributes.Bold,
                Text = TitleLabelText.ToUpper(),
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 20,
            };

            Label descriptionLabel = new Label()
            {
                TextColor = ColorPalette.LightBlue,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = Description,
                FontSize = 17
            };



            VerticalOptions = LayoutOptions.Center;
            Margin = 10;


            Children.Add(image);
            Children.Add(titleLabel);
            Children.Add(descriptionLabel);
        }
    }
}
