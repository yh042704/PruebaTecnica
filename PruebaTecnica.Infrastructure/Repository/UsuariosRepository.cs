using Dapper;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Application.Interfaces.Repository;
using PruebaTecnica.Infrastructure.Enums;
using PruebaTecnica.Infrastructure.Repository.Common;
using PruebaTecnica.Infrastructure.Utility;
using PruebaTecnica.SQL.Queries;

namespace PruebaTecnica.Infrastructure.Repository
{
    public class UsuariosRepository(IDataSource dataSource) : GenericRepository(dataSource), IUsuariosRepository
    {
        public async Task<dynamic?> RegistrarUsuario(dynamic JsonData)
        {
            string usuario = JsonData.usuario;
            string nombre = JsonData.nombre;
            string direccion = JsonData.direccion;
            string noDocumento = JsonData.noDocumento;
            string password = JsonData.password;

            DynamicParameters p = new();
            p.Add("@usuario", usuario);
            p.Add("@nombre", nombre);
            p.Add("@password", password);
            p.Add("@direccion", direccion);
            p.Add("@noDocumento", noDocumento);

            ParameterServer<dynamic> ps = new()
            {
                TipoResult = TipoResult.SP,
                QuerySP = Usuarios.UsuarioInsert,
                Parameters = p
            };

            return await base.ExecuteQueryAsync(ps);
        }
    }
}
