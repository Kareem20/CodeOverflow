namespace CodeOverFlow.Entities
{
    public class Question : Post
    {
        public string Title { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<Answer> Answers { get; set; } = new List<Answer>();

        public void AddTag(Tag tag)
        {
            if (!Tags.Exists(t => t.TagID == tag.TagID))
            {
                Tags.Add(tag);
            }
        }
        public void AddAnswer(Answer answer)
        {
            Answers.Add(answer);
        }

    }

}