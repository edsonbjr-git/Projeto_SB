using SlideOverKit;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    public class ModalView : SlideMenuView
    {
        public ModalView()
        {
            this.IsFullScreen = true;
            this.MenuOrientations = MenuOrientation.BottomToTop;
            WidthRequest = App.ScreenWidth;
            HeightRequest = App.ScreenHeight;
            AnimationDurationMillisecond = 0;
            this.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
            this.BackgroundViewColor= Color.FromRgba(0, 0, 0, 0.0);
        }

    }
}
