using CodeOverFlow.Data;
using CodeOverFlow.Entities;

namespace CodeOverFlow.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public void RegisterUser(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(username)
                || string.IsNullOrWhiteSpace(email)
                || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Invalid input. Registration failed");
                return;
            }
            User new_user = new User(username, email, password);
            _userRepository.Add(new_user);
            Console.WriteLine("User registered successfully.");
        }
        // @identifier refer to username or password.
        public User? Login(string identifier, string password)
        {
            List<User> users = _userRepository.getAll();
            foreach (User user in users)
            {
                if ((user.Username.Equals(identifier) || user.Email.Equals(identifier))
                    && user.ConfirmePassword(password))
                {
                    return user;
                }
            }
            Console.WriteLine("Invalid credentails.");
            return null;
        }
        public List<int> GetPreferredTags(User user)
        {
            List<int> tagsId = new List<int>();
            foreach (Tag tag in _userRepository.GetUserPreferredTag(user.UserID))
                tagsId.Add(tag.TagID);
            return tagsId;
        }
        public User GetById(int id) => _userRepository.getByID(id);

        public bool IsUsernameTaken(string username) => _userRepository.CheckUsernameExists(username);
        public bool IsEmailTaken(string email) => _userRepository.CheckEmailExists(email);
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
