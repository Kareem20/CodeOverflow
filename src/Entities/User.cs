namespace CodeOverFlow.Entities
{

    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public string PasswordHashed { get; set; }

        public List<int> PreferredTags { get; set; } = new List<int>();

        public User() { }

        public User(string username, string email, string password)
        {
            Username = username;
            Email = email;
            PasswordHashed = HashPassword(password);
        }
        private string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
        public bool ConfirmePassword(String password)
        {
            return this.PasswordHashed == (password) || this.PasswordHashed == HashPassword(password);
        }

    }
}