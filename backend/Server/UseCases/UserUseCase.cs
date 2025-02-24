using Server.UserRepository;
using Server.Domain;


namespace Server.UseCases
{
    public class UserUseCase
    {
        
        private readonly IUserRepository _repository;

        public UserUseCase(IUserRepository repository)
        {
            _repository = repository;
        }

        public void CreateUser(string username, string number, string password)
        {
            var user = new User
            {
                Username = username,
                Number = number,
                Password = password
            };

            ValidateUser(user);
            _repository.Add(user);
        }

        public User? GetUserByNumber(string number)
        {
            return _repository.GetByNumber(number);
        }

        public void UpdateUser(User user)
        {
            ValidateUser(user);
            _repository.Update(user);
        }

        public void DeleteUser(string number)
        {
            var user = _repository.GetByNumber(number);
            if (user == null)
                throw new ArgumentException("User not found.");

            _repository.Delete(user);
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