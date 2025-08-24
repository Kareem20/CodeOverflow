using CodeOverFlow.Entities;
using CodeOverFlow.Services;
using CodeOverFlow.ui;

namespace CodeOverflow.ui
{
    public class ConsoleUI
    {
        private readonly UserService _userService;
        private readonly QuestionService _questionService;
        private readonly AnswerService _answerService;
        private readonly VoteService _voteService;
        private readonly TagService _tagService;
        private readonly Dictionary<int, Action> _mainMenuOptions;
        private readonly Dictionary<int, Action> _userOptions;
        private readonly Dictionary<int, Action<List<Question>>> _questionOptions;
        private readonly Dictionary<int, Action<Question>> _specificQuestionOptions;

        private User? currentUser = null;

        public ConsoleUI()
        {
            _userService = new UserService();
            _questionService = new QuestionService();
            _answerService = new AnswerService();
            _voteService = new VoteService();
            _tagService = new TagService();
            _mainMenuOptions = new Dictionary<int, Action>
            {
                [1] = CreateUser,
                [2] = Login,
                [3] = Logout
            };
            _userOptions = new Dictionary<int, Action>
            {
                [1] = BrowseFeed,
                [2] = AskQuestion,
                [3] = ManagePreferredTags,
                [4] = ViewUserQuestions,
                [5] = Logout
            };
            _questionOptions = new Dictionary<int, Action<List<Question>>>
            {
                [1] = ChooseQustion,
                [2] = VoteQuestion
            };
            _specificQuestionOptions = new Dictionary<int, Action<Question>>
            {
                [1] = ViewSpecificQuestion,
                [2] = EditSpecificQuestion,
                [3] = DeleteQuestion
            };
        }
        public void StartOptions()
        {
            bool exit = false;
            while (!exit)
            {
                int choice = ConsoleHelper.PromptOptions("Welcome to CodeOverflow :)" +
                    "\nSelect an Option:", "Create New User.", "Login.", "Exit.");
                if (_mainMenuOptions.TryGetValue(choice, out var action))
                    action();
                if (choice == 3)
                    exit = true;
            }
        }
        private void UserOptions()
        {
            bool backToMainMenu = false;
            while (!backToMainMenu)
            {
                //TODO: Edit your own questions
                int choice = ConsoleHelper.PromptOptions("User Options: ",
                    "Browse the feed.",
                    "Ask a Question.",
                    "Manage preferred tags.",
                    "View Your own Questions.",
                    "Logout.");
                if (_userOptions.TryGetValue(choice, out var action))
                    action();
                if (choice == 5)
                    break;
            }
        }
        private void CreateUser()
        {
            string username = ConsoleHelper.PromptString("Enter username: ", s => s,
                (s => !_userService.IsUsernameTaken(s), "This username is taken,Choose another one"));
            string email = ConsoleHelper.PromptString("Enter email: ", s => s,
                (s => _userService.IsValidEmail(s), "Invalid email format. Please enter a valid email."),
                (s => !_userService.IsEmailTaken(s), "This email is already registered. Please try another."));
            string password = ConsoleHelper.PromptString("Enter password: ", s => s,
                (s => _userService.isPasswordValid(s), "Invalid password format,Please enter a valid password."));
            var Registered_user = _userService.RegisterUser(username, email, password);
            if (Registered_user == null)
                ConsoleHelper.ShowMsg("Invalid credentails,Please try again.");
            else
                ConsoleHelper.ShowMsg("Registration complete successfully,You can now login.");
            DisplayLine();
        }
        private void Login()
        {
            string identifier = ConsoleHelper.PromptString("Enter username or email: ", s => s);
            string password = ConsoleHelper.PromptString("Enter password: ", s => s);
            currentUser = _userService.Login(identifier, password);
            if (currentUser != null)
            {
                ConsoleHelper.ShowMsg($"Login successful. Welcome,{currentUser.Username}!");
                DisplayLine();
                UserOptions();
            }
            else
            {
                ConsoleHelper.ShowMsg("Login failed. Please try again.");
            }
        }
        private void BrowseFeed()
        {
            List<Tag> userPreferredTags = _userService.GetPreferredTags(currentUser);
            if (userPreferredTags.Count == 0)
            {
                ConsoleHelper.ShowMsg("There is no questions based on your preferred tags." +
                    "\nYou can update your preferred tags.");
                return;
            }
            List<Question> preferredQuestions = _questionService.GetFeed(userPreferredTags);
            //TODO: there is no questions on prefered question
            int cnt = 1;
            foreach (Question question in preferredQuestions)
            {
                ConsoleHelper.ShowMsg($"{cnt++}: {question.Title}?" +
                    $"\n    {question.Body}" +
                    $"\n        Upvotes: {_voteService.GetUpvotes(question.ID)} / Downvotes: {_voteService.GetDownvotes(question.ID)}" +
                    $"\n Author: {_userService.GetById(question.AuthorID).Username}");
                DisplayLine();
            }
            bool back = false;
            while (!back)
            {
                int choice = ConsoleHelper.PromptOptions("Choose an option:",
                    "View a specific question.", "Vote a specific question.", "Return to main options.");
                if (_questionOptions.TryGetValue(choice, out var action))
                    action(preferredQuestions);
                if (choice == 3)
                    back = true;
            }
        }
        private void ChooseQustion(List<Question> preferredQuestions)
        {
            int questionIndex = ConsoleHelper.PromptString(
                "Enter the number of question.",
                int.Parse,
                (i => (0 <= i && i < preferredQuestions.Count), "Please select a correct question number.")
            );
            ViewSpecificQuestion(preferredQuestions[questionIndex]);
            DisplayLine();
        }
        private void VoteQuestion(List<Question> preferredQuestions)
        {
            bool back = false;
            while (!back)
            {
                int choice = ConsoleHelper.PromptOptions("Choose an option:",
                       "Upvote a specific question.", "Downvote a specific question.", "Return to main options.");
                if (choice == 1 || choice == 2)
                {
                    int questionIndex = ConsoleHelper.PromptString(
                        "Enter the number of question.",
                        int.Parse,
                        (i => (0 <= i && i < preferredQuestions.Count), "Please select a correct question number.")
                    );
                    _voteService.VotePost(currentUser
                        , preferredQuestions[questionIndex].ID
                        , null
                        , preferredQuestions[questionIndex].AuthorID
                        , (choice == 1 ? 1 : -1));// 1  for upvote && -1 for downvote
                }
                if (choice == 3)
                    back = true;
            }
        }
        private void ViewUserQuestions()
        {
            List<Question> userQuestions = _questionService.GetUserQuestions(currentUser.UserID);
            if (userQuestions.Count == 0)
            {
                ConsoleHelper.ShowMsg("You don't ask questions yet.");
                return;
            }
            int cnt = 1;
            foreach (Question question in userQuestions)
            {
                ConsoleHelper.ShowMsg($"{cnt++}: {question.Title}?" +
                    $"\n    {question.Body}" +
                    $"\n        Upvotes: {_voteService.GetUpvotes(question.ID)} / Downvotes: {_voteService.GetDownvotes(question.ID)}");
                ConsoleHelper.ShowMsg("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            }
            bool back = false;
            while (!back)
            {
                int choice = ConsoleHelper.PromptOptions("Choose an option:",
                    "To view a specific question.", "Edit a specific question.",
                    "Delete a specific question.", "Return to main options.");
                if (1 <= choice && choice <= 3)
                {
                    int questionIndex = ConsoleHelper.PromptString(
                       "Enter the number of question.",
                       int.Parse,
                       (i => (0 <= i && i < userQuestions.Count), "Please select a correct question number.")
                   );
                    if (_specificQuestionOptions.TryGetValue(questionIndex, out var action))
                        action(userQuestions[questionIndex]);
                }
                if (choice == 4)
                    back = true;
            }
        }
        private void EditSpecificQuestion(Question question) { }
        private void ViewSpecificQuestion(Question question)
        {
            ConsoleHelper.ShowMsg($"- {question.Title}" +
                $"\n  {question.Body}" +
                $"\n        Upvotes: {_voteService.GetUpvotes(question.ID)} / Downvotes: {_voteService.GetDownvotes(question.ID)}" +
                $"\n Author: {_userService.GetById(question.AuthorID).Username}");
            List<Answer> answers = _answerService.GetListOfAnswer(question.ID);
            if (answers.Count == 0)
            {
                ConsoleHelper.ShowMsg("There is no answers for this question");
                return;
            }
            ConsoleHelper.ShowMsg("~~~~~~~~~~~~~~~~~~~~Answers~~~~~~~~~~~~~~~~~~~~");
            int cnt = 1;
            foreach (Answer answer in answers)
            {
                ConsoleHelper.ShowMsg($"{cnt++}: {answer.Body}" +
                    $"\n    Author: {_userService.GetById(answer.AuthorID).Username}" +
                    $"\n        Upvotes: {_voteService.GetUpvotes(answer.ID)} / Downvotes: {_voteService.GetDownvotes(answer.ID)}");
            }
            bool back = false;
            while (!back)
            {
                int choice = ConsoleHelper.PromptOptions("Choose an option",
                    "Upvote a specific answer.", "Downvote a specific answer.", "Return to main options.");
                if (choice == 1 || choice == 2)
                {
                    int answerIndex = ConsoleHelper.PromptString(
                        "Enter the number of answer.",
                        int.Parse,
                        (i => (0 <= i && i < answers.Count), "Please select a correct answer number.")
                    );
                    _voteService.VotePost(currentUser
                               , null
                               , answers[answerIndex].ID
                               , answers[answerIndex].AuthorID
                               , (answerIndex == 1 ? 1 : -1));
                }
                if (choice == 3)
                    back = true;
            }
        }
        private void DeleteQuestion(Question question)
        {
            _questionService.DeleteQuestion(question.ID);
            ConsoleHelper.ShowMsg("Question deleted successfully.");
        }
        private void AskQuestion()
        {
            string questionTitle = ConsoleHelper.PromptString("Enter the main title of the question.", s => s);
            string questionBody = ConsoleHelper.PromptString("Enter the body of the question.",s=>s);
            var tagNames = ConsoleHelper.PromptCommaSperatedStrings("Enter tags (comma-separated, or leave blank): ");
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
