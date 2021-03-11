using Xamarin.Forms;

namespace BalizaFacil.Services
{
    public class ServicesManager
    {
        private static ServicesManager instance;

        public static ServicesManager Instance
        {
            get
            {
                if (instance is null)
                    instance = new ServicesManager();

                return instance;
            }
        }

        public IStorageService Storage { get; set; }
        public IBluetoothService Bluetooth { get; set; }
        public IQRCodeSannerService QRCodeScanner { get; set; }
        public ISoundPlayerService SoundPlayer { get; set; }
        public IUtilsService Utils { get; set; }
        public ILocationService Location { get; set; }
        public IEmail Email { get; set; }

        private ServicesManager()
        {
            Storage = DependencyService.Get<IStorageService>();
            Bluetooth = DependencyService.Get<IBluetoothService>();
            QRCodeScanner = DependencyService.Get<IQRCodeSannerService>();
            SoundPlayer = DependencyService.Get<ISoundPlayerService>();
            Utils = DependencyService.Get<IUtilsService>();
            Location = DependencyService.Get<ILocationService>();
            Email = DependencyService.Get<IEmail>();
        }
    }
}
