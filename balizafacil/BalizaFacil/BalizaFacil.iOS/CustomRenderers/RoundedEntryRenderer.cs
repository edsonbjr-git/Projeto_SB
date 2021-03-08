using BalizaFacil.UI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryRenderer))]
namespace BalizaFacil.Droid.CustomRenderers
{
    public class RoundedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);


            if (Control != null)
            {
                var entry = e.NewElement as RoundedBorderedEntry;
                Control.Layer.CornerRadius = 100;
            }
        }
    }
}