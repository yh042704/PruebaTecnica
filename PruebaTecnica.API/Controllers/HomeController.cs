using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Core.Dtos;
using PruebaTecnica.Infrastructure.Utility;

namespace PruebaTecnica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpPost("EnviarContactos")]
        public async Task<ApiResponse> EnviarContactos([FromBody] dynamic datos)
        {
            string mailfrom = datos.Correo;
            string mailNamefrom = datos.Nombre;
            string body = datos.Message;

            await PTUtils.SendEmail(mailfrom, mailNamefrom, "Mensaje plataforma", body);

            return new ApiResponse(true, "Mensaje enviado satisfactoriamente!", "");
        }
    }
}
