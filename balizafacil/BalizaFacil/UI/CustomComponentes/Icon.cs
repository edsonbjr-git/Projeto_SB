using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.IO;
using Xamarin.Forms;

namespace BalizaFacil.UI
{
    public class Icon : SKCanvasView
    {
        public static readonly BindableProperty FileNameProperty = BindableProperty.Create(nameof(FileName), typeof(string), typeof(Icon), "", propertyChanged: Redraw);
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(Icon), Color.Black, propertyChanged: Redraw);

       

        public string FileName { get => (string)GetValue(FileNameProperty); set => SetValue(FileNameProperty, value); }
        public Color Color { get => (Color)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }
        public bool CentralizeHorizontally { get; set; } = true;
        public Icon()
        {
            PaintSurface += OnPaintSurface;
        }

        private static void Redraw(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Icon)?.InvalidateSurface();
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            try
            {

                SKCanvas canvas = e.Surface.Canvas;
                canvas.Clear();

                if (string.IsNullOrWhiteSpace(FileName))
                    return;

                var aa = GetType().Assembly.GetManifestResourceNames();

                using (Stream stream = GetType().Assembly.GetManifestResourceStream($"BalizaFacil.Resources.Icons.{FileName}"))
                using (SKPaint paint = new SKPaint() { ColorFilter = SKColorFilter.CreateBlendMode(Color.ToSKColor(), SKBlendMode.SrcIn) })
                {
                    SkiaSharp.Extended.Svg.SKSvg svg = new SkiaSharp.Extended.Svg.SKSvg();
                    svg.Load(stream);

                    SKImageInfo info = e.Info;
                    SKRect bounds = svg.ViewBox;

                    float xRatio = info.Width / bounds.Width;
                    float yRatio = info.Height / bounds.Height;

                    float ratio = Math.Min(xRatio, yRatio);

                    if (CentralizeHorizontally)
                    {
                        canvas.Translate(info.Width / 2, 0);
                        canvas.Translate(-1 * bounds.Width * ratio / 2, 0);
                    }

                    canvas.Translate(0, info.Height / 2);
                    canvas.Translate(0, -1 * bounds.Height * ratio / 2);

                    canvas.Scale(ratio);

                    canvas.DrawPicture(svg.Picture, paint);
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
