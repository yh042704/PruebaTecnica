using Newtonsoft.Json;
using PruebaTecnica.Core.Dtos;
using System.Net;

namespace PruebaTecnica.Api.Controllers.ErrorHandling
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);

                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Validación del Token ha fallado. Acceso denegado");
                }
            }
            catch (Exception exception)
            {
                var errorResult = new ApiResponse(
                    success: false,
                    message: exception.Message.Trim(),
                    result: exception.StackTrace!
                );


                if (exception.InnerException != null)
                {
                    errorResult.message += (", " + exception.InnerException);
                }

                var response = context.Response;
                if (!response.HasStarted)
                {
                    string json = JsonConvert.SerializeObject(errorResult);
                    response.ContentType = "application/json";
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await response.WriteAsync(json);
                }
            }
        }
    }
}
