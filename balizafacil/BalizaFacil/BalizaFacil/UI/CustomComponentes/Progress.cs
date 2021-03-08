using BalizaFacil.Models;
using System;
using System.Linq;
using Xamarin.Forms;

namespace BalizaFacil.UI
{
    public class Progress : Grid
    {
        public int TotalStepCount { get; set; }

        public Color DoneForeColor { get; set; }

        public Color CurrentForeColor { get; set; }

        public Color ToDoForeColor { get; set; }

        private int currentStep { get; set; }

        public int CurrentStep
        {
            get => currentStep;
            set
            {
                currentStep = value;
                for (int i = 0; i <= TotalStepCount; i++)
                {
                    if (Steps[i] is null)
                        return;

                    var status = this.CurrentStep == i ? ProgressStepStatus.Doing : this.CurrentStep > i ? ProgressStepStatus.Done : ProgressStepStatus.Todo;
                    Steps[i].Status = status;
                }

                var step = Steps.FirstOrDefault(stp => stp.Status == ProgressStepStatus.Doing);
                var width = step.UIElement.Bounds.X + step.UIElement.Bounds.Width / 2;
                BackgroundLine.WidthRequest = width;
            }
        }

        public ProgressStep[] Steps { get; set; }

        public Label BackgroundLine { get; set; }

        public Progress(int TotalStepCount, int CurrentStep, bool backgroundTrace = true)
        {
            try
            {
                this.Steps = new ProgressStep[TotalStepCount + 1];
                this.TotalStepCount = TotalStepCount;


                this.RowDefinitions.Add(new RowDefinition() { Height = 50 });

                BackgroundLine = new CustomLabel()
                {
                    BackgroundColor = ColorPalette.LightBlue,
                    HeightRequest = 5,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    WidthRequest = 0,
                    CustomPadding = 5
                };

                if (backgroundTrace)
                    this.Children.Add(BackgroundLine, 0, 0);

                Grid stepsGrid = new Grid()
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                stepsGrid.RowDefinitions.Add(new RowDefinition() { });
                for (int i = 0; i <= TotalStepCount; i++)
                {
                    var status = CurrentStep == i ? ProgressStepStatus.Doing : CurrentStep > i ? ProgressStepStatus.Done : ProgressStepStatus.Todo;
                    Steps[i] = new ProgressStep(i, status);

                    stepsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                    stepsGrid.Children.Add(Steps[i].UIElement, i, 0);
                }

                this.Children.Add(stepsGrid, 0, 0);

                this.CurrentStep = CurrentStep;

                this.LayoutChanged += OnLayoutChanged;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        private void OnLayoutChanged(object sender, EventArgs e)
        {
            try
            {
                if (BackgroundLine.WidthRequest < 1)
                {
                    var step = Steps.FirstOrDefault(stp => stp.Status == ProgressStepStatus.Doing);
                    var width = step.UIElement.Bounds.X + step.UIElement.Bounds.Width / 2;
                    BackgroundLine.WidthRequest = width;
                    this.LayoutChanged -= OnLayoutChanged;
                }
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        /// <summary>
        /// Advance one step
        /// </summary>
        /// <returns> If is completed</returns>
        public bool Advance()
        {
            try
            {
                Steps[CurrentStep].Status = ProgressStepStatus.Done;
                CurrentStep++;

                if (CurrentStep > TotalStepCount)
                    return true;
                else
                    Steps[CurrentStep].Status = ProgressStepStatus.Doing;

                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
