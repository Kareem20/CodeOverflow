using CodeOverFlow.Entities;
using CodeOverFlow.Services;

namespace CodeOverflow.ui
{
    public class ConsoleUI
    {
        private readonly UserService _userService = new UserService();
        private readonly QuestionService _questionService = new QuestionService();
        private readonly AnswerService _answerService = new AnswerService();
        private readonly VoteService _voteService = new VoteService();
        private User currentUser = null;

        public void Start()
        {
            bool exit = false;
            while (!exit)
            {
                DisplayMainMenu();
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        CreateUser();
                        break;
                    case "2":
                        Login();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        Start();
                        break;
                }
            }
        }
        private void DisplayMainMenu()
        {
            Console.WriteLine("Welcome to CodeOverflow :)" +
                "\nSelect an Option:" +
                "\n1: Create New User." +
                "\n2: Login." +
                "\n0: Exit.");
            Console.WriteLine("Enter your choice: ");
        }
        private void CreateUser()
        {
            string username, email, password;
            while (true)
            {
                Console.WriteLine("Enter username: ");
                username = Console.ReadLine();
                if (_userService.IsUsernameTaken(username))
                {
                    Console.WriteLine("This username is already taken. Please try another.");
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.WriteLine("Enter email: ");
                email = Console.ReadLine();
                if (!_userService.IsValidEmail(email))
                {
                    Console.WriteLine("Invalid email format. Please enter a valid email.");
                }
                else
                if (_userService.IsEmailTaken(email))
                {
                    Console.WriteLine("This email is already registered. Please try another.");
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine("Enter password: ");
            password = Console.ReadLine();
            _userService.RegisterUser(username, email, password);
            Console.WriteLine("User registration successful. You can now login.");
        }
        private void Login()
        {
            Console.WriteLine("Enter username or email: ");
            string identifier = Console.ReadLine();
            Console.WriteLine("Enter password: ");
            string password = Console.ReadLine();
            currentUser = _userService.Login(identifier, password);
            if (currentUser != null)
            {
                Console.WriteLine($"Login successful. Welcome,{currentUser.Username}!");
                DisplayUserOptions();
            }
            else
            {
                Console.WriteLine("Login failed. Please try again.");
            }
        }
        private void DisplayUserOptions()
        {
            bool backToMainMenu = false;
            while (!backToMainMenu)
            {
                Console.WriteLine("User Options: " +
                    "\n1: Ask a Question." +
                    "\n2: Browse the feed." +
                    "\n3: Manage preferred tags." +
                    "\n4: Logout.");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AskQuestion();
                        break;
                    case "2":
                        BrowseFeed();
                        break;
                    case "3":
                        ManagePreferredTags();
                        break;
                    case "4":
                        Logout();
                        backToMainMenu = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        DisplayUserOptions();
                        break;
                }
            }
        }
        private void AskQuestion() { }
        private void BrowseFeed() { }
        private void ManagePreferredTags() { }
        private void Logout() { }
    }
}
