using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace BalizaFacil.UI
{
    public class RoundedEntry : Entry
    {
        public bool CenteredPlaceholder { get; set; } = false;
        public Color TextAreaColor { get; set; }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            try
            {
                base.OnPropertyChanged(propertyName);

                if (CenteredPlaceholder && propertyName == nameof(this.Text))
                {
                    if (this.Text.Length > 0)
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
