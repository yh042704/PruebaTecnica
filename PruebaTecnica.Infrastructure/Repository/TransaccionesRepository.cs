using Dapper;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Application.Interfaces.Repository;
using PruebaTecnica.Infrastructure.Enums;
using PruebaTecnica.Infrastructure.Repository.Common;
using PruebaTecnica.Infrastructure.Utility;
using PruebaTecnica.SQL.Queries;

namespace PruebaTecnica.Infrastructure.Repository
{
    public class TransaccionesRepository(IDataSource dataSource) : GenericRepository(dataSource), ITransacciones
    {
        public async Task<dynamic?> GetTarjetasCredito(int usuarioID)
        {
           
            DynamicParameters p = new();
            p.Add("@usuarioID", usuarioID);

            ParameterServer<dynamic> ps = new()
            {
                TipoResult = TipoResult.List,
                TipoComando = System.Data.CommandType.StoredProcedure,
                QuerySP = Transacciones.TarjetasCreditoObtener,
                Parameters = p
            };

            return await base.ExecuteQueryAsync(ps);
        }

        public async Task<dynamic?> GetHistorialTransacciones(int idTarjeta, int Periodo, int Mes)
        {
            DynamicParameters p = new();
            p.Add("@idTarjeta", idTarjeta);
            p.Add("@Periodo", Periodo);
            p.Add("@Mes", Mes);
            ParameterServer<dynamic> ps = new()
            {
                TipoResult = TipoResult.List,
                TipoComando = System.Data.CommandType.StoredProcedure,
                QuerySP = Transacciones.TansaccionesObtener,
                Parameters = p
            };

            return await base.ExecuteQueryAsync(ps);

        }

        public async Task<dynamic?> GetEstadoCuenta(int idTarjeta, int Periodo, int Mes)
        {
            DynamicParameters p = new();
            p.Add("@idTarjeta", idTarjeta);
            p.Add("@Periodo", Periodo);
            p.Add("@Mes", Mes);
            ParameterServer<dynamic> ps = new()
            {
                TipoResult = TipoResult.Multi,
                Tablas = ["EstadoCuentaEnc", "EstadoCuentaDetalle"],
                TipoComando = System.Data.CommandType.StoredProcedure,
                QuerySP = Transacciones.EstadoCuentaObtener,
                Parameters = p
            };

            return await base.ExecuteQueryAsync(ps);
        }

        public async Task<dynamic?> ProcesarCompra(dynamic datos)
        {
            int idTarjeta = datos.idTarjeta;
            string descripcion = datos.Descripcion;
            string fecha = datos.Fecha;
            decimal monto = datos.Monto;

            DynamicParameters p = new();
            p.Add("@idTarjeta", idTarjeta);
            p.Add("@descripcion", descripcion);
            p.Add("@fecha", PTUtils.ConvertDateTimeSQL(fecha));
            p.Add("@monto", -monto);
            p.Add("@TipoMovimiento", 1);

            ParameterServer<dynamic> ps = new()
            {
                TipoResult = TipoResult.List,
                TipoComando = System.Data.CommandType.StoredProcedure,
                QuerySP = Transacciones.ComprasInsertar,
                Parameters = p
            };

            return await base.ExecuteQueryAsync(ps);
        }

        public async Task<dynamic?> ProcesarPago(dynamic datos)
        {
            int idTarjeta = datos.idTarjeta;
            string descripcion = datos.Descripcion;
            string fecha = datos.Fecha;
            decimal monto = datos.Monto;

            DynamicParameters p = new();
            p.Add("@idTarjeta", idTarjeta);
            p.Add("@descripcion", descripcion);
            p.Add("@fecha", PTUtils.ConvertDateTimeSQL(fecha));
            p.Add("@monto", monto);
            p.Add("@TipoMovimiento", 2);

            ParameterServer<dynamic> ps = new()
            {
                TipoResult = TipoResult.List,
                TipoComando = System.Data.CommandType.StoredProcedure,
                QuerySP = Transacciones.PagosInsertar,
                Parameters = p
            };

            return await base.ExecuteQueryAsync(ps);
        }
    }
}
