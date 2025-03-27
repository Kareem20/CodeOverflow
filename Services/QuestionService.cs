using CodeOverFlow.Data;
using CodeOverFlow.Entities;

namespace CodeOverFlow.Services
{
    public class QuestionService
    {
        private readonly QuestionRepository _questionRepository = new QuestionRepository();

        public void AskQuestion(User user, string title, string body, List<Tag> tags)
        {
            if (user == null)
            {
                //TODO: not loggin
                return;
            }
            Question new_question = new Question
            {
                Title = title,
                Body = body,
                AuthorID = user.UserID,
                Timestamp = DateTime.Now,
                Tags = tags
            };
            _questionRepository.Add(new_question);
            Console.WriteLine("Question posted successfully.");
        }
        // Get the questions feed based on the user's preferred tags.
        public List<Question> GetFeed(User user) => _questionRepository.GetByPreferredTags(user.PreferredTags);

        public Question GetQuestionId(int questionId) => _questionRepository.GetById(questionId);
    }
}
