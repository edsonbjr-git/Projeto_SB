using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace BalizaFacil.Models
{
    [Serializable()]
    public class Reports
    {
        public string completed = "";
        public DateTime timeStart;
        public DateTime timeEnd { get; set; }
        public string parkingTime = "";
        public double[] diffDistance;
        public double[] maxSpeed;
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
           
            for(int c = 0 ; c < 6 ; c++)
            {
                result += "Step " + c + " - Diff: " + diffDistance[c] + "\tVel. Maxima: " + Math.Round(Convert.ToDecimal(maxSpeed[c]), 2) + "\n";
            }
            return completed + "Dia: " + parkingTime +"\nTempo de baliza: " + timeDiff.Hours +":" + timeDiff.Minutes + ":"+timeDiff.Seconds + "\n"+ result + "\n";
        }
    }
}
