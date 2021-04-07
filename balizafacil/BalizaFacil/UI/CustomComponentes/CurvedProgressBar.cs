using System;
using BalizaFacil.Core;
using BalizaFacil.Screens;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace BalizaFacil.UI
{
    public class CurvedProgressBar : SKCanvasView
    {

        #region Canvas Paints

        public SKPaint ExtraLightGrayStroke { get; set; }
        public SKPaint DarkBlueStroke { get; set; }
        public SKPaint DarkGreenStroke { get; set; }
        public SKPaint RedStroke { get; set; }
        public SKPaint WhiteStroke { get; set; }
        public SKPaint BlackTextFill { get; set; }
        public SKPaint PinkTextFill { get; set; }
        public SKPaint PinkStroke { get; set; }
        public SKPaint WhiteFill { get; set; }
        public SKPaint ExtraLightGrayFill { get; set; }
        public SKPaint ExtraRedFill { get; set; }
        public SKPaint ExtraGreenFill { get; set; }
        public SKPaint thickLinePaint { get; set; }
        #endregion

        #region Canvas Paths

        SKPath ProgressBackground { get; set; } = new SKPath();
        SKPath ProgressBackgroundRed { get; set; } = new SKPath();
        SKPath ProgressBackgroundGreen { get; set; } = new SKPath();

        SKPath ProgressTint { get; set; } = new SKPath();

        #endregion

        #region Rectangles
        public SKRect BaseRectangle { get; set; }
        public SKRect GrayCircleRectangle { get; set; }
        #endregion

        #region Progress Advance Angles

        public float ProgressPathStartAngle { get; set; } = 232;
        public float ProgressPathStartAngleRed { get; set; } = 293;
        public float ProgressPathStartAngleGreen { get; set; } = 280;
        public float ProgressPathFinalAngle { get; set; } = 76;
        public float ProgressCircleStartAngle { get; set; } = -38;


        #endregion

        #region Texts

        public string Faltam { get => "Faltam  ".ToUpper(); }
        public string XCms { get => CmsLeft >= 0 ? CmsLeft.ToString() : CmsLeft.ToString(); }
        public string CM { get => "   Cm".ToUpper(); }
        public float TextLeft { get; set; }
        public float TextTop { get; set; }

        #endregion

        public static bool isBackStep = false;

        public static BindableProperty ProgressProperty = BindableProperty.Create(nameof(Progress), typeof(float), typeof(CurvedProgressBar), 0f);

        public static BindableProperty CmsLeftProperty = BindableProperty.Create(nameof(CmsLeft), typeof(int), typeof(CurvedProgressBar), 0);

        /// <summary>
        /// 0 to 100 value. 
        /// </summary>
        public float Progress { get => (float)GetValue(ProgressProperty); set => SetValue(ProgressProperty, value); }
        public int CmsLeft { get => (int)GetValue(CmsLeftProperty); set => SetValue(CmsLeftProperty, value); }
        public bool Redraw { get; set; }

        public CurvedProgressBar()
        {
            try
            {
                porcentProgress = 125;
                isBackStep = true;
                PaintSurface += OnPaintSurface;
                InitializePaints();
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void StartRedrawing()
        {
            Redraw = true;
            Device.StartTimer(TimeSpan.FromMilliseconds(1000 / 60), () =>
            {
                InvalidateSurface();
                return Redraw;
            });
        }

        private void InitializePaints()
        {
            thickLinePaint = new SKPaint
            {
                Style = SKPaintStyle.StrokeAndFill,
                Color = SKColors.Black,
                StrokeWidth = 10
            };

            //Progresso faltando
            ExtraLightGrayStroke = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 20,
                StrokeCap = SKStrokeCap.Round,
                Color = ColorPalette.ExtraLightGray.ToSKColor(),
                IsAntialias = true
            };
            //Progresso ja feito
            DarkBlueStroke = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = App.WidthPixels / 36,
                StrokeCap = SKStrokeCap.Round,
                Color = ColorPalette.DarkBlue.ToSKColor(),
                IsAntialias = true
            };

            DarkGreenStroke = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = App.WidthPixels / 36,
                StrokeCap = SKStrokeCap.Round,
                Color = ColorPalette.DarkGreen.ToSKColor(),
                IsAntialias = true
            };

            RedStroke = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = App.WidthPixels / 36,
                StrokeCap = SKStrokeCap.Round,
                Color = ColorPalette.Red.ToSKColor(),
                IsAntialias = true
            };

            WhiteStroke = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = App.WidthPixels / 36,
                StrokeCap = SKStrokeCap.Round,
                Color = Color.White.ToSKColor(),
                IsAntialias = true
            };
            //bolinha do progresso
            PinkStroke = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = App.WidthPixels / 154.28f,
                StrokeCap = SKStrokeCap.Round,
                Color = ColorPalette.Pink.ToSKColor(),
                IsAntialias = true
            };

            BlackTextFill = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                StrokeWidth = App.WidthPixels / 216,
                Color = Color.Black.ToSKColor(),
                IsAntialias = true,
                FakeBoldText = true,
                TextSize = 50
            };

            PinkTextFill = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                StrokeWidth = App.WidthPixels / 360,
                Color = ColorPalette.Pink.ToSKColor(),
                IsAntialias = true,
                FakeBoldText = true,
                TextSize = 50
            };

            WhiteFill = new SKPaint()
            {
                Color = Color.White.ToSKColor(),
                StrokeCap = SKStrokeCap.Round,
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            ExtraLightGrayFill = new SKPaint()
            {
                Color = ColorPalette.ExtraLightGray.ToSKColor(),
                StrokeCap = SKStrokeCap.Round,
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            ExtraRedFill = new SKPaint()
            {
                Color = ColorPalette.Red.ToSKColor(),
                StrokeCap = SKStrokeCap.Round,
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            ExtraGreenFill = new SKPaint()
            {
                Color = ColorPalette.DarkGreen.ToSKColor(),
                StrokeCap = SKStrokeCap.Round,
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };


        }

        double Distance = StepViewModel.DistanciaTotal;
        public static int porcentProgress = 100;
        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            try
            {
                SKCanvas canvas = e.Surface.Canvas;
                
                float sideSize = e.Info.Width * 1.5f;
                float progressAngle = ProgressPathFinalAngle * (Progress / porcentProgress);
                float progressCircleRadius = App.WidthPixels / 43.2f;

                if (BaseRectangle.Width == 0)
                {
                    BaseRectangle = new SKRect()
                    {
                        Size = new SKSize(sideSize, sideSize)
                    };

                    float circleMargin = App.WidthPixels / 27;
                    GrayCircleRectangle = new SKRect()
                    {
                        Location = new SKPoint(circleMargin, circleMargin),
                        Size = new SKSize(sideSize - circleMargin * 2, sideSize - circleMargin * 2)
                    };
                    
                    ProgressBackground.AddArc(BaseRectangle, ProgressPathStartAngle, ProgressPathFinalAngle);

                    ProgressBackgroundRed.AddArc(BaseRectangle, ProgressPathStartAngleRed, 20);

                    //ta melhorando
                    //double Angle_20cm = StepViewModel.MarginHit * 76 / Distance;
                    double Angle_20cm = StepViewModel.MarginHit * 76 / StepViewModel.DistanciaTotal;
                    /* //modo treinamento
                     if (Angle_20cm > 30)
                         Angle_20cm = 30;*/
                    Angle_20cm = Angle_20cm / 1.25;
                    double target = 232 + (76 / 1.25); // angulo da barra preta (final da etapa)
                                                       //Este 1.25 é por causa q o progresso calcula foi multiplicado por 1.25
                                                       //Antes o 100% estava em 76 graus, agora ele irá para 76 / 1.25

                   // 07/03/2021
                   // if (FlowManager.CurrentStep1 == ApplicationStep.ManeuverI || FlowManager.CurrentStep1 == ApplicationStep.ManeuverII) 
                   // sergio, AjusteMov, Ajuste verde todas etapas no modo treinamento
                   
                    /*// modo treinamento
                    if (StepViewModel.TrainingMode || (FlowManager.CurrentStep1 == ApplicationStep.ManeuverI || FlowManager.CurrentStep1 == ApplicationStep.ManeuverII || FlowManager.CurrentStep1 == ApplicationStep.ManeuverIII || FlowManager.CurrentStep1 == ApplicationStep.ManeuverIV || FlowManager.CurrentStep1 == ApplicationStep.ManeuverV))
                        ProgressBackgroundGreen.AddArc(BaseRectangle, (float)(target-Angle_20cm), (float)(2*Angle_20cm));
                    else
                        ProgressBackgroundGreen.AddArc(BaseRectangle, (float)(target), (float)(Angle_20cm));*/

                    ProgressBackgroundGreen.AddArc(BaseRectangle, (float)(target - Angle_20cm), (float)(2 * Angle_20cm));
                    //onde começa , tamanho
                    //     286, 13


                    float widthFaltamText = BlackTextFill.MeasureText(Faltam);
                    float widthXText = BlackTextFill.MeasureText(XCms) * 1.5f;
                    float widthCMText = BlackTextFill.MeasureText(CM);

                    float totalTextWidth = widthFaltamText + widthXText + widthCMText;

                    float faltamTextProportion = (widthFaltamText * 100 / totalTextWidth) / 100;

                    var freeSpace = e.Info.Width * 0.65f;

                    TextLeft = freeSpace / 2;

                    TextTop = e.Info.Height / 2.5f;

                    BlackTextFill.TextSize = 0.35f * (e.Info.Width * faltamTextProportion) * BlackTextFill.TextSize / widthFaltamText;
                    PinkTextFill.TextSize = BlackTextFill.TextSize * 1.5f;
                }

                ProgressTint.Reset();

                ProgressTint.AddArc(BaseRectangle, ProgressPathStartAngle, Math.Min(progressAngle, ProgressPathFinalAngle));


                canvas.Clear();
                canvas.Save();

                canvas.Translate(e.Info.Width / 2 - sideSize / 2, App.ScreenWidth / 10.8f);


                canvas.DrawPath(ProgressBackground, ExtraLightGrayStroke);
               


                canvas.DrawPath(ProgressBackgroundRed, WhiteStroke);
                canvas.DrawPath(ProgressBackgroundGreen, DarkGreenStroke);
                if (Progress > 0)
                canvas.DrawPath(ProgressTint, DarkBlueStroke);
                var initial = (100 * (Distance - StepViewModel.Margin) / Distance);
                var final = (100 * (Distance + StepViewModel.Margin) / Distance);
                var XRight = PinkTextFill.MeasureText(XCms);
                if (Progress > 0)
                {
                   
                    if (Progress < initial)
                    {

                        canvas.DrawPath(ProgressBackgroundRed, WhiteStroke);
                        canvas.DrawPath(ProgressBackgroundGreen, DarkGreenStroke);
                        canvas.DrawOval(GrayCircleRectangle, ExtraLightGrayFill);
                        //canvas.DrawOval(GrayCircleRectangle, ExtraRedFill);

                    }
                    else if (Progress >= initial && Progress <= final)
                    {

                        canvas.DrawOval(GrayCircleRectangle, ExtraGreenFill);// ExtraLightGrayFill);
                        XRight = BlackTextFill.MeasureText(XCms);
                    }

                    else if (Progress >= final) {
                        //canvas.DrawPath(ProgressTint, RedStroke);
                        canvas.DrawOval(GrayCircleRectangle, ExtraLightGrayFill);
                        XRight = BlackTextFill.MeasureText(XCms);
                    }
                    if (Progress > final)
                    {
                        canvas.DrawPath(ProgressBackgroundRed, RedStroke);
                        canvas.DrawPath(ProgressBackgroundGreen, DarkGreenStroke);
                    }
                    
                }



                var usedProgressAngle = Progress > 0 ? Progress > porcentProgress ? ProgressPathFinalAngle - 1 : progressAngle : 0;
                canvas.RotateDegrees(ProgressCircleStartAngle + usedProgressAngle, BaseRectangle.Width / 2f, BaseRectangle.Height / 2f);
                if (Progress > -1 && Progress < 125)
                {
                    canvas.DrawCircle(BaseRectangle.Width / 2f, 0, progressCircleRadius, WhiteFill);
                    canvas.DrawCircle(BaseRectangle.Width / 2f, 0, progressCircleRadius, PinkStroke);
                }

                canvas.Restore();

                //canvas.Translate(0, App.ScreenWidth / 30f);
                var FaltamRight = BlackTextFill.MeasureText(Faltam);

                //1280x720
                canvas.DrawText(Faltam, TextLeft, TextTop, BlackTextFill);
                //if (isBackStep)
                //    canvas.DrawLine(560, 83, 650, -150, thickLinePaint);
                //var XRight = PinkTextFill.MeasureText(XCms);
                canvas.DrawText(XCms, TextLeft + FaltamRight, TextTop, PinkTextFill);

                canvas.DrawText(CM, TextLeft + FaltamRight + XRight, TextTop, BlackTextFill);
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }

        }


    }
}
