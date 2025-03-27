using CodeOverFlow.Data;
using CodeOverFlow.Entities;

namespace CodeOverFlow.Services
{
    public class AnswerService
    {
        private readonly AnswerRepository _answerRepository = new AnswerRepository();

        public void PostAnswer(User user, Question question, string body)
        {
            Answer answer_existing = _answerRepository.GetByUserAndQuestion(user.UserID, question.ID);
            // User has answered the question before.
            if (answer_existing != null)
            {
                Console.WriteLine();
                return;
            }
            Answer answer = new Answer
            {
                Body = body,
                AuthorID = user.UserID,
                QuestionId = question.ID,
                Timestamp = DateTime.Now,
                isEdited = false
            };
            _answerRepository.Add(answer);
            Console.WriteLine("Answer posted successfully");
        }
        public void EditAnswer(User user, Answer answer, string new_answer)
        {
            // Edit an answer (only if it belongs to the user).
            if (answer.AuthorID != user.UserID)
            {
                Console.WriteLine("You can only edit your own answers.");
                return;
            }
            answer.Edit(new_answer);
            _answerRepository.Edit(answer.ID, new_answer);
            Console.WriteLine("Answer is edited sucessfully");
        }
    }
}
