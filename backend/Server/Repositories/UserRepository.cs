using Microsoft.EntityFrameworkCore;
using Server.Domain.Entities;
using Server.UserRepositories;

namespace Server.Repositories
{
    public class UserRepository(AppDbContext appDbContext) : IUserRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task Create(User user)
        {
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            _appDbContext.Users?.Remove(user);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<User>> Get()
        {
            return await _appDbContext.Users.ToListAsync();
        }

        public async Task<User?> Get(int id)
        {
            return await _appDbContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<User?> Get(string number)
        {
            return _appDbContext.Users.Where(x => x.Number == number).FirstOrDefaultAsync();
        }

        public async Task Update(User user)
        {
            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
