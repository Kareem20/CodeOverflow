using CodeOverFlow.Entities;
using System.Data.SqlClient;

namespace CodeOverFlow.Data
{
    public class VoteRepository
    {
        private readonly string _connectionString = DbConfig.ConnectionString;

        public void Add(Vote vote)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"INSERT INTO {DbMetaData.VOTE_TABLE} " +
                    $"({DbMetaData.USER_ID_COLUMN}, " +
                    $"{DbMetaData.QUESTION_ID_COLUMN}, " +
                    $"{DbMetaData.ANSWER_ID_COLUMN}, " +
                    $"{DbMetaData.VOTE_TYPE_COLUMN}, " +
                    $"{DbMetaData.VOTE_TIMESTAMP_COLUMN}) " +
                    $"VALUES (@UserId, @QuestionId, @AnswerId, @VoteType, @Timestamp)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", vote.VoterID);
                    cmd.Parameters.AddWithValue("@QuestionId", vote.QuestionID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AnswerId", vote.AnswerID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@VoteType", vote.VoteType);
                    cmd.Parameters.AddWithValue("@Timestamp", vote.Timestamp);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Update(Vote vote)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"UPDATE {DbMetaData.VOTE_TABLE} SET " +
                    $"{DbMetaData.VOTE_TYPE_COLUMN} = @VoteType, " +
                    $"{DbMetaData.VOTE_TIMESTAMP_COLUMN} = @Timestamp " +
                    $"WHERE {DbMetaData.VOTE_ID_COLUMN} = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@VoteType", vote.VoteType);
                    cmd.Parameters.AddWithValue("@Timestamp", vote.Timestamp);
                    cmd.Parameters.AddWithValue("@Id", vote.VoteID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public Vote? GetByUserAndPost(int userId, int postId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"SELECT * FROM {DbMetaData.VOTE_TABLE} " +
                    $"WHERE {DbMetaData.USER_ID_COLUMN} = @UserId " +
                    $"AND ({DbMetaData.QUESTION_ID_COLUMN} = @PostId OR " +
                    $"{DbMetaData.ANSWER_ID_COLUMN} = @PostId)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@PostId", postId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Vote
                            {
                                VoteID = (int)reader[DbMetaData.VOTE_ID_COLUMN],
                                VoterID = (int)reader[DbMetaData.USER_ID_COLUMN],
                                QuestionID = reader[DbMetaData.QUESTION_ID_COLUMN] != DBNull.Value ? (int?)reader[DbMetaData.QUESTION_ID_COLUMN] : null,
                                AnswerID = reader[DbMetaData.ANSWER_ID_COLUMN] != DBNull.Value ? (int?)reader[DbMetaData.ANSWER_ID_COLUMN] : null,
                                VoteType = (int)reader[DbMetaData.VOTE_TYPE_COLUMN],
                                Timestamp = (DateTime)reader[DbMetaData.VOTE_TIMESTAMP_COLUMN]
                            };
                        }
                    }
                }
            }
            return null;
        }
        public int GetNumberOfVotes(int postId, int voteType)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"SELECT COUNT(*) AS vote_count " +
                    $"FROM {DbMetaData.VOTE_TABLE} " +
                    $"WHERE ({DbMetaData.QUESTION_ID_COLUMN} = @postId " +
                    $"OR {DbMetaData.ANSWER_ID_COLUMN} = @postId)" +
                    $"AND {DbMetaData.VOTE_TYPE_COLUMN} = @voteType";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@postId", postId);
                    cmd.Parameters.AddWithValue("@voteType", voteType);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (int)reader["vote_count"];
                        }
                    }
                }
            }
            return -1;
        }

    }
}
