using System;
using System.Collections.Generic;
using BalizaFacil.Core;
using BalizaFacil.Models;
using BalizaFacil.Services;
using BalizaFacil.UI;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class StepView : Grid, IAppearView, IDisappearView
    {
        private string DirectionMainInstruction
        {
            get
            {
                if (ApplicationStep.Back == ViewModel.StepAtual)
                {
                    return "Você passou do ponto".ToUpper();
                }
                else
                {
                    switch (ViewModel.Direction)
                    {
                        case SteeringWheel.CentrallyAligned:
                            return "Alinhe o volante".ToUpper();
                        case SteeringWheel.TotallyLeft:
                            return "Volante para a esquerda".ToUpper();
                        case SteeringWheel.TotallyRight:
                            return "Volante para a direita".ToUpper();
                    }
                }

                return "";
            }
        }

        private Color ColorMainInstruction
        {
            get
            {
                return ColorPalette.ExtraLightGray;

            }

        }


        IStorageService Storage => ServicesManager.Instance.Storage;

        #region Logical direction
    
        public string ImageBackWay
        {
            get
            {
                switch (ViewModel.Acceleration)
                {
                    case AccelerationDirection.Backward:
                        return "rightArrow1.svg";
                    case AccelerationDirection.Forward:
                        return "leftArrow1.svg";
                }
                return "";
            }
            
        }

        public string ImageBackWayAgain
        {
            get
            {
                switch (ViewModel.Acceleration)
                {
                    case AccelerationDirection.Backward:
                        return "leftArrow1.svg";
                    case AccelerationDirection.Forward:
                        return "rightArrow1.svg";
                }
                return "";
            }
        }
        public string DescritionBack
        {
            get
            {
                switch (ViewModel.Acceleration)
                {
                    case AccelerationDirection.Backward:
                        return "Voce passou da distancia, \nSiga em frente";
                    case AccelerationDirection.Forward:
                        return "Voce passou da distancia, \nSiga em ré";
                }
                return "";
            }
        }

        private string DirectionSecondInstruction
        {
            get
            {

                switch (ViewModel.Direction)
                {
                    case SteeringWheel.CentrallyAligned:
                        return "Alinhe as rodas".ToLower();
                    case SteeringWheel.TotallyLeft:
                        return "Totalmente para a esquerda".ToLower();
                    case SteeringWheel.TotallyRight:
                        return "Totalmente para a direita".ToLower();
                }

                return "";
            }
        }
        private string AccelerationInstruction
        {
            get
            {
                switch (ViewModel.Acceleration)
                {
                    case AccelerationDirection.Forward:
                        return "Siga em frente".ToUpper();
                    case AccelerationDirection.Backward:
                        return "Siga em ré".ToUpper();
                }

                return "";
            }
        }
       


        public string ConclusionText
        {
            get
            {
                switch (ViewModel.Step)
                {
                    case 0: return "começamos bem... bora continuar!";
                    case 1: return "força na peruca... vamos lá!";
                }

                if (ViewModel.Step == ViewModel.TotalSteps - 1)
                    return "tá acabando... bora finalizar!";


                bool isTotalEven = ViewModel.TotalSteps % 2 != 0;
                bool halfStepsDone = isTotalEven ? ViewModel.TotalSteps / 2 == ViewModel.Step : (ViewModel.TotalSteps + 1) / 2 == ViewModel.Step;

                if (halfStepsDone)
                    return "passamos da metade... vamos continuar!";

                return "está dando certo... ufa!";

            }
        }
        #endregion
        public StepViewModel ViewModel { get; set; }
        public Dictionary<StepModal, View> ModalContent { get; set; } = new Dictionary<StepModal, View>();
        public double Distance { get; set; }
        CurvedProgressBar Progress { get; set; }
        ApplicationStep currentStep { get; set; }
        public StepView(AccelerationDirection acceleration, SteeringWheel direction, ApplicationStep currentStep, double distance, int totalSteps, int step)
        {
            Distance = distance;
            this.currentStep = currentStep;
            this.ViewModel = new StepViewModel(acceleration, direction, currentStep, distance, totalSteps, step, this);
            BindingContext = ViewModel;

            ConfigureScreen();
            ConfigureModals();
        }

        private void ConfigureModals()
        {
            View ConclusionContent = new StackLayout()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                Margin = 20,
                Children =
                    {
                        new Icon()
                        {
                            FileName = "step_conclusion.svg",
                            WidthRequest = App.ScreenWidth / 5,
                            HeightRequest = App.ScreenWidth / 5,
                            HorizontalOptions = LayoutOptions.Center,
                            Color = ColorPalette.LightBlue,
                            Margin = 5
                        },
                        new Label()
                        {
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 21,
                            TextColor = Color.White,
                            Text = "Etapa concluída".ToUpper(),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = 3
                        },
                        new Label()
                        {
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 17,
                            TextColor = ColorPalette.LightBlue,
                            Text = ConclusionText.ToLower(),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = 3
                        }
                    }
            };

            View CurbTouchContent = new Frame()
            {
                BackgroundColor = Color.FromRgba(0, 0, 0, .60),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = App.ScreenWidth,
                Margin = 35,
                Padding = 45,
                CornerRadius = 40,
                Content = new StackLayout()
                {
                    Children =
                    {
                        new Label()
                        {
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 18,
                            TextColor = Color.White,
                            Text = "tocou na guia???".ToUpper(),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            //Margin = 2
                        },
                        new Icon()
                        {
                            FileName = "touch_icon1.svg",
                            WidthRequest = App.ScreenWidth / 2,
                            HeightRequest = App.ScreenWidth / 2,
                            HorizontalOptions = LayoutOptions.Center,
                            Color = ColorPalette.LightBlue,
                            //Margin = 2
                        }
                    }
                }
            };

            TapGestureRecognizer curbTouchGesture = new TapGestureRecognizer()
            {
                Command = ViewModel.CurbColision
            };
            CurbTouchContent.GestureRecognizers.Add(curbTouchGesture);

            View SensorDisconnectedContent = new Frame()
            {
                BackgroundColor = Color.FromRgba(0, 0, 0, .85),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = App.ScreenWidth,
                Margin = 35,
                Padding = 35,
                CornerRadius = 30,
                Content = new StackLayout()
                {
                    Children =
                    {
                        new Icon()
                        {
                            FileName = "sensor_disconnected.svg",
                            WidthRequest = App.ScreenWidth / 6,
                            HeightRequest = App.ScreenWidth / 6,
                            HorizontalOptions = LayoutOptions.Center,
                            Color = ColorPalette.LightBlue,
                            Margin = 3
                        },
                        new Label()
                        {
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 21,
                            TextColor = Color.White,
                            Text = "Sensor desconectado".ToUpper(),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = new Thickness(0,10, 0, 0)
                        },
                        new Label()
                        {
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 17,
                            TextColor = ColorPalette.LightBlue,
                            Text = "aguarde um minutinho".ToLower(),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = 2
                        }
                    }
                }
            };

            View WrongWayContent = new Frame()
            {
                BackgroundColor = Color.FromRgba(0, 0, 0, .85),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = App.ScreenWidth,
                Margin = 35,
                Padding = 35,
                CornerRadius = 30,
                Content = new StackLayout()
                {
                    Children =
                    {
                        new Icon()
                        {
                            FileName = "wrong_way.svg",
                            WidthRequest = App.ScreenWidth / 6,
                            HeightRequest = App.ScreenWidth / 6,
                            HorizontalOptions = LayoutOptions.Center,
                            Color = ColorPalette.LightBlue,
                            Margin = 3
                        },
                        new Label()
                        {
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 21,
                            TextColor = Color.White,
                            Text = "Sentido Errado".ToUpper(),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = new Thickness(0,10, 0, 0)
                        },
                        new Label()
                        {
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 17,
                            TextColor = ColorPalette.LightBlue,
                            Text = "Eiii, você está indo pro lado errado da vida",
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = 2
                        }
                    }
                }
            };
            
            View LeftWrongWay = new Frame()
            {
                BackgroundColor = Color.FromRgba(0, 0, 0, .75),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = App.ScreenWidth,
                Margin = 35,
                Padding = 35,
                CornerRadius = 30,
                Content = new StackLayout()
                {
                    Children =
                    {
                        new Icon()
                        {
                            FileName = ImageBackWay,
                            WidthRequest = App.ScreenWidth / 2,
                            HeightRequest = App.ScreenWidth / 2,
                            HorizontalOptions = LayoutOptions.Center,
                            Color = ColorPalette.LightBlue,
                            Margin = 4
                        },
                        new Label()
                        {
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 21,
                            TextColor = Color.White,
                            Text = DescritionBack.ToUpper(),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = 3
                        },
                        new Label()
                        {
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 17,
                            TextColor = Color.White,
                            Text = "Mantenha o volante",
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = 3
                        }
                    }
                }
            };

            View RightWrongWay = new Frame()
            {
                BackgroundColor = Color.FromRgba(0, 0, 0, .75),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = App.ScreenWidth,
                Margin = 35,
                Padding = 35,
                CornerRadius = 30,
                Content = new StackLayout()
                {
                    Children =
                    {
                        new Icon()
                        {
                            FileName = ImageBackWayAgain,
                            WidthRequest = App.ScreenWidth / 2,
                            HeightRequest = App.ScreenWidth / 2,
                            HorizontalOptions = LayoutOptions.Center,
                            Color = ColorPalette.LightBlue,
                            Margin = 5
                        },
                        new Label()
                        {
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 21,
                            TextColor = Color.White,
                            Text = "Voce passou da distancia,\n" + AccelerationInstruction.ToUpper(),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = 3
                        },
                        new Label()
                        {
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 17,
                            TextColor = Color.White,
                            Text = "Mantenha o volante",
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = 3
                        }
                    }
                }
            };


            TapGestureRecognizer backTouchGesture = new TapGestureRecognizer()
            {
                Command = ViewModel.NextStep
            };
            LeftWrongWay.GestureRecognizers.Add(backTouchGesture);

            TapGestureRecognizer exitManeuver = new TapGestureRecognizer()
            {
                Command = ViewModel.Cancel
            };
            WrongWayContent.GestureRecognizers.Add(exitManeuver);
            SensorDisconnectedContent.GestureRecognizers.Add(exitManeuver);


            View OkaySpot = new Frame()
            {
                BackgroundColor = Color.FromRgba(0, 0, 0, .75),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = App.ScreenWidth,
                Margin = 35,
                Padding = 45,
                CornerRadius = 30,
                Content = new StackLayout()
                {
                    Children =
                    {
                        new Icon()
                        {
                            FileName = "like.svg",
                            WidthRequest = App.ScreenWidth / 2,
                            HeightRequest = App.ScreenWidth / 2,
                            HorizontalOptions = LayoutOptions.Center,
                            Color = ColorPalette.LightGreen,
                            Margin = 5
                        },
                        new Label()
                        {
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 21,
                            TextColor = Color.White,
                            Text = "PODE PARAR".ToUpper(),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = 3
                        }
                    }
                }
            };
            View finalizedStep = new Frame()
            {
                BackgroundColor = Color.FromRgba(0, 0, 0, .75),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = App.ScreenWidth,
                Margin = 15,
                Padding = 25,
                CornerRadius = 30,
                Content = new StackLayout()
                {
                    Children =
                    {
                        new Icon()
                        {
                            FileName = "like.svg",
                            WidthRequest = App.ScreenWidth / 4,
                            HeightRequest = App.ScreenWidth / 4,
                            HorizontalOptions = LayoutOptions.Center,
                            Color = ColorPalette.LightGreen,
                            Margin = 5
                        },
                        new Label()
                        {
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 18,
                            TextColor = Color.White,
                            Text = "Acha que ficou centralizado?".ToUpper(),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = 3
                        }
                    }
                }
            };
            finalizedStep.GestureRecognizers.Add(backTouchGesture);

            ModalContent.Add(StepModal.Conclusion, ConclusionContent);
            ModalContent.Add(StepModal.CurbTouch, CurbTouchContent);
            ModalContent.Add(StepModal.SensorDisconnected, SensorDisconnectedContent);
            ModalContent.Add(StepModal.WrongWay, WrongWayContent);
            ModalContent.Add(StepModal.LeftWay, LeftWrongWay);
            ModalContent.Add(StepModal.RightWay, RightWrongWay);
            ModalContent.Add(StepModal.FinalizedStep, finalizedStep);
            ModalContent.Add(StepModal.OkaySpot, OkaySpot);

        }

        private void ConfigureScreen()
        {
            BackgroundColor = Color.White;

            Progress generalProgress = new Progress(ViewModel.TotalSteps, ViewModel.Step)
            {
                Margin = new Thickness(5, 5, 5, 0)
            };



            Label directionText = new Label()
            {
                TextColor = Color.Black,
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                Text = DirectionMainInstruction,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            RoundedBorderedLabel mainDirectionLabel = new RoundedBorderedLabel(directionText, Color.Transparent, 0, ColorMainInstruction, 7)
            {
                //BackgroundColor = ColorMainInstruction,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(30, 0),
                VerticalOptions = LayoutOptions.Start
            };
            mainDirectionLabel.Content.HorizontalOptions = mainDirectionLabel.HorizontalOptions;

            Label secondDirectionLabel = new Label()
            {
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 15,
                Text = DirectionSecondInstruction,
                TextColor = Color.Black,
            };
             

            UI.Animation directionGIF = new UI.Animation(ViewModel.Direction);


            Label accelerationText = new Label()
            {
                TextColor = Color.Black,
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                Text = AccelerationInstruction,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            RoundedBorderedLabel accelerationLabel = new RoundedBorderedLabel(accelerationText, Color.Transparent, 0, ColorPalette.ExtraLightGray, 7)
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = App.ScreenWidth,
                Margin = new Thickness(30, 0),
            };


            accelerationLabel.Content.HorizontalOptions = accelerationLabel.HorizontalOptions;

            UI.Animation accelerationGIF = new UI.Animation(ViewModel.Acceleration);

            int ciclo = 0;

            if (ViewModel.Direction != SteeringWheel.CentrallyAligned)
            {

                accelerationGIF.KeepAnimating = false;
                accelerationGIF.Completed += () =>
                {
                    ciclo++;

                    if (ciclo <= 1)
                        directionGIF.StartRedrawing();
                };

                directionGIF.Completed += () =>
                {
                    if (ciclo <= 1)
                        accelerationGIF.StartRedrawing();
                };
            }
            else
            {
                accelerationGIF.Completed += () =>
                {
                    ciclo++;

                    if (ciclo <= 1)
                        accelerationGIF.StartRedrawing();
                };
            }

            Progress = new CurvedProgressBar()
            {
                HeightRequest = App.ScreenHeight / 5 * 1.2,
                WidthRequest = App.ScreenWidth
            };

            Progress.SetBinding(CurvedProgressBar.ProgressProperty, new Binding(nameof(ViewModel.Progress)));
            Progress.SetBinding(CurvedProgressBar.CmsLeftProperty, new Binding(nameof(ViewModel.DistanceLeft)));
            
            RoundedButton closeButton = new RoundedButton()
            {
                BackgroundColor = ColorPalette.LightBlue,
                TextColor = Color.White,
                Text = "X",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 40,
                HeightRequest = 40,
                Padding = 0,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Margin = 10,
                Command = ViewModel.Cancel
            };

            Label ParametrosTest = new Label()
            {

                BackgroundColor = ColorPalette.ExtraLightGray,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Margin = 10,
            };

            RoundedButton nextRound = new RoundedButton()
            {
                BackgroundColor = ColorPalette.LightBlue,
                TextColor = Color.White,
                Text = "Proxima Etapa".ToUpper(),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(15, 10),
                Padding = new Thickness(30, 13),
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                Command = ViewModel.NextRound

            };

            ColumnDefinitions.Add(new ColumnDefinition() { Width = App.ScreenWidth });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(App.ScreenHeight / 5 * 1.2, GridUnitType.Absolute) });

            Children.Add(generalProgress, 0, 0);
            Children.Add(mainDirectionLabel, 0, 1);
            if (StepViewModel.TrainingMode)
            {
                Children.Add(ParametrosTest, 0, 2);
                Children.Add(nextRound, 0, 7);
            }
            else
            {
                
                Children.Add(secondDirectionLabel, 0, 2);
            }

            Children.Add(directionGIF, 0, 3);
            Children.Add(accelerationLabel, 0, 4);

            Children.Add(accelerationGIF, 0, 5);

            Children.Add(Progress, 0, 6);
            Children.Add(closeButton, 0, 6);



            if (App.IsAndroidSDKBelowMarshmallow)
            {
                ViewModel.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(ViewModel.DistanceLeft))
                    {
                        Progress.CmsLeft = (int)ViewModel.DistanceLeft;
                    }
                
                };
            }
            ViewModel.PropertyChanged += (sender, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                   
                    //accelerationText.Text = $"V = {Math.Round(Convert.ToDecimal(DistanceManager.Instance.CurrentSpeed), 2)} KM/H \n";
                    //ParametrosTest.Text = $"V = {Math.Round(Convert.ToDecimal(StepViewModel.velocidade), 2)} KM/H \n" +
                    //                      $"P = {Math.Round(StepViewModel.parou1, 0)}   CM";
                    //directionText.Text = $"V = {Math.Round(Convert.ToDecimal(DistanceManager.Instance.CurrentSpeed * 0 + StepViewModel.FilteredSpeed), 2)}";
                    directionText.HorizontalTextAlignment = TextAlignment.Center;

                    if ((FlowManager.CurrentStep1 == ApplicationStep.ManeuverIII ) && ViewModel.curbTouch)
                    {
                    cabeOuNaoCabe = DistanceManager.Instance.CorrectOnCurbColisionTeste(Direction.Left, ApplicationStep.ManeuverIII);
                        if (cabeOuNaoCabe == 1)
                        {
                            directionText.BackgroundColor = Color.Green;
                            mainDirectionLabel.BackgroundColor = Color.Green;

                        }
                        else if (cabeOuNaoCabe == 2)
                        {
                            directionText.BackgroundColor = Color.Gray;
                            mainDirectionLabel.BackgroundColor = Color.Gray;
                        }
                        else
                        {
                            directionText.BackgroundColor = Color.Red;
                            mainDirectionLabel.BackgroundColor = Color.Red;
                        }
                    }
                    else
                    {
                        directionText.BackgroundColor = Color.Transparent;
                        mainDirectionLabel.BackgroundColor = Color.White;
                    }
                });
            };
        }

        public static int cabeOuNaoCabe;
        void IAppearView.OnAppearing()
        {
            //ServicesManager.Instance.SoundPlayer.PlaySound(VoiceType);
            Progress.StartRedrawing();
        }

        void IDisappearView.OnDisappearing()
        {
            Progress.Redraw = false;
            ViewModel.StepEnded();
        }
    }
}