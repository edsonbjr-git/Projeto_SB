using SkiaSharp.Views.Forms;
using BalizaFacil.Models;
using SkiaSharp;
using System;
using Xamarin.Forms;
using System.IO;
using System.Collections.Generic;

namespace BalizaFacil.UI
{
    public class Animation : SKCanvasView
    {
        public event Action Completed;
        public string Image { get; set; }
        private AnimationType type;

        public AnimationType Type
        {
            get { return type; }
            set
            {
                type = value;

                switch (type)
                {
                    case AnimationType.CarBackward:
                        Image = "backward.png";
                        break;
                    case AnimationType.CarForward:
                        Image = "forward.png";
                        break;
                    case AnimationType.SteeringWheelLeft:
                        Image = "totallyleft.png";
                        break;
                    case AnimationType.SteeringWheelRight:
                        Image = "totallyright.png";
                        break;
                    case AnimationType.SteeringWheelCentered:
                        Image = "centrallyaligned.png";
                        break;
                }
            }
        }

        public bool SteeringWheelStopped { get; private set; }
        public DateTime? AnimationStopped { get; set; }

        private TimeSpan timeAnimating;
        public TimeSpan TimeAnimating
        {
            get => timeAnimating; set
            {
                timeAnimating = value;
                CalculateDegreesPerFrame();
            }
        }

        private void CalculateDegreesPerFrame()
        {
            if (TimeAnimating == null || TimeAnimating == TimeSpan.Zero || TargetInitialDifference == 0)
                return;

            float totalSeconds = (float)TimeAnimating.TotalSeconds;
            float anglesPerSecond = TargetInitialDifference / totalSeconds;
            float anglesperFrame = anglesPerSecond / FramesPerSecond;

            DegreesPerFrame = anglesperFrame;
        }

        public TimeSpan TimeStopped { get; set; }
        public volatile bool KeepAnimating;

        public float FramesPerSecond { get; set; }

        #region Steering Wheel Animation
        public float CurrentDegree { get; set; }
        public float TargetAngle { get; set; }
        public float BaseTargetAngle { get; set; }

        private float targetInitialDifference;

        public float TargetInitialDifference
        {
            get => targetInitialDifference;
            set
            {
                targetInitialDifference = value;
                CalculateDegreesPerFrame();
            }
        }

        public float DegreesPerFrame { get; set; }
        #endregion

        #region Car Animation
        public float? CurrentYPosition { get; set; }
        #endregion

        public Animation(AnimationType type)
        {
            if (Bitmaps == null)
                Animation.Bitmaps = new Dictionary<AnimationType, SKBitmap>();

            Type = type;
            PaintSurface += OnPaintSurface;
            FramesPerSecond = 30;
            KeepAnimating = true; ;
            TimeAnimating = TimeSpan.FromSeconds(1f);
            TimeStopped = TimeSpan.FromSeconds(2f);
            CurrentYPosition = null;

            if (Type == AnimationType.SteeringWheelLeft || Type == AnimationType.SteeringWheelRight || Type == AnimationType.Wainting)
            {
                TargetInitialDifference = 360;
                CurrentDegree = 0;
                TargetAngle = Type == AnimationType.SteeringWheelLeft ? -360 : 360;
                BaseTargetAngle = TargetAngle;
            }

            if (!Bitmaps.ContainsKey(Type))
            {
                using (Stream stream = GetType().Assembly.GetManifestResourceStream($"BalizaFacil.Resources.Images.{Image}"))
                using (MemoryStream memStream = new MemoryStream())
                {
                    stream.CopyToAsync(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);
                    Bitmaps[Type] = SKBitmap.Decode(memStream);
                }
            }


            StartRedrawing();
        }

        public Animation(AccelerationDirection acceleration)
            : this(acceleration == AccelerationDirection.Backward ? AnimationType.CarBackward : AnimationType.CarForward)
        {
        }

        public Animation(SteeringWheel direction)
            : this(direction == SteeringWheel.TotallyLeft ? AnimationType.SteeringWheelLeft : direction == SteeringWheel.TotallyRight ? AnimationType.SteeringWheelRight : AnimationType.SteeringWheelCentered)
        {

        }

        public static Dictionary<AnimationType, SKBitmap> Bitmaps { get; set; }
        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            try
            {
                if (!KeepAnimating)
                    return;

                SKCanvas canvas = e.Surface.Canvas;
                canvas.Clear();

                if (string.IsNullOrWhiteSpace(Image))
                    return;

                SKBitmap bitmap = Bitmaps[Type];

                if (Type == AnimationType.SteeringWheelCentered)
                {
                    canvas.Translate(e.Info.Width / 2, e.Info.Height / 2);

                    float height = e.Info.Height - 10;
                    float width = height;

                    float x = -width / 2;
                    float y = -height / 2;

                    canvas.DrawBitmap(bitmap, new SKRect(x, y, x + width, y + height));
                }
                else if (Type == AnimationType.SteeringWheelLeft || Type == AnimationType.SteeringWheelRight)
                {


                    canvas.Translate(e.Info.Width / 2, e.Info.Height / 2);
                    canvas.RotateDegrees(CurrentDegree);

                    float height = e.Info.Height - 10;
                    float width = height;

                    float x = -width / 2;
                    float y = -height / 2;

                    canvas.DrawBitmap(bitmap, new SKRect(x, y, x + width, y + height));

                    CurrentDegree = type == AnimationType.SteeringWheelLeft ? CurrentDegree - DegreesPerFrame : CurrentDegree + DegreesPerFrame;

                    if ((Type == AnimationType.SteeringWheelRight && CurrentDegree >= TargetAngle + 45) || (Type == AnimationType.SteeringWheelLeft && CurrentDegree <= TargetAngle - 45))
                    {
                        TargetAngle += BaseTargetAngle;
                        KeepAnimating = false;
                        //Device.StartTimer(TimeStopped, () =>
                        //{
                        //    StartRedrawing();
                        //    return false;
                        //});
                        Completed?.Invoke();
                    }
                }
                else if (Type == AnimationType.CarBackward || Type == AnimationType.CarForward)
                {
                    if (!CurrentYPosition.HasValue)
                        CurrentYPosition = Type == AnimationType.CarBackward ? - e.Info.Height : e.Info.Height;

                    canvas.Translate(0, CurrentYPosition.Value);

                    float height = e.Info.Height;
                    float factor = (float)bitmap.Height / (float)e.Info.Height;
                    float width = (float)bitmap.Width / factor;

                    float x = (e.Info.Width - width) / 2;
                    float y = (e.Info.Height - height) / 2;


                    canvas.DrawBitmap(bitmap, new SKRect(x, y, x + width, y + height));

                    if (Type == AnimationType.CarBackward)
                    {
                        CurrentYPosition += height / 25;

                        if (CurrentYPosition >= 0)
                        {
                            CurrentYPosition = -e.Info.Height;
                            KeepAnimating = false;
                            Completed?.Invoke();
                        }


                    }
                    else if (Type == AnimationType.CarForward)
                    {
                        CurrentYPosition -= height / 25;

                        if (CurrentYPosition <= 0)
                        {
                            CurrentYPosition = e.Info.Height;
                            KeepAnimating = false;
                            Completed?.Invoke();

                        }

                    }

                }

            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void StartRedrawing()
        {
            KeepAnimating = true;
            Device.StartTimer(TimeSpan.FromSeconds(1 / FramesPerSecond), () =>
            {
                InvalidateSurface();
                return KeepAnimating;
            });
        }
    }


}
