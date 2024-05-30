namespace PruebaTecnica.Application.Interfaces.Repository
{
    public interface ITransacciones
    {
        public Task<dynamic?> ProcesarCompra(dynamic datos);
        public Task<dynamic?> ProcesarPago(dynamic datos);
        public Task<dynamic?> GetHistorialTransacciones(int idTarjeta, int Periodo, int Mes);
        public Task<dynamic?> GetEstadoCuenta(int idTarjeta, int Periodo, int Mes);

        public Task<dynamic?> GetTarjetasCredito(int usuarioID);
    }
}
