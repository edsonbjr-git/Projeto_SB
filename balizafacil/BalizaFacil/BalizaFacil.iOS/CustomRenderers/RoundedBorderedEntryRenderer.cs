using BalizaFacil.UI;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RoundedBorderedEntry), typeof(RoundedBorderedEntryRenderer))]
namespace BalizaFacil.Droid.CustomRenderers
{
    public class RoundedBorderedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var entry = e.NewElement as RoundedBorderedEntry;
                Control.Layer.CornerRadius = 100;
                Control.Layer.BorderWidth = entry.BorderWidth;
                Control.Layer.BorderColor = entry.BorderColor.ToCGColor();
                Control.ClipsToBounds = true;
            }
        }
    }
}