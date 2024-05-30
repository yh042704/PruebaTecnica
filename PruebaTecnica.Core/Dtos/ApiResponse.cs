namespace PruebaTecnica.Core.Dtos
{
    public class ApiResponse
    {
        public ApiResponse()
        {
        }

        public ApiResponse(bool success, string? message, dynamic result)
        {
            this.success = success;
            this.message = message;
            this.result = result;
        }

        public bool success { get; set; } 
        public string? message { get; set; }
        public dynamic? result { get; set; }
    }
}
