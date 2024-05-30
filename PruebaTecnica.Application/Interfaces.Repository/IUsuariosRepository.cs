namespace PruebaTecnica.Application.Interfaces.Repository
{
    public interface IUsuariosRepository
    {
        public Task<dynamic?> RegistrarUsuario(dynamic JsonData);
    }
}
