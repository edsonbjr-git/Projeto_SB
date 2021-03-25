using Android.Content;
using BalizaFacil.Droid.Services;
using BalizaFacil.Models;
using BalizaFacil.Services;
using Java.IO;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Console = System.Console;

[assembly: Xamarin.Forms.Dependency(typeof(Email))]
namespace BalizaFacil.Droid.Services
{
    public class Email : IEmail
    {


        public void sendEmail(string file, string user, string car)
        {

            try
            {

                SmtpClient mailServer = new SmtpClient("smtp.gmail.com", 587);
                mailServer.EnableSsl = true;
                mailServer.UseDefaultCredentials = false;

                mailServer.Credentials = new System.Net.NetworkCredential("baliza.facil@gmail.com","@balizafacil2000");


                MailAddress from = new MailAddress("baliza.facil@gmail.com");
                //MailAddress to = new MailAddress("guilherme.pinto365@gmail.com");
                MailAddress to = new MailAddress("contato@balizafacil.com.br");
                MailMessage msg = new MailMessage(from,to);
                //msg.To.Add(to);
                msg.Subject = "Estatistica BalizaFacil";
                msg.Body = $"Segue anexo estatistica de balizas realizadas pelo usuario: {user}, Carro: {car}";

                Attachment data = new Attachment(file, MediaTypeNames.Application.Pdf);
                // Add time stamp information for the file.

                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(file);
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                // Add the file attachment to this email message.
                msg.Attachments.Add(data);
                try
                {
                    mailServer.Send(msg);
                }
                catch (Exception ex)
                {
                    string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    BalizaFacil.App.Instance.UnhandledException(title, ex);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to send email. Error : " + ex);
            }

        }

        public async Task SaveAndView(string fileName, String contentType, MemoryStream stream, string user, string car)
        {
            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));


            //Create directory and file 
            Java.IO.File myDir = new Java.IO.File(directory);
            myDir.Mkdir();

            Java.IO.File file = new Java.IO.File(myDir, fileName);

            //Remove if the file exists
            if (file.Exists()) file.Delete();

            //Write the stream into the file
            FileOutputStream outs = new FileOutputStream(file);
            outs.Write(stream.ToArray());

            outs.Flush();
            outs.Close();

            //Invoke the created file for viewing
            if (file.Exists())
            {
                Android.Net.Uri path = Android.Net.Uri.FromFile(file);
                string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
                string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
                Intent intent = new Intent(Intent.ActionView);
                intent.SetDataAndType(path, mimeType);
                Forms.Context.StartActivity(Intent.CreateChooser(intent, "Choose App"));
            }

            sendEmail(directory + "/" + fileName,user,car);
        }
    }
}
