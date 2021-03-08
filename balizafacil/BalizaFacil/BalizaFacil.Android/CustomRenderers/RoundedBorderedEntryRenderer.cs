using Android.Content;
using Android.Graphics.Drawables;
using BalizaFacil.Droid.CustomRenderers;
using BalizaFacil.UI;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundedBorderedEntry), typeof(RoundedBorderedEntryRenderer))]
namespace BalizaFacil.Droid.CustomRenderers
{
    public class RoundedBorderedEntryRenderer : EntryRenderer
    {
        public RoundedBorderedEntryRenderer(Context context)
            : base(context)
        {

        }

        private GradientDrawable backgroundDrawable;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var entry = e.NewElement as RoundedBorderedEntry;

                backgroundDrawable = new GradientDrawable();
                backgroundDrawable.SetCornerRadius(100);
                backgroundDrawable.SetStroke(entry.BorderWidth, entry.BorderColor.ToAndroid());

                Control.SetPadding((int)entry.Padding.Left, (int)entry.Padding.Top, (int)entry.Padding.Right, (int)entry.Padding.Bottom);
                Control.SetBackground(backgroundDrawable);

            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }


    }
}