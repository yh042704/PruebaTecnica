using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PruebaTecnica.Controllers.Base;
using System.Security.Claims;

namespace PruebaTecnica.Controllers
{
    public class TransaccionesController(IHttpClientFactory httpClient) : BaseController(httpClient)
    {
        [HttpPost]
        [Authorize(AuthenticationSchemes = "PruebaTecnicaAuthScheme")]
        public async Task<dynamic?> ProcesarCompra([FromForm] int idTarjeta, [FromForm] string Descripcion, [FromForm] string Fecha, [FromForm] string Monto)
        {
            var procesoCompra = new
            {
                idTarjeta,
                Descripcion,
                Fecha,
                Monto
            };

            using var httpResponseMessage = await PostAsync("/api/Transacciones/ProcesarCompra", procesoCompra);

            var response = await GetContentString(httpResponseMessage);
            dynamic? resp = JsonConvert.DeserializeObject(response);

            bool sucess = resp!.success;
            string message = resp!.message;
            if (sucess) {
                JArray result = resp!.result;
                dynamic resultData = result[0].First().First();
                var resultValue = resultData.Value;

                string data = $@"<div style='padding:0 50px; width:400px;'>
                                <p>IdTransaccion: <b>{resultValue}</b> </p>
                                <p>Descripcion: <b>{Descripcion}</b> </p>
                                <p>Fecha: <b>{Fecha}</b> </p>
                                <p>Monto: <b>{Monto}</b> </p></div>";

                resp.success = true;
                resp.message = data;

                return resp;
            }
            else
                throw new Exception(message);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "PruebaTecnicaAuthScheme")]
        public async Task<dynamic?> ProcesarPago([FromForm] int idTarjeta, [FromForm] string Descripcion, [FromForm] string Fecha, [FromForm] string Monto)
        {
            var ProcesarPago = new
            {
                idTarjeta,
                Descripcion,
                Fecha,
                Monto
            };

            using var httpResponseMessage = await PostAsync("/api/Transacciones/ProcesarPago", ProcesarPago);

            var response = await GetContentString(httpResponseMessage);
            dynamic? resp = JsonConvert.DeserializeObject(response);

            bool sucess = resp!.success;
            string message = resp!.message;
            if (sucess)
            {
                JArray result = resp!.result;
                dynamic resultData = result[0].First().First();
                var resultValue = resultData.Value;

                string data = $@"<div style='padding:0 50px; width:400px;'>
                                <p>IdTransaccion: <b>{resultValue}</b> </p>
                                <p>Descripcion: <b>{Descripcion}</b> </p>
                                <p>Fecha: <b>{Fecha}</b> </p>
                                <p>Monto: <b>{Monto}</b> </p></div>";

                resp.success = true;
                resp.message = data;

                return resp;
            }
            else
                throw new Exception(message);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "PruebaTecnicaAuthScheme")]
        public async Task<dynamic?> GetTarjetasCredito()
        {
            var userAuth = GetUsuarioID();

            using var httpResponseMessage = await GetAsync(string.Concat("/api/Transacciones/GetTarjetasCredito?userAuth=", userAuth), true);
            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            dynamic? resp = JsonConvert.DeserializeObject(response);

            bool sucess = resp!.success;
            dynamic result = resp!.result;
            string message = resp!.message;

            if (sucess)
                return result;
            else
                throw new Exception(message);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "PruebaTecnicaAuthScheme")]
        public async Task<dynamic?> GetTransacciones([FromQuery] int idTarjeta, [FromQuery] int Periodo, [FromQuery] int Mes )
        {
            string parametros = string.Concat("idTarjeta=", idTarjeta, "&Periodo=", Periodo, "&Mes=", Mes);

            using var httpResponseMessage = await GetAsync(string.Concat("/api/Transacciones/GetTransacciones?", parametros), true);

            return await GetContentString(httpResponseMessage);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "PruebaTecnicaAuthScheme")]
        public async Task<dynamic?> GetEstadoCuenta([FromQuery] int idTarjeta, [FromQuery] int Periodo, [FromQuery] int Mes)
        {
            string parametros = string.Concat("idTarjeta=", idTarjeta, "&Periodo=", Periodo, "&Mes=", Mes);

            using var httpResponseMessage = await GetAsync(string.Concat("/api/Transacciones/GetEstadoCuenta?", parametros), true);

            return await GetContentString(httpResponseMessage);
        }

        public string GetUsuarioID() {

            var claimsIdentity = User.Identity as ClaimsIdentity;
            return claimsIdentity!.Claims.ToList()[3].Value.ToString().Split(',')[1];
        }
    }
}
