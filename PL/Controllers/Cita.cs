using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.CopyAnalysis;


namespace PL.Controllers
{
    public class Cita : Controller
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;

        public Cita(Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        [HttpGet]
        public IActionResult Form()
        {
            ML.Cita cita = new ML.Cita();
            cita.Candidato = new ML.Candidato();
            cita.Status = new ML.Status();
            cita.Status.IdStatus = 1;
            cita.Reclutador = new ML.Reclutador();
            cita.Candidato.Vacante = new ML.Vacante();
            cita.Candidato.Vacante.Vacantes = BL.Vacante.GetAll().Objects;
            cita.Reclutador.Reclutadores = BL.Reclutador.GetAll().Objects;
            return View(cita);
        }
        [HttpPost]
        public IActionResult Form(ML.Cita cita)
        {
            cita.Candidato.IdCandidato = GenerarIDUsuarioConFechaHora(cita.Candidato).ToUpper();
            ML.Result resultCandidato = BL.Candidato.Add(cita.Candidato);
            if (resultCandidato.Correct)
            {

                ML.Result result = BL.Cita.Add(cita);
                if (result.Correct)
                {
                    byte[] QR = QRGenerator.QR.GenerateQr("http://localhost:5184/cita/validar?qR=" + cita.Candidato.IdCandidato + cita.Fecha.ToString("ddMMyyyyHHmm"));
                    if (QR != null)
                    {
                        Send(cita, QR);
                    }
                    return View("Modal");
                }
                else
                {
                    return View("Modal");
                }
            }
            else
            {
                return View("Modal");
            }

        }
        [HttpGet]
        public IActionResult Validar(string? qR)
        {
            ML.Candidato candidato = new ML.Candidato();
            if (qR != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7164/cita/validar/");

                    var postTask = client.GetAsync(qR);
                    postTask.Wait();
                        
                    var resultAlumno = postTask.Result;
                    if (resultAlumno.IsSuccessStatusCode)
                    {
                        var readTask = resultAlumno.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        candidato = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Candidato>(readTask.Result.Object.ToString());
                        ViewBag.Status = 200;
                        return View(candidato);
                    }
                    else
                    {
                        var readTask = resultAlumno.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        if (readTask.Result.ErrorMessage == "El tiempo del QR expiro")
                        {
                            candidato = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Candidato>(readTask.Result.Object.ToString());
                            ViewBag.Status = 201;
                            return View(candidato);
                        }
                        else
                        {
                            ViewBag.Status = 400;
                            return View();
                        }
                    }
                }
            }
            else
            {
                ViewBag.Status = 404;
                return View();
            }
        }
        public void Send(ML.Cita cita, byte[] QR)
        {
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = contentPath + "/wwwroot/Correo/CorreoDigis.html";

            string body = string.Empty;

            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }

            //Remplazando lo de html
            body = body.Replace("{día}", cita.Fecha.ToString());
            body = body.Replace("{NombreRH}", cita.Reclutador.Nombre + ' ' + cita.Reclutador.ApellidoPaterno + ' ' + cita.Reclutador.ApellidoMaterno);

            ContentType c = new ContentType("image/jpeg");

            System.Net.Mail.LinkedResource linkedResource2 = new System.Net.Mail.LinkedResource(new MemoryStream(QR));
            linkedResource2.ContentType = c;
            linkedResource2.ContentId = "qr";
            linkedResource2.TransferEncoding = TransferEncoding.Base64;

            System.Net.Mail.AlternateView alternativeView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
            alternativeView.LinkedResources.Add(linkedResource2);

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("digis01sistemas@gmail.com", "xrbiwsbgqqcmnace"),
                EnableSsl = true,
                UseDefaultCredentials = false
            };

            var mensaje = new System.Net.Mail.MailMessage
            {
                From = new System.Net.Mail.MailAddress("digis01sistemas@gmail.com"),
                Subject = "Codigo acceso al edificio",
                Body = body,
                IsBodyHtml = true,
            };

            mensaje.Attachments.Clear();

            mensaje.To.Add("chrisroyhp1990@gmail.com");
            mensaje.AlternateViews.Add(alternativeView);

            smtpClient.Send(mensaje);
        }

        public string GenerarIDUsuarioConFechaHora(ML.Candidato candidato)
        {
            return candidato.Nombre[0].ToString() + candidato.ApellidoPaterno[0].ToString() + candidato.ApellidoMaterno[0].ToString() + DateTime.Now.ToString("ddMMyyyyHHmmss");
        }
    }
}
