using CodeOverFlow.Data;
using CodeOverFlow.Entities;

namespace CodeOverFlow.Services
{
    public class VoteService
    {
        private readonly VoteRepository _voteRepository = new VoteRepository();

        // @questionId null if the vote for answer.
        // @answerId null i the vote for question.
        public void VotePost(User voter, int? questionId, int? answerId, int authorId, int new_vote)
        {
            if (voter.UserID == authorId)
            {
                Console.WriteLine("You can't vote your own post.");
                return;
            }
            if (questionId == null && answerId == null)
            {
                Console.WriteLine("Vote must be linked to a question or an answer");
                return;
            }
            int postId = (int)(questionId == null ? answerId : questionId);
            Vote existingVote = _voteRepository.GetByUserAndPost(voter.UserID, postId);
            if (existingVote != null)
            {
                if (existingVote.VoteType == new_vote)
                {
                    Console.WriteLine("You have already voted in this direction.");
                    return;
                }
                else
                {
                    // Switch the vote
                    existingVote.VoteType = new_vote;
                    existingVote.Timestamp = DateTime.Now;
                    _voteRepository.Update(existingVote);
                    Console.WriteLine("Your vote has been updated.");
                }
            }
            else
            {
                Vote vote = new Vote
                {
                    VoterID = voter.UserID,
                    QuestionID = questionId,
                    AnswerID = answerId,
                    VoteType = new_vote,
                    Timestamp = DateTime.Now
                };
                _voteRepository.Add(vote);
                Console.WriteLine("Your vote has been recorded.");
            }
        }
    }
}
