namespace CodeOverFlow.Entities
{
    public class Vote
    {
        public int VoteID { get; set; }
        public int VoterID { get; set; }
        public int? QuestionID { get; set; }    // Null if it's an answer vote
        public int? AnswerID { get; set; }      // Nullable if it's a question vote
        public int VoteType { get; set; }       //  +1 for upvote,-1 for downvote.
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}