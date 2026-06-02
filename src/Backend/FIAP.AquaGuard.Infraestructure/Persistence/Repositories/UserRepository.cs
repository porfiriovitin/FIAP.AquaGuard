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

        public async Task<(List<User> Users, int Total)> ListUsersAsync(int page = 1, int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var total = await _context.Users.CountAsync();

            var users = await _context.Users.AsNoTracking().OrderBy(c => c.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (users, total);
        }

        public async Task<(List<User> Users, int Total)> ListUsersPerCityAsync(int page = 1, int pageSize = 10, Guid cityId = default)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var total = await _context.Users.Where(y => y.CityId == cityId).CountAsync();

            var users = await _context.Users.AsNoTracking().Where(y => y.CityId == cityId).OrderBy(c => c.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (users, total);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }
    }
}
