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

        public string VersaoAndroid { get; set; }

        public string ModeloCelular { get; set; }

        public TimeSpan Inicio { get; set; }

        public TimeSpan Fim { get; set; }

        public string Concluida { get; set; }

        public double TamanhoDaVaga { get; set; }

        public int ErroEtapa1 { get; set; }

        public int ErroEtapa2 { get; set; }

        public int ErroEtapa3 { get; set; }

        public int ErroEtapa4 { get; set; }

        public int ErroEtapa5 { get; set; }

        public int ErroEtapa6 { get; set; }

        public int VelFinalEtapa1 { get; set; }

        public int VelFinalEtapa2 { get; set; }

        public int VelFinalEtapa3 { get; set; }

        public int VelFinalEtapa4 { get; set; }

        public int VelFinalEtapa5 { get; set; }

        public int VelFinalEtapa6 { get; set; }

        public int VelMaxEtapa1 { get; set; }

        public int VelMaxEtapa2 { get; set; }

        public int VelMaxEtapa3 { get; set; }

        public int VelMaxEtapa4 { get; set; }

        public int VelMaxEtapa5 { get; set; }

        public int VelMaxEtapa6 { get; set; }

        public string Nota { get; set; }
    }
}
