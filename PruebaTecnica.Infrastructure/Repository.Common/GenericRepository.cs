using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Infrastructure.Utility;

namespace PruebaTecnica.Infrastructure.Repository.Common
{
    public abstract class GenericRepository(IDataSource dataSource)
    {
		private readonly IDataSource _source = dataSource;

        public virtual async Task<dynamic?> ExecuteQueryAsync<T>(ParameterServer<T> parameterServer) { 
		
			return await _source.ExecuteQueryAsync<T>(parameterServer);
		}
    }
}
