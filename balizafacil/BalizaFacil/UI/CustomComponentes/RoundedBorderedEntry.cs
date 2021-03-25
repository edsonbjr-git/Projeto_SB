using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace BalizaFacil.UI
{
    public class RoundedBorderedEntry : Entry
    {
        public Thickness Padding { get; set; } = new Thickness(70, 30, 50, 30);


        public Color BorderColor { get; set; }

        public int BorderWidth { get; set; } = 4;

        private bool centeredPlaceholder;
        public bool CenteredPlaceholder
        {
            get => centeredPlaceholder; set
            {
                OnPropertyChanging();
                centeredPlaceholder = value;
                OnPropertyChanged();
            }
        }

        public RoundedBorderedEntry()
        {
            Padding = new Thickness(50, 30);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            try
            {
                base.OnPropertyChanged(propertyName);

                if (CenteredPlaceholder && (propertyName == nameof(this.Text) || propertyName == nameof(this.CenteredPlaceholder)))
                {
                    if (this.Text != null && this.Text.Length > 0)
                        this.HorizontalTextAlignment = TextAlignment.Start;
                    else
                        this.HorizontalTextAlignment = TextAlignment.Center;
                }
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }
    }
}
