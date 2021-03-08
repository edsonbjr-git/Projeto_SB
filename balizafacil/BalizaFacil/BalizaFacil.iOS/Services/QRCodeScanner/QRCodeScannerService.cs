using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BalizaFacil.iOS.Services.QRCodeScanner;
using BalizaFacil.Services;
using Xamarin.Forms;
using ZXing.Mobile;

[assembly: Dependency(typeof(QRCodeScannerService))]
namespace BalizaFacil.iOS.Services.QRCodeScanner
{
    class QRCodeScannerService : IQRCodeSannerService
    {
        private MobileBarcodeScanner scanner { get; set; }
        private MobileBarcodeScanningOptions options { get; set; }

        public QRCodeScannerService()
        {
            options = new MobileBarcodeScanningOptions();
            scanner = new MobileBarcodeScanner()
            {
                TopText = "Escaneie o QRCode na embalagem do seu sensor"
            };
        }

        public async Task<string> ScanAsync()
        {
            var result = await scanner.Scan(options);

            return result.Text;
        }
    }
}