using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    
    [Route("QR")]
    [ApiController]
    public class QRController : ControllerBase
    {
        [HttpGet]
        [Route("{QRString}")]
        public IActionResult QRValidation(string QRString)
        {
            ML.Result result = new ML.Result();
            if (QRString.Length == 29)
            {
                string idCandidato = QRString.Substring(0, 17);
                string fechaCita = QRString.Substring(17);

                result = BL.Candidato.GetById(idCandidato);
                if (result.Correct)
                {
                    if (FechaCitaCorrecta(fechaCita))
                    {
                        return Ok(result);
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "El tiempo del QR expiro";
                        return BadRequest(result);
                    }
                }
                else
                {
                    result.ErrorMessage = "Candidato no encontrado";
                    return BadRequest(result);
                }

            }
            else
            {
                result.Correct = false;
                result.ErrorMessage = "QR Invalido";
                return BadRequest(result);
            }
        }

        [NonAction]
        public bool FechaCitaCorrecta(string fechaCita)
        {
            DateTime fechaFormato;
            if (DateTime.TryParseExact(fechaCita, "ddMMyyyyHHmm",
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None,
                                       out fechaFormato))
            {
                DateTime fechaActual = DateTime.Now;
                fechaFormato = fechaFormato.AddSeconds(59);
                if (fechaFormato >= fechaActual)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
