using System;
namespace BalizaFacil.Models
{
    public class LogBaliza
    {
        public LogBaliza()
        {
        }

        public string Id { get; set; }

        public double Latitude { get; set; }

        public double Lontitude { get; set; }

        public double Altitude { get; set; }

        public DateTime Data { get; set; }
    }
}
