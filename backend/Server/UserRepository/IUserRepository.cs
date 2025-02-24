using Server.Domain;

namespace Server.UserRepository
{
    public interface IUserRepository
    {
        void Add(User user);
        User? GetByNumber(string number);
        void Update(User user);
        void Delete(User user);
    }
}
