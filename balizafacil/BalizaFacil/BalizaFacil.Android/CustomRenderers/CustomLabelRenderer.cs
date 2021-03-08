using Android.Content;
using BalizaFacil.Droid.CustomRenderers;
using BalizaFacil.UI;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRenderer))]
namespace BalizaFacil.Droid.CustomRenderers
{
    public class CustomLabelRenderer : LabelRenderer
    {
        public CustomLabelRenderer(Context context)
            : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var label = e.NewElement as CustomLabel;
                Control.SetPadding((int)label.CustomPadding.Left, (int)label.CustomPadding.Top, (int)label.CustomPadding.Right, (int)label.CustomPadding.Bottom);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }


    }
}