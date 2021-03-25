using System.Threading.Tasks;

namespace BalizaFacil.Services
{
    public interface IQRCodeSannerService
    {
        Task<string> ScanAsync();
    }
}
