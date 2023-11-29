using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace PL.Controllers
{
    public class QRSend : Controller
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;

        public QRSend(Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        public IActionResult Send()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Send(string email, byte[] QR)
        {
            #region Pruebas quitar despues 
            email = "chrisroyhp1990@gmail.com";
            byte[] data = QRGenerator.QR.GenerateQr(email);

            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = contentPath + "/wwwroot/Correo/CorreoRisosu.html";
            #endregion

            string body = string.Empty;

            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }

            ContentType c = new ContentType("image/jpeg");

            //System.Net.Mail.LinkedResource linkedResource1 = new System.Net.Mail.LinkedResource(new MemoryStream(data));
            //linkedResource1.ContentType = c;
            //linkedResource1.ContentId = "reclutador";
            //linkedResource1.TransferEncoding = TransferEncoding.Base64;

            System.Net.Mail.LinkedResource linkedResource2 = new System.Net.Mail.LinkedResource(new MemoryStream(data));
            linkedResource2.ContentType = c;
            linkedResource2.ContentId = "qr";
            linkedResource2.TransferEncoding = TransferEncoding.Base64;

            System.Net.Mail.AlternateView alternativeView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
            //alternativeView.LinkedResources.Add(linkedResource1);
            alternativeView.LinkedResources.Add(linkedResource2);

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("chrisrhernandezp@gmail.com", "cegoveqaoawgyxfa"),
                EnableSsl = true,
                UseDefaultCredentials = false
            };

            var mensaje = new System.Net.Mail.MailMessage
            {
                From = new System.Net.Mail.MailAddress("chrisrhernandezp@gmail.com"),
                Subject = "Codigo acceso al edificio",
                Body = body,
                IsBodyHtml = true,
            };

            mensaje.Attachments.Clear();

            mensaje.To.Add("chrisroyhp1990@gmail.com");
            mensaje.AlternateViews.Add(alternativeView);

            smtpClient.Send(mensaje);

            return PartialView();
        }
    }
}
