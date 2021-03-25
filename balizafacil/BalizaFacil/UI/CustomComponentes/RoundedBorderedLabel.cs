using Xamarin.Forms;

namespace BalizaFacil.UI
{
    public class RoundedBorderedLabel : Frame
    {
        public Frame BorderFrame { get => this; }

        public Frame BackgroundFrame { get => this.Content as Frame;  }

        public Label InnerLabel { get; set; }

        public RoundedBorderedLabel(Label label, Color borderColor, int borderWidth, Color? labelBackgroundColor = null, Thickness? padding = null, int cornerRadius = 100)
        {
            try
            {
                InnerLabel = label;
                this.BackgroundColor = borderColor;
                this.Padding = new Thickness(0);
                this.Margin = new Thickness(0);
                this.CornerRadius = cornerRadius;
                this.VerticalOptions = LayoutOptions.CenterAndExpand;
                this.HorizontalOptions = LayoutOptions.Center;
                this.Content = new Frame
                {
                    BackgroundColor = labelBackgroundColor.HasValue ? labelBackgroundColor.Value : Color.White,
                    Margin = new Thickness(borderWidth),
                    Padding = padding.HasValue ? padding.Value : new Thickness(40, 3),
                    CornerRadius = this.CornerRadius,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                    Content = label
                };
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }
    }
}
