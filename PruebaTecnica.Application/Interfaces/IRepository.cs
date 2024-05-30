namespace PruebaTecnica.Application.Interfaces
{
    public interface IRepository<T> where T: class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(string where);
        //Task<IReadOnlyList<T>> GetByFiltersAsync(string procName, string database, CommandType cmdType, Parameter[] DBParameters = null);
        Task<string> AddAsync(T jsonAdd);
        Task<string> UpdateAsync(T jsonUpdate);
        Task<string> DeleteAsync(T jsonDelete);
        Task<string> BatchAsync(T jsonBatch);
    }
}
