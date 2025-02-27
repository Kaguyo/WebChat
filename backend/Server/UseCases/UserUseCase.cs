using Server.Domain.Entities;
using Server.Repositories;

namespace Server.UseCases
{
    public class UserUseCase(IUserRepository userRepository)
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<int> CreateUser(User user)
        {
            var CreateUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Number = user.Number,
                Password = user.Password,
            };

            ValidateUser(CreateUser);
            Console.WriteLine("Validação de usuario concluida!");
            await _userRepository.Create(CreateUser);
            return CreateUser.Id;
        }

        public async Task<User?> GetUserId(int id)
        {
            return await _userRepository.Get(id);
        }

        public async Task<User?> GetUserByNumber(string number, string password)
        {
            var loginId = await _userRepository.GetNumber(number);
            var loginID2 = await _userRepository.GetPassword(password);

            if (loginId == loginID2)
            {
                return loginId;
            }
            else
            {
                return null;
            }
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

        private static void ValidateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName))
                throw new ArgumentException("First Name is required.");

            if (string.IsNullOrWhiteSpace(user.LastName))
                throw new ArgumentException("Last Name is required.");

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
