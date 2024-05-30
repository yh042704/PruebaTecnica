using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaTecnica.Controllers.Base;
using System.Dynamic;
using System.Security.Claims;

namespace PruebaTecnica.Controllers
{
    public class LoginController(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor) : BaseController(httpClient, httpContextAccessor)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        [HttpPost]
        public async Task<string> Login([FromBody] dynamic _userData)
        {
            var httpResponseMessage = await PostAsync("/api/Login/Login", _userData);
            string response = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                dynamic? resp = JsonConvert.DeserializeObject(response);
                bool sucess = resp!.success;

                if (sucess)
                {
                    string userid = _userData.usuario;
                    string result = resp!.result;
                    string message = resp!.message;

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString("N")),
                        new Claim(ClaimTypes.Name, userid),
                        new Claim(ClaimTypes.UserData, result),
                        new Claim(Utility.UtilityPT.UserConf, message)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "PruebaTecnicaAuthScheme");

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true,
                        ExpiresUtc = DateTime.UtcNow.AddHours(8)
                    };

                    var cookieOptions = new CookieOptions()
                    {
                        Expires = authProperties.ExpiresUtc,
                        Path = "/"
                    };

                    Response.Cookies.Append("usuarioID", message.Split(",")[1], cookieOptions);
                    Response.Cookies.Append("nombre", message.Split(",")[2], cookieOptions);
                    Response.Cookies.Append("usuario", userid.Split("@")[0], cookieOptions);

                    _httpContextAccessor.HttpContext!.User.AddIdentity(claimsIdentity);

                    await _httpContextAccessor.HttpContext!.SignInAsync(
                        "PruebaTecnicaAuthScheme",
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                }
            };

            return response;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "PruebaTecnicaAuthScheme")]
        public async Task<string> LogOut()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync("PruebaTecnicaAuthScheme");

            Response.Cookies.Delete("usuarioID");
            Response.Cookies.Delete("nombre");
            Response.Cookies.Delete("usuario");

            dynamic errorResult = new ExpandoObject();
            errorResult.success = true;
            errorResult.message = "success";
            errorResult.result = "Desconectado";

            return JsonConvert.SerializeObject(errorResult);
        }
    }
}
