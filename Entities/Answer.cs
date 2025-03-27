namespace CodeOverFlow.Entities
{
    public class Answer : Post
    {
        public int QuestionId { get; set; }
        public bool isEdited { get; set; } = false;

        public void Edit(string newAnswer)
        {
            isEdited = true;
            Body = newAnswer;
        }
    }
}