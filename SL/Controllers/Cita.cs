using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Cita : ControllerBase
    {
        [HttpGet]
        public IActionResult Index() { 
            return Ok();
        }
    }
}
