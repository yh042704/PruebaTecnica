using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Application.Interfaces.Repository;
using PruebaTecnica.Core.Dtos;

namespace PruebaTecnica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionesController(ITransacciones itransacciones) : ControllerBase
    {
        private readonly ITransacciones _itransacciones = itransacciones;
        [HttpPost("ProcesarCompra")]
        public async Task<ApiResponse> ProcesarCompra([FromBody] dynamic datos)
        {
            var dataResult = await _itransacciones.ProcesarCompra(datos);

            return new ApiResponse(
                success: true,
                message: "",
                result: dataResult
            );
        }

        [HttpPost("ProcesarPago")]
        public async Task<ApiResponse> ProcesarPago([FromBody] dynamic datos)
        {
            var dataResult = await _itransacciones.ProcesarPago(datos);

            return new ApiResponse(
                success: true,
                message: "",
                result: dataResult
            );
        }


        [HttpGet("GetTarjetasCredito")]
        [Authorize]
        public async Task<dynamic?> GetTarjetasCredito([FromQuery]int userAuth)
        {
            var dataResult = await _itransacciones.GetTarjetasCredito(userAuth);

            return new ApiResponse(
                success: true,
                message: "",
                result: dataResult!
            );
        }

        [HttpGet("GetTransacciones")]
        [Authorize]
        public async Task<dynamic?> GetTransacciones([FromQuery] int idTarjeta, [FromQuery] int Periodo, [FromQuery] int Mes)
        {
            var dataResult = await _itransacciones.GetHistorialTransacciones(idTarjeta, Periodo, Mes);

            return new ApiResponse(
                success: true,
                message: "",
                result: dataResult!
            );
        }

        [HttpGet("GetEstadoCuenta")]
        [Authorize]
        public async Task<dynamic?> GetEstadoCuenta([FromQuery] int idTarjeta, [FromQuery] int Periodo, [FromQuery] int Mes)
        {
            var dataResult = await _itransacciones.GetEstadoCuenta(idTarjeta, Periodo, Mes);

            return new ApiResponse(
                success: true,
                message: "",
                result: dataResult!
            );
        }

    }
}
