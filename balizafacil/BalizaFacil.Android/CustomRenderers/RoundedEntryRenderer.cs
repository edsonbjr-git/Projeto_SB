using Android.Content;
using Android.Graphics.Drawables;
using BalizaFacil.Droid.CustomRenderers;
using BalizaFacil.UI;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryRenderer))]
namespace BalizaFacil.Droid.CustomRenderers
{
    public class RoundedEntryRenderer : EntryRenderer
    {
        public RoundedEntryRenderer(Context context)
            : base(context)
        {

        }

        private GradientDrawable backgroundDrawable;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var entry = e.NewElement as RoundedEntry;

                backgroundDrawable = new GradientDrawable();
                backgroundDrawable.SetCornerRadius(100);
                backgroundDrawable.SetColor(entry.TextAreaColor.ToAndroid());
                Control.TextAlignment = Android.Views.TextAlignment.Center;

                int marginSide = (int)Math.Round(App.ScreenWidth / 10f, 0);
                int marginTopDown = (int)Math.Round(App.ScreenHeight / 32f);
                Control.SetPadding(marginSide, marginTopDown, marginSide, marginTopDown);
                Control.SetBackground(backgroundDrawable);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }


    }
}