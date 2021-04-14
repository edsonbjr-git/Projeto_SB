using BalizaFacil.Core;
using BalizaFacil.Models;
using BalizaFacil.Screens;
using BalizaFacil.Services;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BalizaFacil.Screens
{
    [Serializable()]
    class ReportsViewModel : INotifyPropertyChanged
    {
        public List<Reports> reports = new List<Reports>();

        IEmail email;
        private static ReportsViewModel instance;
        public static ReportsViewModel Instance
        {
            get
            {
                if (instance is null)
                {
                    instance = new ReportsViewModel();
                }
                
                return instance;
            }
        }
        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;
        [field: NonSerialized()]
        public ICommand Continue { get; internal set; }
        [field: NonSerialized()]
        public ReportsViewModel()
        {
            Continue = new Command(() =>
            {
                email = ServicesManager.Instance.Email;
                createPDF();
               
                BaseContentPage.Instance.PopModal();

            });
        }
        IStorageService Storage => ServicesManager.Instance.Storage;

        public string ModeloCelular { get; internal set; }
        public object MACSensor { get; internal set; }
        public string VersaoAndroid { get; internal set; }

        public void createPDF()
        {
            PdfDocument document = new PdfDocument();

            //Add a page to the document
            PdfPage page = document.Pages.Add();

            //Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;

            //Set the standard font
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

            //Draw the text
            graphics.DrawString("Estatistica Baliza Facil \n\n" + GetStatistics(), font, PdfBrushes.Black, new PointF(0, 0));
            MemoryStream stream = new MemoryStream();
            document.Save(stream);

            //Close the document
            document.Close(true);

            //Save the stream as a file in the device and invoke it for viewing
            email.SaveAndView("Estatisticas.pdf", "application/pdf", stream,Storage.Username,Storage.Car.Name +" "+ Storage.Car.Style);

        }

        [field: NonSerialized()]
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        string FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "balizas.bin");
        

        public bool GetHistorical()
        {
            if (File.Exists(FileName))
            {
                Stream TestFileStream = File.OpenRead(FileName);
                BinaryFormatter deserializer = new BinaryFormatter();
                reports = (List<Reports>)deserializer.Deserialize(TestFileStream);
                //Debug.Print(historicals.ToString());
                TestFileStream.Close();
                return true;
            }
            return false;

        }

        public void SaveHistorical()
        {
            Stream TestFileStream = File.Create(FileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(TestFileStream, reports);
            TestFileStream.Close();
        }
        public string GetStatistics()
        {
            string result = "";
            foreach (Reports his in reports)
            {
                result += his.ToString() + "\n";
            }
            return result;
        }
        public override string ToString()
        {
            string result = "";
            foreach(Reports his in reports)
            {
                result += his.ToString() + "\n";
            }
            return result;
        }
    }
}
