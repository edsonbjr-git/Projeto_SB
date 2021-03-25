using Xamarin.Forms;

namespace BalizaFacil.UI
{
    public class CustomViewCell : ViewCell
    {
        public static BindableProperty SelectedProperty = BindableProperty.Create(nameof(Selected), typeof(bool), typeof(CustomViewCell), false);

        public bool Selected { get => (bool)GetValue(SelectedProperty); set => SetValue(SelectedProperty, (bool)value); }

        public Label label { get; set; } 

        public CustomViewCell(Label label)
        {
            this.label = label;
        }
        public Color SelectedTextColor { get; set; }
        public Color NormalTextColor { get; set; }
    }
}