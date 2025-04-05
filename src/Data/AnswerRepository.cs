using CodeOverFlow.Entities;
using System.Data.SqlClient;

namespace CodeOverFlow.Data
{
    class AnswerRepository
    {
        public readonly string _connectionString = DbConfig.ConnectionString;

        public void Add(Answer answer)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"INSERT INTO {DbMetaData.ANSWER_TABLE} " +
                    $"({DbMetaData.ANSWER_ID_COLUMN}, " +
                    $"{DbMetaData.USER_ID_COLUMN}, " +
                    $"{DbMetaData.QUESTION_ID_COLUMN}, " +
                    $"{DbMetaData.ANSWER_TIMESTAMP_COLUMN}, " +
                    $"{DbMetaData.ANSWER_ISEDITED_COLUMN}) " +
                    $"VALUES (@Body, @AuthorId, @QuestionId, @Timestamp, @IsEdited)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Body", answer.Body);
                    cmd.Parameters.AddWithValue("@AuthorId", answer.AuthorID);
                    cmd.Parameters.AddWithValue("@QuestionId", answer.QuestionId);
                    cmd.Parameters.AddWithValue("@Timestamp", answer.Timestamp);
                    cmd.Parameters.AddWithValue("@IsEdited", answer.isEdited);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public Answer? GetByUserAndQuestion(int userId, int questionId)
        {
            // To enforce "one answer per question per user"
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"SELECT * FROM {DbMetaData.ANSWER_TABLE} WHERE " +
                    $"{DbMetaData.USER_ID_COLUMN} = @AuthorId " +
                    $"AND {DbMetaData.QUESTION_ID_COLUMN} = @QuestionId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@AuthorId", userId);
                    cmd.Parameters.AddWithValue("@QuestionId", questionId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Answer
                            {
                                ID = (int)reader[DbMetaData.ANSWER_ID_COLUMN],
                                Body = reader[DbMetaData.ANSWER_TEXT_COLUMN].ToString(),
                                AuthorID = (int)reader[DbMetaData.USER_ID_COLUMN],
                                QuestionId = (int)reader[DbMetaData.QUESTION_ID_COLUMN],
                                Timestamp = (DateTime)reader[DbMetaData.ANSWER_TIMESTAMP_COLUMN],
                                isEdited = (bool)reader[DbMetaData.ANSWER_ISEDITED_COLUMN]
                            };
                        }
                    }
                }
            }
            return null;
        }
        public void Edit(int answer_id, string new_answer)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"UPDATE {DbMetaData.ANSWER_TABLE} " +
                    $"SET [{DbMetaData.ANSWER_TEXT_COLUMN}] = {new_answer} " +
                    $"WHERE {answer_id} = @id";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", new_answer);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Answer> GetByQuestion(int questionId)
        {
            List<Answer> answers = new List<Answer>();
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"SELECT * FROM {DbMetaData.ANSWER_TABLE} " +
                    $"WHERE {DbMetaData.QUESTION_ID_COLUMN} = @QuestionId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@QuestionId", questionId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            answers.Add(new Answer
                            {
                                ID = (int)reader[DbMetaData.ANSWER_ID_COLUMN],
                                Body = reader[DbMetaData.ANSWER_TEXT_COLUMN].ToString(),
                                AuthorID = (int)reader[DbMetaData.USER_ID_COLUMN],
                                QuestionId = (int)reader[DbMetaData.QUESTION_ID_COLUMN],
                                Timestamp = (DateTime)reader[DbMetaData.ANSWER_TIMESTAMP_COLUMN],
                                isEdited = (bool)reader[DbMetaData.ANSWER_ISEDITED_COLUMN]
                            });
                        }
                    }
                }
            }
            return answers;
        }
    }
}
