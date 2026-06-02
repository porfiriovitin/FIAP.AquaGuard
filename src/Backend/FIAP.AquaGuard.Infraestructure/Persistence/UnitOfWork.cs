using FIAP.AquaGuard.Infrastructure.Persistence;
using FIAP.AquaGuard.Domain.Repositories;

namespace FIAP.AquaGuard.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}

