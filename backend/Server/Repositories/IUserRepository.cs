using Server.Domain.Entities;

namespace Server.Repositories
{
    public interface IUserRepository
    {
        public Task<List<User>> Get();
        public Task<User?> Get(int id);
        public Task<User?> Get(string number);
        public Task Create(User user);
        public Task Update(User user);
        public Task Delete(User user);
    }
}