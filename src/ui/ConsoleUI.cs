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
        private readonly TagService _tagService = new TagService();
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
            Console.WriteLine("You can now login.");
            DisplayLine();
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
                DisplayLine();
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
                //TODO: Edite your own questions
                Console.WriteLine("User Options: " +
                    "\n1: Browse the feed." +
                    "\n2: Ask a Question." +
                    "\n3: Manage preferred tags." +
                    "\n4: Logout.");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        BrowseFeed(); // TODO make the feed show user's question also.
                        break;
                    case "2":
                        AskQuestion();
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
        private void BrowseFeed()
        {
            List<Tag> userPreferredTags = _userService.GetPreferredTags(currentUser);
            if (userPreferredTags.Count == 0)
            {
                Console.WriteLine("There is no questions based on your preferred tags." +
                    "\nYou can update your preferred tags.");
                return;
            }
            List<Question> preferredQuestions = _questionService.GetFeed(userPreferredTags);
            int cnt = 1;
            foreach (Question question in preferredQuestions)
            {
                Console.WriteLine($"{cnt++}: {question.Title}?" +
                    $"\n    {question.Body}" +
                    $"\n        Upvotes: {_voteService.GetUpvotes(question.ID)} / Downvotes: {_voteService.GetDownvotes(question.ID)}" +
                    $"\n Author: {_userService.GetById(question.AuthorID).Username}");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            }
            while (true)
            {
                Console.WriteLine("Choose an option" +
                    "\n1: To view a specific question." +
                    "\n2: Vote a specific question." +
                    "\n3: Return to main options.");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    while (true)
                    {
                        Console.WriteLine("Enter the number of question.");
                        string indexChoice = Console.ReadLine();
                        int questionIndex = int.Parse(indexChoice) - 1;
                        if (questionIndex < 0 || questionIndex >= preferredQuestions.Count)
                        {
                            Console.WriteLine("Please select a correct question number.");
                        }
                        else
                        {
                            ViewSpecificQuestion(preferredQuestions[questionIndex]);
                            DisplayLine();
                            break;
                        }
                    }
                    break;
                }
                else if (choice == "2")
                {
                    while (true)
                    {
                        Console.WriteLine("Choose an option" +
                       "\n1: Upvote a specific question" +
                       "\n2: Downvote a specific question" +
                       "\n3: Return to main options.");
                        string voteChoice = Console.ReadLine();
                        if (voteChoice == "1" || voteChoice == "2")
                        {
                            while (true)
                            {
                                Console.WriteLine("Enter the number of question.");
                                string indexChoice = Console.ReadLine();
                                int questionIndex = int.Parse(indexChoice) - 1;
                                if (questionIndex < 0 || questionIndex >= preferredQuestions.Count)
                                {
                                    Console.WriteLine("Please select a correct question number.");
                                }
                                else
                                {
                                    _voteService.VotePost(currentUser
                                        , preferredQuestions[questionIndex].ID
                                        , null
                                        , preferredQuestions[questionIndex].AuthorID
                                        , (voteChoice == "1" ? 1 : -1));
                                    break;
                                }
                            }
                            break;
                        }
                        else if (voteChoice == "3")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid option. Please try again.");
                        }
                    }
                    break;
                }
                else if (choice == "3")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }
            }
        }
        private void ViewSpecificQuestion(Question question)
        {
            Console.WriteLine($"- {question.Title}" +
                $"\n  {question.Body}" +
                $"\n        Upvotes: {_voteService.GetUpvotes(question.ID)} / Downvotes: {_voteService.GetDownvotes(question.ID)}" +
                $"\n Author: {_userService.GetById(question.AuthorID).Username}");
            List<Answer> answers = _answerService.GetListOfAnswer(question.ID);
            if (answers.Count == 0)
            {
                Console.WriteLine("There is no answers for this question");
            }
            else
            {
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~Answers~~~~~~~~~~~~~~~~~~~~");
                foreach (Answer answer in answers)
                {
                    int cnt = 1;
                    Console.WriteLine($"{cnt++}: {answer.Body}" +
                        $"\n    Author: {_userService.GetById(answer.AuthorID).Username}" +
                        $"\n        Upvotes: {_voteService.GetUpvotes(answer.ID)} / Downvotes: {_voteService.GetDownvotes(answer.ID)}");
                }
                while (true)
                {
                    Console.WriteLine("Choose an option" +
                   "\n1: Upvote a specific answer" +
                   "\n2: Downvote a specific answer" +
                   "\n3: Return to main options.");
                    string choice = Console.ReadLine();
                    if (choice == "1" || choice == "2")
                    {
                        while (true)
                        {
                            Console.WriteLine("Enter the number of answer.");
                            string indexChoice = Console.ReadLine();
                            int answerIndex = int.Parse(indexChoice) - 1;
                            if (answerIndex < 0 || answerIndex >= answers.Count)
                            {
                                Console.WriteLine("Please select a correct answer number.");
                            }
                            else
                            {
                                _voteService.VotePost(currentUser
                                    , null
                                    , answers[answerIndex].ID
                                    , answers[answerIndex].AuthorID
                                    , (choice == "1" ? 1 : -1));
                                break;
                            }
                        }
                        break;
                    }
                    else if (choice == "3")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Please try again.");
                    }
                }
            }
        }
        private void AskQuestion()
        {
            Console.WriteLine("Enter the main title of the question.");
            string questionTitle = Console.ReadLine();
            Console.WriteLine("Enter the body of the question.");
            string questionBody = Console.ReadLine();
            List<Tag> questionTags = new List<Tag>();
            Console.Write("Enter tags (comma-separated, or leave blank): ");
            string rawTags = Console.ReadLine() ?? "";
            var tagNames = rawTags
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim().ToLower())
                .Where(t => !string.IsNullOrEmpty(t))
                .Distinct()
                .ToList();
            foreach (var name in tagNames)
            {
                questionTags.Add(new Tag
                {
                    TagID = _tagService.GetOrCreateTagId(name),
                    TagName = name
                });
            }
            _questionService.AskQuestion(currentUser, questionTitle, questionBody, questionTags);
        }
        private void ManagePreferredTags()
        {
            List<Tag> preferredTags = _userService.GetPreferredTags(currentUser);
            Console.WriteLine("Your prefered tags.");
            int cnt = 1;
            foreach (Tag tag in preferredTags)
                Console.WriteLine($"{cnt++}: {tag.TagName}");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            while (true)
            {
                Console.WriteLine("Choose an option" +
                    "\n1: Add a new Tag." +
                    "\n2: Delete a preferred Tag." +
                    "\n3: Back.");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    Console.WriteLine("Enter the tag name.");
                    string new_tag = Console.ReadLine();
                    int new_add_tag_id = _tagService.GetOrCreateTagId(new_tag);
                    _userService.AddPreferredTag(currentUser.UserID, new_add_tag_id);
                    break;
                }
                else if (choice == "2")
                {
                    while (true)
                    {
                        Console.WriteLine("Enter the deleted tag.");
                        string indexChoice = Console.ReadLine();
                        int tagIndex = int.Parse(indexChoice) - 1;
                        if (tagIndex < 0 || tagIndex >= preferredTags.Count)
                        {
                            Console.WriteLine("Please select a correct tag number.");
                        }
                        else
                        {
                            _userService.DeletePreferredTag(currentUser.UserID, preferredTags[tagIndex].TagID);
                            break;
                        }
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }
            }
        }
        private void Logout() => Console.WriteLine("See you late :).");
        private void DisplayLine() => Console.WriteLine("-----------------------------------------\n");

    }
}
