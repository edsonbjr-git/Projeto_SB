using BalizaFacil.Utils;
using Xamarin.Forms;

namespace BalizaFacil.UI
{
    public class CarSelectionList : ListView
    {
        public CarSelectionList()
        {
            Margin = new Thickness(0, -6, 0, 0);
            SeparatorColor = ColorPalette.LightGray;
            ItemTemplate = new DataTemplate(() =>
            {
                Label label = new Label()
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    VerticalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(20, 0, 0, 0),
                    BackgroundColor = Color.White,
                    FontSize = 16,
                    TextColor = ColorPalette.Gray,
                    FontAttributes = FontAttributes.Bold
                };
                label.SetBinding(Label.TextProperty, new Binding("Name", converter: new ValueConverter(name => name.ToString().ToUpper())));

                return new CustomViewCell(label)
                {
                    View = label,
                    SelectedTextColor = ColorPalette.Pink,
                    NormalTextColor = label.TextColor
                };
            });
        }
    }
}
