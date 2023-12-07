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
        private readonly IConfiguration _configuration;

        public Cita(Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment, IConfiguration configuration)
        {
            Environment = _environment;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Form()
        {
            string connectionString = _configuration.GetConnectionString("Dev");
            BL.Vacante obj = new BL.Vacante(connectionString);
            BL.Reclutador objreclutador = new BL.Reclutador(connectionString);
            ML.Cita cita = new ML.Cita();
            cita.Candidato = new ML.Candidato();
            cita.Status = new ML.Status();
            cita.Status.IdStatus = 1;
            cita.Reclutador = new ML.Reclutador();
            cita.Candidato.Vacante = new ML.Vacante();
            cita.Candidato.Vacante.Vacantes = obj.GetAll().Objects;
            cita.Reclutador.Reclutadores = objreclutador.GetAll().Objects;
            return View(cita);
        }
        [HttpPost]
        public IActionResult Form(ML.Cita cita)
        {
            cita.Candidato.IdCandidato = GenerarIDUsuarioConFechaHora(cita.Candidato).ToUpper();
            ML.Result resultCandidato = BL.Candidato.Add(cita.Candidato);
            if (resultCandidato.Correct)
            {
                string connectionString = _configuration.GetConnectionString("Dev");
                BL.Cita citaobj = new BL.Cita(connectionString);
                ML.Result result = citaobj.Add(cita);
                if (result.Correct)
                {
                    byte[] QR = QRGenerator.QR.GenerateQr(_configuration["EndPointsCita:UrlQr"] + cita.Candidato.IdCandidato + cita.Fecha.ToString("ddMMyyyyHHmm"));
                    if (QR != null)
                    {
                        Send(cita, QR);
                    }
                    ViewBag.Mensaje = "Se agendo correctamente la cita.";
                    return View("Modal");
                }
                else
                {
                    ViewBag.Mensaje = "Ocurrio un error al agendar la cita.";
                    return View("Modal");
                }
            }
            else
            {
                ViewBag.Mensaje = "Ocurrio un error al agendar la cita.";
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
                    client.BaseAddress = new Uri(_configuration["EndPointsCita:UrlValidar"]);

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

            string path = contentPath + _configuration["EmailCredentials:TemplateRoute"];

            string body = string.Empty;

            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{Candidato}", cita.Candidato.Nombre + ' ' + cita.Candidato.ApellidoPaterno);
            body = body.Replace("{día}", cita.Fecha.ToString());
            body = body.Replace("{correoRH}", cita.Reclutador.Correo);
            body = body.Replace("{NombreRH}", cita.Reclutador.Nombre + ' ' + cita.Reclutador.ApellidoPaterno + ' ' + cita.Reclutador.ApellidoMaterno);

            ContentType c = new ContentType("image/jpeg");

            System.Net.Mail.LinkedResource linkedResource2 = new System.Net.Mail.LinkedResource(new MemoryStream(QR));
            linkedResource2.ContentType = c;
            linkedResource2.ContentId = "qr";
            linkedResource2.TransferEncoding = TransferEncoding.Base64;

            System.Net.Mail.AlternateView alternativeView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
            alternativeView.LinkedResources.Add(linkedResource2);

            var smtpClient = new SmtpClient(_configuration["SMTPConfig:SMTPClient"])
            {
                Port = int.Parse(_configuration["SMTPConfig:Port"]),
                Credentials = new NetworkCredential(_configuration["EmailCredentials:Correo"], _configuration["EmailCredentials:Password"]),
                EnableSsl = bool.Parse(_configuration["SMTPConfig:EnableSsl"]),
                UseDefaultCredentials = bool.Parse(_configuration["SMTPConfig:UseDefaultCredentials"])
            };

            var mensaje = new System.Net.Mail.MailMessage
            {
                From = new System.Net.Mail.MailAddress(_configuration["EmailCredentials:Correo"]),
                Subject = _configuration["EmailCredentials:Subject"],
                Body = body,
                IsBodyHtml = true,
            };

            mensaje.Attachments.Clear();

            //Para enviar un correo a multiples personas realizarlo asi ("correo1@gmail.com, correo2@gmail.com")
            mensaje.To.Add(cita.Candidato.Correo + ", " + _configuration["EmailCredentials:CC"]);
            mensaje.AlternateViews.Add(alternativeView);

            smtpClient.Send(mensaje);
        }

        public string GenerarIDUsuarioConFechaHora(ML.Candidato candidato)
        {
            return candidato.Nombre[0].ToString() + candidato.ApellidoPaterno[0].ToString() + candidato.ApellidoMaterno[0].ToString() + DateTime.Now.ToString("ddMMyyyyHHmmss");
        }
    }
}
