using BalizaFacil.iOS.Services;
using BalizaFacil.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(ScreenService))]
namespace BalizaFacil.iOS.Services
{
    public class ScreenService : IUtilsService
    {
        public void CloseApp()
        {
            throw new System.NotImplementedException();
        }

        public void HideStatusBar()
        {
            
        }

        public void SetLandscape()
        {
        }

        public void SetPortrait()
        {
        }

        public void ShowStatusBar()
        {
        }
    }
}