using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaTecnica.Controllers.Enums;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace PruebaTecnica.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor? _httpContextAccessor;
        private HttpClient? _httpClient;

        public BaseController(IHttpClientFactory httpClient)
        {
            _clientFactory = httpClient;
        }

        public BaseController(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        protected async Task<HttpResponseMessage> PostAsync(string url, dynamic _userData, bool validarCredenciales = false) {

            return await PutPostDeleteAsync(url, _userData, TipoHTTP.Post, validarCredenciales);
        }

        protected async Task<HttpResponseMessage> PutAsync(string url, dynamic _userData, bool validarCredenciales = false)
        {

            return await PutPostDeleteAsync(url, _userData, TipoHTTP.Put, validarCredenciales);
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string url, dynamic _userData, bool validarCredenciales = false)
        {

            return await PutPostDeleteAsync(url, _userData, TipoHTTP.Delete, validarCredenciales);
        }

        private async Task<HttpResponseMessage> PutPostDeleteAsync(string url, dynamic _userData, TipoHTTP Tipo, bool validarCredenciales)
        {
            StringContent? data = null;

            if (!Tipo.Equals(TipoHTTP.Delete))
            {
                data = new StringContent(JsonConvert.SerializeObject(_userData),
                                              Encoding.UTF8,
                                              Application.Json);
            }

            Config(validarCredenciales);

            if (Tipo.Equals(TipoHTTP.Post))
            {
                return await _httpClient!.PostAsync(url, data);
            }
            else if (Tipo.Equals(TipoHTTP.Put))
            {
                return await _httpClient!.PutAsync(url, data);
            }

            return await _httpClient!.DeleteAsync(string.Concat(url, "/", _userData));
        }

        protected async Task<HttpResponseMessage> GetAsync(string url, bool validarCredenciales = false)
        {
            Config(validarCredenciales);
         
            return await _httpClient!.GetAsync(url); ;
        }

        protected async Task<string> GetContentString(HttpResponseMessage _httpResponseMessage, bool enviarNotificacion = false, string topic = "")
        {
            string response = await _httpResponseMessage.Content.ReadAsStringAsync();
            if (response.Equals(""))
            {
                throw new Exception(_httpResponseMessage.ReasonPhrase);
            }

            return response;
        }

        protected void Config(bool validarCredenciales)
        {
            _httpClient = _clientFactory.CreateClient("RestApi");

            if (validarCredenciales )
            {
                var claim = HttpContext.User.FindFirst(ClaimTypes.UserData)?.Value;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", claim);
            }
        }
    }
}
