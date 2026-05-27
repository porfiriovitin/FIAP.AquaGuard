namespace FIAP.AquaGuard.Domain.Repositories;

public interface IUnitOfWork
{
    Task CommitAsync();
}
