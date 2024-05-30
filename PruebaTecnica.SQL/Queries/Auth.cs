namespace PruebaTecnica.SQL.Queries
{
	public static class Auth
	{
		public static string AuthSQL => @"SELECT usuarioID, usuario, nombre, 
										CASE WHEN [password] = HASHBYTES('SHA2_256', CAST(@Password AS NVARCHAR(4000))) THEN '1' ELSE '0' END PassCorrect
										FROM Usuarios WHERE usuario = @IdUsuario";
	}
}
