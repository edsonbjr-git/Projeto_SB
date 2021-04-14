using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Xamarin.Forms.Internals;

namespace BalizaFacil.Models
{
    [Preserve(AllMembers = true)]
    [Serializable()]
    public class Reports
    {

        // Jhonatan, todas essas variaveis devem ir para o firebase
        // Inicio - variaveis para firebase

        // Parametros associados a baliza como um todo
        public string completed { get; set; } = "";
        public DateTime timeStart { get; set; }
        public DateTime timeEnd { get; set; }
        public string parkingTime { get; set; } = "";
        // Parametros associados a baliza como um todo


        // Parametros gravados para cada uma das 6 etapas
        public double[] diffDistance;
        public double[] maxSpeed;

        public double[] ElapsedTimeStep { get; set; } = new double[6];
        public double[] StepEndSpeed { get; set; } = new double[6];
        public double[] StepInitialSpeed { get; set; } = new double[6];
        public double[] CurbTouch { get; set; } = new double[6];
        public string[] dummie_str1 { get; set; } = new string[6];
        public string[] dummie_str2 { get; set; } = new string[6];
        public string[] dummie_str3 { get; set; } = new string[6];
        public double[] dummie_double1 { get; set; } = new double[6];
        public double[] dummie_double2 { get; set; } = new double[6];
        public double[] dummie_double3 { get; set; } = new double[6];
        public DateTime[] dummie_time1 { get; set; } = new DateTime[6];
        // Parametros gravados para cada uma das 6 etapas

        // Fim - variaveis para firebase


        private static Reports instance;
        public static Reports Instance
        {
            get
            {
                if (instance is null)
                    instance = new Reports();

                return instance;
            }
        }

        public double Altitude { get; set; } = -1;
        public double Latitude { get; set; } = -1;
        public double Lontitude { get; set; } = -1;
        public string ModeloCelular { get; set; } = "N/A";
        public string VersaoAndroid { get;  set; } = "N/A";
        public string MACSensor { get;  set; } = "N/A";

        public Reports()
        {
            timeStart = DateTime.Now;
            Console.WriteLine($"timeStart {timeStart}"); // sergio, AjusteMov
            diffDistance = new double[6];
            maxSpeed = new double[6];
        }
        public void Reset()
        {

            try
            {
                instance = null;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }

        }

        public override string ToString()
        {
            TimeSpan timeDiff = timeEnd.Subtract(timeStart);
            int diffTime = (int)timeDiff.TotalSeconds;
            string result = "";

            for (int c = 0; c < 6; c++)
            {
                result += "Step " + c + " - Diff: " + diffDistance[c] + "\tVel. Maxima: " + Math.Round(Convert.ToDecimal(maxSpeed[c]), 2) + "\n";
            }
            return completed + "Dia: " + parkingTime + "\nTempo de baliza: " + timeDiff.Hours + ":" + timeDiff.Minutes + ":" + timeDiff.Seconds + "\n" + result + "\n";
        }
    }
}
