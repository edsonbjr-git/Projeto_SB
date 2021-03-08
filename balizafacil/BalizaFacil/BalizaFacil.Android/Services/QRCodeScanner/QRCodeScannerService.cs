using System.Threading.Tasks;
using BalizaFacil.Droid.Services;
using BalizaFacil.Services;
using Xamarin.Forms;
using ZXing.Mobile;

[assembly: Dependency(typeof(QRCodeScannerService))]
namespace BalizaFacil.Droid.Services
{

    public class QRCodeScannerService : IQRCodeSannerService
    {
        private MobileBarcodeScanner Scanner { get; set; }
        private MobileBarcodeScanningOptions Options { get; set; }

        public QRCodeScannerService()
        {
            try
            {
                Options = new MobileBarcodeScanningOptions();
                Scanner = new MobileBarcodeScanner()
                {
                    TopText = "Escaneie o QRCode na embalagem do seu sensor"
                };
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }
        }

        public async Task<string> ScanAsync()
        {
            try
            {
                var result = await Scanner.Scan(Options);

                return result.Text;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }

            return "";
        }
    }
}