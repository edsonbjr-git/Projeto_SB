using System;
using System.Linq;
using BalizaFacil.Core;
using BalizaFacil.Services;
using BalizaFacil.UI;
using BalizaFacil.Utils;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class MeasureSpaceView : Grid, IDisappearView
    {
        IStorageService Storage => ServicesManager.Instance.Storage;
        public MeasureSpaceViewModel ViewModel { get; set; }

        public MeasureSpaceView()
        {
            ViewModel = new MeasureSpaceViewModel();
            BindingContext = ViewModel;
            ConfigureScreen();
        }

        private void ConfigureScreen()
        {
            var navBar = BaseContentPage.Instance.ConfigureNavBar("", true, true);
            Progress progress = new Progress(1, 0, false);
            ViewModel.Progress = progress;
           
            Label instructionLabel = new Label
            { 
                FontSize = 17,
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.Start,
            };
            instructionLabel.SetBinding(Label.TextProperty, new Binding(nameof(ViewModel.Instruction), converter: new ValueConverter(value => value.ToString().ToUpper())));

            Label directionLabel = new Label
            {
                FontSize = 17,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.Black,
                FontAttributes = FontAttributes.Bold,
                Margin = 0
            };
            directionLabel.SetBinding(Label.TextProperty, new Binding(nameof(ViewModel.DirectionInstruction), converter: new ValueConverter(value => value.ToString().ToUpper())));
            RoundedBorderedLabel roundedLabel = new RoundedBorderedLabel(directionLabel, ColorPalette.Pink, 3, Color.White, new Thickness(30, 1))
            {
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(5, 5, 5, 0)
            };

            GIF InstructionGIF = new GIF()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = App.ScreenWidth,
                MinimumHeightRequest = 50,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            InstructionGIF.SetBinding(GIF.GifSourceProperty, nameof(ViewModel.InstructionGIFSource));

            Button measureSpaceButton = new Button
            {
                Text = "Medir vaga".ToUpper(),
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 17,
                BackgroundColor = ColorPalette.Pink,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(20, 0),
                //Padding = new Thickness(12),
                BorderRadius = 18,
                HeightRequest = 55,
                Command = ViewModel.MeasureSpaceCommand,
            };
            measureSpaceButton.SetBinding(IsVisibleProperty, new Binding(nameof(ViewModel.MeasuringSpace), converter: new ValueConverter((value) => !(bool)value)));

            Button endMeasurementButton = new Button()
            {
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 17,
                BackgroundColor = ColorPalette.Pink,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(20, 0),
                //Padding = new Thickness(12),
                BorderRadius = 18,
                HeightRequest = 55,
                Command = ViewModel.EndMeasurementCommand,
            };
            endMeasurementButton.SetBinding(IsVisibleProperty, new Binding(nameof(ViewModel.MeasuringSpace)));

            Button cancelButton = new Button()
            {
                BackgroundColor = ColorPalette.LightBlue,
                TextColor = Color.White,
                Text = "cancelar".ToUpper(),
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.End,
                Margin = new Thickness(20, 5),
                //Padding = new Thickness(12),
                BorderRadius = 18,
                HeightRequest = 55,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                Command = ViewModel.CancelMeasurementCommand
            };
            Button jumpButton = new Button()
            {
                BackgroundColor = ColorPalette.LightBlue,
                TextColor = Color.White,
                Text = "pular medição".ToUpper(),
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(20, 5),
                //Padding = new Thickness(12),
                BorderRadius = 18,
                HeightRequest = 55,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                Command = ViewModel.JumpMeasurementCommand
            };

            Button teste = new Button()
            {
                BackgroundColor = ColorPalette.LightBlue,
                FontSize = 0,
                HorizontalOptions = LayoutOptions.End,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                BorderRadius = 50
            };


            RowDefinitions.Add(new RowDefinition() { Height = 60 });
            RowDefinitions.Add(new RowDefinition() { Height = 50 });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = App.ScreenWidth });
            //Grid.ColumnDefinitions.Add()
            Children.Add(navBar, 0, 0);
            Children.Add(progress, 0, 1);
            Children.Add(instructionLabel, 0, 2);
            Children.Add(roundedLabel, 0, 3);
            if (Storage.TrainingMode)
                Children.Add(teste, 0, 3);
            Children.Add(InstructionGIF, 0, 4);
            Children.Add(measureSpaceButton, 0, 5);
            Children.Add(endMeasurementButton, 0, 5);
            Children.Add(cancelButton, 0, 6);
            Children.Add(jumpButton, 0,6);

            //if (App.IsAndroidSDKBelowMarshmallow)
            //{
            ViewModel.PropertyChanged += (sender, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        if (Storage.TrainingMode)
                        {
                            if (teste.IsVisible && Models.Sensor.StatusChecked)
                            {
                                teste.IsVisible = false;
                                teste.IsEnabled = false;
                            }
                            else if (!teste.IsVisible && Models.Sensor.StatusChecked)
                            {
                                teste.IsVisible = true;
                                teste.IsEnabled = true;
                            }
                        }
                        //sizeTypeInTime = DistanceManager.Instance.CalculateParkingSpaceSizeType(Math.Round(ViewModel.ParkingSpaceSize+0.1));
                        //ViewModel.CountMetters(Math.Round(ViewModel.ParkingSpaceSize));
                        //switch (sizeTypeInTime)
                        //{
                        //    case Models.ParkingSpaceSizeType.VeryEasy:
                        //    case Models.ParkingSpaceSizeType.Easy:
                        //    case Models.ParkingSpaceSizeType.Average:
                        //        endMeasurementButton.BackgroundColor = Color.Green;
                        //        break;
                        //    case Models.ParkingSpaceSizeType.Hard:
                        //        endMeasurementButton.BackgroundColor = Color.Orange;
                        //        break;
                        //    case Models.ParkingSpaceSizeType.VeryHard:
                        //        endMeasurementButton.BackgroundColor = Color.Gray;
                        //        break;
                        //    default:
                        //        endMeasurementButton.BackgroundColor = Color.Red;
                        //        break;
                        //}
                        measureSpaceButton.IsVisible = measureSpaceButton.IsVisible;
                        endMeasurementButton.IsVisible = endMeasurementButton.IsVisible;
                        if (e.PropertyName == nameof(ViewModel.DirectionInstruction))
                        {
                            directionLabel.Text = ViewModel.DirectionInstruction.ToUpper();
                        }
                        else if (e.PropertyName == nameof(ViewModel.InstructionGIFSource))
                        {
                            InstructionGIF.GIFSource = ViewModel.InstructionGIFSource;
                        }
                        else if (e.PropertyName == nameof(ViewModel.Instruction))
                        {
                            instructionLabel.Text = ViewModel.Instruction.ToUpper();
                        }
                        else if (e.PropertyName == nameof(ViewModel.MeasuringSpace))
                        {
                            endMeasurementButton.IsVisible = ViewModel.MeasuringSpace;
                            measureSpaceButton.IsVisible = !ViewModel.MeasuringSpace;
                        }
                        else if (e.PropertyName == nameof(ViewModel.ParkingSpaceSize))
                        {
                            endMeasurementButton.Text = $"Concluir medição ({Math.Round(ViewModel.ParkingSpaceSize, 0)} CM)";
                            //measureSpaceButton.Text = $"Medir Vaga \n({Math.Round(ViewModel.ParkingSpaceSize, 0)} CM)";
                        }
                    }
                    catch (System.Exception ex)
                    {
                        string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                        BalizaFacil.App.Instance.UnhandledException(title, ex);
                    }
                });
            };
            //}
        }

        public Models.ParkingSpaceSizeType sizeTypeInTime;
        public void OnDisappearing()
        {
            Services.ServicesManager.Instance.SoundPlayer.StopSound();
        }


    }
}