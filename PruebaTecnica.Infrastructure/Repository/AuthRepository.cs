using Dapper;
using Microsoft.Extensions.Configuration;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Application.Interfaces.Repository;
using PruebaTecnica.Core.Dtos;
using PruebaTecnica.Infrastructure.Enums;
using PruebaTecnica.Infrastructure.Repository.Common;
using PruebaTecnica.Infrastructure.Utility;
using PruebaTecnica.SQL.Queries;

namespace PruebaTecnica.Infrastructure.Repository
{
    public class AuthRepository(IDataSource dataSource, IConfiguration configuration) : GenericRepository(dataSource), IAuthRepository
	{
		private readonly IDataSource _source = dataSource;
		private readonly IConfiguration _configuration = configuration;

        public async Task<bool> ValidUser(dynamic user)
		{
			string userId = user.usuario;
			string password = user.password;

			DynamicParameters p = new();
			p.Add("@IdUsuario", userId);
			p.Add("@Password", password);

			ParameterServer<UserAuthDto> ps = new()
			{
				TipoResult = TipoResult.List,
				QuerySP = Auth.AuthSQL,
				Parameters = p
			};

			if (await base.ExecuteQueryAsync<UserAuthDto>(ps) is IReadOnlyList<UserAuthDto> data)
			{
				if (data.Any())
				{
                    if (data[0].PassCorrect!.Equals("0"))
                    {
                        throw new Exception("Usuario/Contraseña incorrecta, favor verificar...");
                    }
                    else
                    {
						user.usuarioID = data[0].usuarioID!;
						user.password = "";
                        user.nombre = data[0].nombre!;
                        user.Result = "Ingreso exitoso";
                        user.TokenJWT = JWTUtils.GetToken(userId, _configuration["Jwt:Key"]!);
                    }                    
				}else
				{
                    throw new Exception("Usuario/Contraseña incorrecta, favor verificar...");
                }
			}

			return true;
		}
	}
}
