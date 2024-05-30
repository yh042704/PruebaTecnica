using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Application.Interfaces.Repository;
using PruebaTecnica.Core.Dtos;

namespace PruebaTecnica.Api.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class LoginController(IAuthRepository authRepository) : ControllerBase
    {
		private readonly IAuthRepository _authRepository = authRepository;

        [HttpPost("Login")]
        public async Task<dynamic> Login([FromBody] dynamic _userData)
		{
			await _authRepository.ValidUser(_userData);
			string token = _userData.TokenJWT;
            string tipoUser = string.Concat(_userData.Result, ",", _userData.usuarioID, ",", _userData.nombre);

            return new ApiResponse(
               success: true,
               message: tipoUser,
               result: token
           );
        }
	}
}
