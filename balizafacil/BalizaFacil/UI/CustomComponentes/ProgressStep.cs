using BalizaFacil.Models;
using Xamarin.Forms;

namespace BalizaFacil.UI
{
    public class ProgressStep
    {
        public RoundedBorderedLabel UIElement { get; set; }


        private ProgressStepStatus status;
        public ProgressStepStatus Status
        {
            get => status;
            set
            {
                if (status == value)
                    return;

                status = value;

                switch (Status)
                {
                    case ProgressStepStatus.Todo:
                        BorderFrame.BackgroundColor = ColorPalette.Gray;
                        BackgroundFrame.BackgroundColor = Color.White;
                        Label.TextColor = ColorPalette.Gray;
                        Label.Text = (StepNumber + 1).ToString();
                        break;
                    case ProgressStepStatus.Doing:
                        BorderFrame.BackgroundColor = ColorPalette.LightBlue;
                        BackgroundFrame.BackgroundColor = ColorPalette.LightBlue;
                        Label.TextColor = Color.White;
                        Label.Text = (StepNumber + 1).ToString();
                        break;
                    case ProgressStepStatus.Done:
                        BorderFrame.BackgroundColor = ColorPalette.Gray;
                        BackgroundFrame.BackgroundColor = Color.White;
                        Label.TextColor = ColorPalette.Gray;
                        Label.Text = "✔";
                        break;
                }

            }
        }

        public int StepNumber { get; set; }

        public Label Label => UIElement.InnerLabel as Label;

        public Frame BorderFrame => UIElement.BorderFrame;

        public Frame BackgroundFrame => UIElement.BackgroundFrame;

        public ProgressStep(int StepNumber, ProgressStepStatus Status = ProgressStepStatus.Todo)
        {
            try
            {
                Label labelTodo = new Label()
                {
                    Text = (StepNumber + 1).ToString(),
                    FontSize = 13,
                    TextColor = ColorPalette.Gray,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold
                };

                UIElement = new RoundedBorderedLabel(labelTodo, labelTodo.TextColor, 2, Color.White, 3);

                int sideSize = 30;
                UIElement.BorderFrame.WidthRequest = sideSize;
                UIElement.BackgroundFrame.WidthRequest = sideSize;
                UIElement.BorderFrame.HeightRequest = sideSize;
                UIElement.BackgroundFrame.HeightRequest = sideSize;

                this.StepNumber = StepNumber;
                this.Status = Status;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }


    }
}
