namespace PruebaTecnica.Application.Interfaces
{
    public interface IDataSource
    {
        public Task<dynamic?> ExecuteQueryAsync<T>(dynamic parameterServer);
        public void Dispose();
    }
}
