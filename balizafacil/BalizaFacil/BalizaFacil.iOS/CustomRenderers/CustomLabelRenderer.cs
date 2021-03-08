using BalizaFacil.iOS.CustomRenderers;
using BalizaFacil.UI;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRenderer))]
namespace BalizaFacil.iOS.CustomRenderers
{
    public class CustomLabelRenderer : LabelRenderer
    {

        public CustomLabelRenderer() { }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var label = e.NewElement as CustomLabel;
                SetNativeControl(new TagUiLabel(label.CustomPadding));
            }
        }


        public class TagUiLabel : UILabel
        {
            
            private UIEdgeInsets EdgeInsets { get; set; }
            public Thickness Padding { get; }

            public TagUiLabel(Thickness padding)
            {
                Padding = padding;
            }


            public TagUiLabel()
            {
                EdgeInsets = new UIEdgeInsets((float)Padding.Top, (float)Padding.Left, (float)Padding.Bottom, (float)Padding.Right);
            }
            public override void DrawText(CoreGraphics.CGRect rect)
            {
                base.DrawText(EdgeInsets.InsetRect(rect));
            }
        }

    }
}