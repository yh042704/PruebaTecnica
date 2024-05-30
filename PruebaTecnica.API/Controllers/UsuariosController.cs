using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Application.Interfaces.Repository;
using PruebaTecnica.Core.Dtos;
using PruebaTecnica.SQL.Queries;
using System.Net.Http;

namespace PruebaTecnica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController(IUsuariosRepository usuariosRepository) : ControllerBase
    {
        private readonly IUsuariosRepository _usuariosRepository = usuariosRepository;

        [HttpPut("RegistrarUsuario")]
        public async Task<dynamic?> RegistrarUsuario([FromBody] dynamic datos)
        {
            var dataResult = await _usuariosRepository.RegistrarUsuario(datos);

            return new ApiResponse(
                success: true,
                message: "Usuario registrado correctamente",
                result: dataResult
            );
        }
    }
}
