using CodeOverFlow.Entities;
using System.Data.SqlClient;

namespace CodeOverFlow.Data
{
    public class QuestionRepository
    {
        private readonly string _connectionString = DbConfig.ConnectionString;
        public void Add(Question question)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = $"INSERT INTO {DbMetaData.QUESTION_TABLE} " +
                    $"({DbMetaData.QUESTION_TITLE_COLUMN}, {DbMetaData.QUESTION_TEXT_COLUMN}, " +
                    $"{DbMetaData.USER_ID_COLUMN}, {DbMetaData.QUESTION_TIMESTAMP_COLUMN}) " +
                    $"VALUES (@Title, @Body, @AuthorId, @Timestamp)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", question.Title);
                    cmd.Parameters.AddWithValue("@Body", question.Body);
                    cmd.Parameters.AddWithValue("@AuthorId", question.AuthorID);
                    cmd.Parameters.AddWithValue("@Timestamp", question.Timestamp);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public Question? GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = $"SELECT * FROM {DbMetaData.QUESTION_TABLE} " +
                    $"WHERE {DbMetaData.QUESTION_ID_COLUMN} = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Question()
                            {
                                ID = (int)reader[DbMetaData.QUESTION_ID_COLUMN],
                                Title = reader[DbMetaData.QUESTION_TITLE_COLUMN].ToString(),
                                Body = reader[DbMetaData.QUESTION_TEXT_COLUMN].ToString(),
                                AuthorID = (int)reader[DbMetaData.USER_ID_COLUMN],
                                Timestamp = (DateTime)reader[DbMetaData.QUESTION_TIMESTAMP_COLUMN]
                            };
                        }
                    }
                }
            }
            return null;
        }
        public List<Question> GetAll()
        {
            List<Question> questions = new List<Question>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = $"SELECT * FROM {DbMetaData.QUESTION_TABLE}";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            questions.Add(new Question
                            {
                                ID = (int)reader[DbMetaData.QUESTION_ID_COLUMN],
                                Title = reader[DbMetaData.QUESTION_TITLE_COLUMN].ToString(),
                                Body = reader[DbMetaData.QUESTION_TEXT_COLUMN].ToString(),
                                AuthorID = (int)reader[DbMetaData.USER_ID_COLUMN],
                                Timestamp = (DateTime)reader[DbMetaData.QUESTION_TIMESTAMP_COLUMN]
                            });
                        }
                    }
                }
            }
            return questions;
        }

        // Get questions filtered by preferred tag IDs.
        public List<Question> GetByPreferredTags(List<int> tagIds)
        {
            List<Question> questions = new List<Question>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // (tag1,tag2,...)
                string tagsInString = "(" + string.Join(", ", tagIds) + ")";
                // Build a simple query that selects questions having at least one of the tags.
                string query = $"SELECT DISTINCT q.* " +
                    $"FROM " +
                    $"{DbMetaData.QUESTION_TABLE} q " +
                    $"INNER JOIN {DbMetaData.QUESTION_TAG_TABLE} qt " +
                    $"ON q.{DbMetaData.QUESTION_ID_COLUMN} = qt.{DbMetaData.QUESTION_ID_COLUMN} " +
                    $"WHERE qt.{DbMetaData.TAG_ID_COLUMN} IN {tagsInString}";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            questions.Add(new Question
                            {
                                ID = (int)reader[DbMetaData.QUESTION_ID_COLUMN],
                                Title = reader[DbMetaData.QUESTION_TITLE_COLUMN].ToString(),
                                Body = reader[DbMetaData.QUESTION_TEXT_COLUMN].ToString(),
                                AuthorID = (int)reader[DbMetaData.USER_ID_COLUMN],
                                Timestamp = (DateTime)reader["Timestamp"]
                            });
                        }
                    }
                }
            }
            return questions;
        }
        public void AddPreferredTag(int questionId,int tagId) {

            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"INSERT INTO {DbMetaData.QUESTION_TAG_TABLE} " +
                    $"({DbMetaData.QUESTION_ID_COLUMN},{DbMetaData.TAG_ID_COLUMN}) " +
                    $"SELECT @questionId,@tagId WHERE NOT EXISTS " +
                    $"(SELECT 1 FROM {DbMetaData.QUESTION_TAG_TABLE} WHERE " +
                    $"{DbMetaData.QUESTION_ID_COLUMN} = @tagId AND {DbMetaData.TAG_ID_COLUMN} = @tagId)";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@questionId", questionId);
                    cmd.Parameters.AddWithValue("@tagId", tagId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Tag> GetPreferredTag(int questionId)
        {
            List<Tag> tags = new List<Tag>();
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"SELECT t.{DbMetaData.TAG_ID_COLUMN},t.{DbMetaData.TAG_NAME_COLUMN} " +
                    $"FROM {DbMetaData.QUESTION_TAG_TABLE} ut JOIN {DbMetaData.TAG_TABLE} t " +
                    $"ON ut.{DbMetaData.TAG_ID_COLUMN} = t.{DbMetaData.TAG_ID_COLUMN} " +
                    $"AND ut.{DbMetaData.QUESTION_ID_COLUMN} = @questionId";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@questionId", questionId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tags.Add(new Tag
                            {
                                TagID = (int)reader[DbMetaData.TAG_ID_COLUMN],
                                TagName = reader[DbMetaData.TAG_NAME_COLUMN].ToString()
                            });
                        }
                    }
                }
            }
            return tags;
        }
    }
}

