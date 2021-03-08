using BalizaFacil.Models;
using BalizaFacil.UI;
using SlideOverKit;
using System;
using System.Linq;
using Xamarin.Forms;
using BalizaFacil.Utils;
using BalizaFacil.Services;
using BalizaFacil.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using BalizaFacil.Screens.Configuration;

namespace BalizaFacil.Screens
{
    public class BaseContentPage : MenuContainerPage
    {
        public event Action BackButtonPressed;

        private static BaseContentPage instance;
        public static BaseContentPage Instance
        {
            get
            {
                if (instance is null)
                    instance = new BaseContentPage();

                return instance;
            }
        }

        #region Screen Content

        public List<View> Pages { get; set; }
        public List<View> Modals { get; set; }

        private View content;
        public new View Content
        {
            get
            {
                return content;
            }
            set
            {
                try
                {
                    Grid @base = base.Content as Grid;

                    if (content != null && @base.Children.Any(child => child == content))
                        @base.Children.Remove(content);

                    content = value;
                    @base.Children.Add(content);
                }
                catch (System.Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }
            }
        }

        public StackLayout PopupContainer { get; set; }

        #endregion

        public BaseContentPage()
        {
            try
            {
                NavigationPage.SetHasNavigationBar(this, false);
                BackgroundColor = Color.White;
                Pages = new List<View>();
                Modals = new List<View>();

                Grid content = new Grid();
                content.RowDefinitions.Add(new RowDefinition() { });
                content.ColumnDefinitions.Add(new ColumnDefinition());

                base.Content = content;
                PopupContainer = new StackLayout();

                ConfigureMenu();
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        void ConfigureMenu()
        {
            try
            {
                var closeIcon = new Icon
                {
                    FileName = "close.svg",
                    Color = ColorPalette.LightBlue,
                    HorizontalOptions = LayoutOptions.End,
                    Margin = 20,
                    WidthRequest = 30,
                    HeightRequest = 30
                };
                closeIcon.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(CloseMenu)
                });

                var menuContent = new StackLayout()
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    HeightRequest = App.ScreenHeight,
                    Children =
                        {
                            closeIcon,
                            new StackLayout()
                            {
                                Margin = new Thickness(0,App.ScreenHeight / 8 ,0,0),
                                VerticalOptions = LayoutOptions.Center,
                                Children =
                                {
                                    new Label()
                                    {
                                        Text = Services.ServicesManager.Instance.Storage.Username.ToUpper(),
                                        TextColor = Color.White,
                                        FontAttributes = FontAttributes.Bold,
                                        FontSize = 18,
                                        HorizontalTextAlignment = TextAlignment.Center,
                                        Margin = new Thickness(0,0,0, 10)
                                    },
                                    new Label()
                                    {
                                        Margin = new Thickness(30, 0, 0,20),
                                        HeightRequest = 3,
                                        BackgroundColor  = ColorPalette.LightBlue
                                    },
                                   GetItemList("register.svg", "Recadastro do veículo", RegisterCar,true),
                                   GetItemList("config.svg", "Configurações", Configurations,true),
                                   GetItemList("video.svg", "Vídeo de instrução", ShowVideo,true),
                                   GetItemList("sensor.svg", "Sensor", ConfigureSensor,true),
                                   GetItemList("help.svg", "Help", Help,true),
                                   GetItemList("register.svg", "Desenvolvedor", Dev,false),
                                }
                            }
                        }
                };

                Image background = new Image()
                {
                    Source = ImageSource.FromResource("BalizaFacil.Resources.Images.arrows.png", GetType().Assembly),
                    BackgroundColor = Color.White,
                    Aspect = Aspect.AspectFill
                };

                Grid slideMenuContent = new Grid();
                slideMenuContent.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(App.ScreenWidth * 0.75) });
                slideMenuContent.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(App.ScreenHeight) });

                slideMenuContent.Children.Add(background, 0, 0);
                slideMenuContent.Children.Add(menuContent, 0, 0);

                SlideMenu = new ModalView()
                {
                    MenuOrientations = MenuOrientation.RightToLeft,
                    WidthRequest = App.ScreenWidth * 0.75,
                    HeightRequest = App.ScreenHeight,
                    BackgroundViewColor = Color.FromRgba(0, 0, 0, 0),
                    Content = slideMenuContent
                };
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }
        IStorageService Storage => ServicesManager.Instance.Storage;
        protected override bool OnBackButtonPressed()
        {
            try
            {
                BackButtonPressed?.Invoke();
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
            return true;
        }

        #region NavBar

        public CustomNavBar ConfigureNavBar(string title = "", bool showBackButton = false, bool showMenuButton = false)
        {
            var navBar = new CustomNavBar(showBackButton, title, showMenuButton);
            navBar.BackButtonClicked += () => { PopView();
                FlowManager.CurrentStep1 -= 1; 
            };
            navBar.MenuButtonClicked += OnMenuButtonClicked;

            return navBar;
        }

        private void OnMenuButtonClicked()
        {
            try
            {
                ShowMenu();
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        private StackLayout GetItemList(string icon, string text, Action onClicked,bool visible)
        {
            Color cor = Color.White;
            Color corBack = Color.LightBlue;
            if (!visible)
            {
                cor = Color.Transparent;
                corBack = Color.Transparent;
            }
            StackLayout result = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(25, 0, 0, 10),
                Children =
                {

                    new Icon()
                    {
                        FileName = icon,
                        WidthRequest = 25,
                        HeightRequest = 25,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center,
                        Margin = new Thickness(0,0, 5, 0),
                        Color = corBack
                    },
                    new Label()
                    {
                        Text = text.ToUpper(),
                        TextColor = cor,                                
                        FontAttributes = FontAttributes.Bold,
                        VerticalTextAlignment = TextAlignment.Center,
                        FontSize = 15
                    }
                }
            };

            result.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(onClicked)
            });

            return result;
        }


        #endregion

        #region Menu Buttons Callbacks

        private void CloseMenu(object obj)
        {
            try
            {
                HideMenu();
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        private void ConfigureSensor()
        {
            try
            {
                HideMenu();
                PushView(new ConfigureSensorAddressView());
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        private void Help()
        {
            HideMenu();
            PushView(new WelcomeView());
        }
        
        private void Dev()
        {
            try
            {
                HideMenu();
                PushView(new ReportsView());
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }



        

        private string StringModeTraing = "Modo de treinamento";
        private void ShowVideo()
        {
            try
            {
                HideMenu();
                Device.OpenUri(new Uri("https://www.youtube.com/watch?v=dhaYUrXcDjw"));
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        private void Configurations()
        {
            try
            {
                HideMenu();
                //CarsRequestJSON.Instance.updateCar();
                PushView(new ConfigurationView(true));
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        private void RegisterCar()
        {
            try
            {
                HideMenu();
                //CarsRequestJSON.Instance.updateCar();
                PushView(new CarBrandView(true));
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        #endregion

        #region Navigation 

        internal void PushView(View view)
        {
            try
            {
                Pages.Add(view);

                if (Modals.Count <= 0)
                {
                    Content = view;
                    OnContentChanged();
                }
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        internal void PopView()
        {
            try
            {
                MeasureSpaceViewModel.measuring = false;
                if (Pages.LastOrDefault() is IDisappearView view)
                    view.OnDisappearing();

                if (Pages.Count > 0)
                    Pages.RemoveAt(Pages.Count - 1);

                if (Modals.Count <= 0 && Pages.Count > 0)
                {
                    Content = Pages.Last();
                    OnContentChanged();
                }
                else
                {
                    FlowManager.Instance.CloseApp();
                }
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        internal void PushModal(View view)
        {
            try
            {
                Modals.Add(view);
                Content = view;

                OnContentChanged();
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        internal void PopModal()
        {
            try
            {
                if (Pages.LastOrDefault() is IDisappearView view)
                    view.OnDisappearing();

                if (Modals.Count > 0)
                    Modals.RemoveAt(Modals.Count - 1);

                if (Modals.Count > 0)
                {
                    Content = Modals.Last();
                }
                else if (Pages.Count > 0)
                {
                    Content = Pages.Last();
                }
                else
                {
                    FlowManager.Instance.CloseApp();
                    return;
                }

                OnContentChanged();
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void OnContentChanged()
        {
            try
            {
                HidePopup();
                BackgroundColor = Color.White;
                BackgroundImage = "";

                string contentType = Content.GetType().Name;

                switch (contentType)
                {
                    case nameof(RegisterView):
                    case nameof(WelcomeView):
                        BackgroundColor = ColorPalette.DarkBlue;
                        BackgroundImage = "background_register.png";
                        break;
                    case nameof(WaitingSensorConnectionView):
                        BackgroundColor = Color.Black;
                        break;
                    default:
                        Content.BackgroundColor = Color.White;
                        break;
                }

                if (Content is IAppearView view)
                    view.OnAppearing();

                IsVisible = false;
                IsVisible = true;
                Device.BeginInvokeOnMainThread(() => { IsVisible = true; });

            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public void ShowPopup(View popup)
        {
            Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        HidePopup();

                        if (Content.GetType().Name != nameof(StepView))
                            return;
                        
                        PopupContainer.Children.Add(popup);

                        Grid @base = base.Content as Grid;
                        @base.Children.Add(PopupContainer);
                        @base.RaiseChild(PopupContainer);

                        PopupContainer.IsVisible = true;
                    }
                    catch (System.Exception ex)
                    {
                        string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                        BalizaFacil.App.Instance.UnhandledException(title, ex);
                    }
                });
        }

        public new void HidePopup()
        {
            try
            {
                Grid @base = base.Content as Grid;
                PopupContainer.Children.Clear();
                @base.LowerChild(PopupContainer);
                @base.Children.Remove(PopupContainer);

                if (Content.GetType().Name != nameof(StepView))
                    PopupContainer.BackgroundColor = Color.White;
                PopupContainer.IsVisible = false;
                Device.BeginInvokeOnMainThread(() => { IsVisible = true; });
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }
        #endregion
    }
}
