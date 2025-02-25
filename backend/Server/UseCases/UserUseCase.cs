using Server.Domain.Entities;
using Server.Repositories;



namespace Server.UseCases
{

    public class UserUseCase(IUserRepository userRepository)
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task CreateUser(string username, string number, string password)
        {
            var user = new User { Username = username, Number = number, Password = password };
            
            ValidateUser(user);
            await _userRepository.Create(user);
            
        }

        public async Task<User?> GetUserId(int id)
        {
            return await _userRepository.Get(id);
        }

        public async Task UpdateUser(User user)
        {
            await _userRepository.Update(user);
        }

        public async Task DeleteUser(int id)
        {
            var user = await _userRepository.Get(id);
            if (user != null)
            {
                await _userRepository.Delete(user);
            }
        }
        private void ValidateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username is required.");

            if (string.IsNullOrWhiteSpace(user.Number))
                throw new ArgumentException("Number is required.");

            if (string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Password is required.");

            if (user.Password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters long.");
        }
    }


}
