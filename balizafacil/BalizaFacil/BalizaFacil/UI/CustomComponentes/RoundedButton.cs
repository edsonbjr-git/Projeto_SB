using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace BalizaFacil.UI
{
    public class RoundedButton : RoundedBorderedLabel
    {
        public event EventHandler Cliked;

        public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(RoundedButton), "");

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                SetValue(TextProperty, value);
                InnerLabel.Text = value;
            }
        }
        public Color TextColor { get => InnerLabel.TextColor; set => InnerLabel.TextColor = value; }
        public FontAttributes FontAttributes { get => InnerLabel.FontAttributes; set => InnerLabel.FontAttributes = value; }
        public double FontSize { get => InnerLabel.FontSize; set => InnerLabel.FontSize = value; }

        public ICommand Command { get; set; }

        public object CommandParameter { get; set; }

        public RoundedButton()
            : base(new Label(), Color.Transparent, 0, Color.Transparent, 0)
        {
            try
            {
                TapGestureRecognizer tapGesture = new TapGestureRecognizer();
                tapGesture.Command = new Command(() =>
                {
                    if (Command != null)
                        Command.Execute(CommandParameter);

                    this.Cliked?.Invoke(this, EventArgs.Empty);
                });

                this.GestureRecognizers.Add(tapGesture);
                
                this.FontSize = 18;
                this.Padding = new Thickness(10);
                this.HorizontalOptions = LayoutOptions.FillAndExpand;
                this.InnerLabel.Margin = 0;
                this.InnerLabel.VerticalTextAlignment = TextAlignment.Center;
                this.InnerLabel.VerticalOptions = LayoutOptions.Center;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }
    }
}
