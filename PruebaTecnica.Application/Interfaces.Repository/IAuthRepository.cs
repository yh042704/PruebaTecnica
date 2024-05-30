namespace PruebaTecnica.Application.Interfaces.Repository
{
    public interface IAuthRepository
    {
        public Task<bool> ValidUser(dynamic user);
    }
}
