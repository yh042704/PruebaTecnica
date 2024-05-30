using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Controllers.Base;

namespace PruebaTecnica.Controllers
{
    public class HomeController(IHttpClientFactory httpClient) : BaseController(httpClient)
    {
        [HttpPost]
        public async Task<dynamic?> EnviarContactos([FromBody] dynamic datos)
        {
            using var httpResponseMessage = await PostAsync("/api/Home/EnviarContactos", datos);

            return await GetContentString(httpResponseMessage);
        }

        [HttpPost]
        public async Task<dynamic?> RegistrarUsuario([FromBody] dynamic datos)
        {
            using var httpResponseMessage = await PutAsync("/api/Usuarios/RegistrarUsuario", datos);

            return await GetContentString(httpResponseMessage);
        }

        [HttpGet]
        public async Task<IActionResult> LoadPartialView([FromQuery] string namePage)
        {
            await Task.Factory.StartNew(() =>
            {
                string[] IdOpcion = namePage.Split(',');

                if (IdOpcion.Length > 1)
                {
                    namePage = IdOpcion[1];
                    ViewBag.IdOpcion = IdOpcion[0];
                }
                else
                {
                    ViewBag.IdOpcion = "0";
                };

                ViewBag.clone = namePage.StartsWith("D-");
                namePage = namePage.TrimEnd().Replace("D-", "");
            });

            return PartialView(string.Concat("/Pages/", namePage, ".cshtml"));
        }
    }
}
