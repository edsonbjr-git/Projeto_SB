using BalizaFacil.Core;
using BalizaFacil.Models;
using System.Threading.Tasks;
using System.IO;

namespace BalizaFacil.Services
{
    public interface IEmail
    {
        void sendEmail(string msg,string user,string car);

        Task SaveAndView(string filename, string contentType, MemoryStream stream,string user,string car);
    }
}
