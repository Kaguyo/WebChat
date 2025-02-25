using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Server.Domain;


namespace Server.UserRepository
{
    public class UserRepository : IUserRepository
    {
        
        private readonly List<User> _users = new();

        
        public void Add(User user)
        {
            _users.Add(user);
        }

        public User? GetByNumber(string number)
        {
            return _users.FirstOrDefault(u => u.Number == number);
        }

        public void Update(User user)
        {
            var existingUser = GetByNumber(user.Number);
            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                existingUser.Password = user.Password;
                existingUser.ProfileImage = user.ProfileImage;
            }
        }

        public void Delete(User user)
        {
            _users.Remove(user);
        }
    }
}