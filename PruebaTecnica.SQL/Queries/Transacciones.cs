namespace PruebaTecnica.SQL.Queries
{
    public static class Transacciones
    {
        public static string TarjetasCreditoObtener => @"sp_GetTarjetasCredito";
        public static string ComprasInsertar => @"sp_TransaccionInsertar";
        public static string PagosInsertar => @"sp_TransaccionInsertar";
        public static string TansaccionesObtener => @"sp_TransaccionObtener";
        public static string EstadoCuentaObtener => @"sp_EstadoCuenta";
    }
}
