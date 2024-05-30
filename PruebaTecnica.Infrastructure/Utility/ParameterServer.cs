using Dapper;
using PruebaTecnica.Infrastructure.Enums;
using System.Data;

namespace PruebaTecnica.Infrastructure.Utility
{
    public struct ParameterServer<T>
	{
		public ParameterServer()
		{
		}

		public string? NombreServer { get; set; } = "SQLServerPruebaTecnica";
		public string? NombreBase { get; set; } = "PruebaTecnica";
		public TipoResult TipoResult { get; set; } = TipoResult.Single;
		public CommandType TipoComando { get; set; } = CommandType.Text;
		public string? QuerySP { get; set; }
		public DynamicParameters? Parameters { get; set; } = null;
		public List<string>? Tablas { get; set; }
	}
}
