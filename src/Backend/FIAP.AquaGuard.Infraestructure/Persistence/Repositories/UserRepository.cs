using FIAP.AquaGuard.Infrastructure.Persistence;
using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FIAP.AquaGuard.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<User?> GetByEmailAsync(string email) => await _context.Users.FirstOrDefaultAsync(y => y.Email == email);

        public async Task<User?> GetByIdAsync(Guid id) => await _context.Users.FirstOrDefaultAsync(y => y.Id == id);

        public void Update(User user)
        {
            _context.Users.Update(user);
        }
    }
}
