namespace CodeOverFlow.Entities
{
    public abstract class Post
    {
        public int ID { get; set; }
        public int AuthorID { get; set; }
        public string Body { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }

    }
}