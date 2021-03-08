using System;
using System.Windows.Input;
using BalizaFacil.Models;
using Xamarin.Forms;

namespace BalizaFacil.UI
{
    public class GIF : Xamarin.Forms.Image
    {
        public static BindableProperty GifSourceProperty = BindableProperty.Create(nameof(GIFSource), typeof(string), typeof(GIF), propertyChanged: OnSourceChanged);

        private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as GIF).GIFSource = (string)newValue;
        }

        public string GIFSource
        {
            set
            {
                Source = ImageSource.FromFile($"_{value}.png");
            }
        }

        private ICommand onClick;
        public ICommand OnClick
        {
            get => onClick; set
            {
                onClick = value;

                TapGestureRecognizer tapGesture = new TapGestureRecognizer() {
                    Command = onClick,
                    CommandParameter = OnClickParameter
                };

                GestureRecognizers.Clear();
                GestureRecognizers.Add(tapGesture);
            }
        }
      
        public object OnClickParameter { get; internal set; }

        public GIF()
        {
            IsOpaque = true;
        }
    }
}
