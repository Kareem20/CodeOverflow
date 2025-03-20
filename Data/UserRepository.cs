using CodeOverFlow.Entities;
using System.Data.SqlClient;
namespace CodeOverFlow.Data
{
    public class UserRepository
    {
        private readonly string _connectionString = DbConfig.ConnectionString;

        public void Add(User user)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = $"INSERT INTO {DbMetaData.USER_TABLE} " +
                    $"({DbMetaData.USER_USERNAME_COLUMN}," +
                    $"{DbMetaData.USER_EMAIL_COLUMN}," +
                    $"{DbMetaData.USER_PASSWORD_COLUMN}) VALUES (@Username, @Email, @PasswordHash)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHashed);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public User? getByID(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"SELECT * FROM {DbMetaData.USER_TABLE} " +
                    $"WHERE {DbMetaData.USER_ID_COLUMN} = @ID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = (int)reader[DbMetaData.USER_ID_COLUMN],
                                Username = reader[DbMetaData.USER_USERNAME_COLUMN].ToString(),
                                Email = reader[DbMetaData.USER_EMAIL_COLUMN].ToString(),
                                PasswordHashed = reader[DbMetaData.USER_PASSWORD_COLUMN].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }
        public List<User> getAll()
        {
            List<User> users = new List<User>();
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"SELECT * FROM {DbMetaData.USER_TABLE}";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserID = (int)reader[DbMetaData.USER_ID_COLUMN],
                                Username = reader[DbMetaData.USER_USERNAME_COLUMN].ToString(),
                                Email = reader[DbMetaData.USER_EMAIL_COLUMN].ToString(),
                                PasswordHashed = reader[DbMetaData.USER_PASSWORD_COLUMN].ToString()
                            });
                        }
                    }
                }
            }
            return users;
        }
        public void AddPreferredTag(int userId, int tagId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"INSERT INTO {DbMetaData.USER_TAG_TABLE} " +
                    $"({DbMetaData.USER_ID_COLUMN},{DbMetaData.TAG_ID_COLUMN}) " +
                    $"SELECT @userId,@tagId WHERE NOT EXISTS " +
                    $"(SELECT 1 FROM {DbMetaData.USER_TAG_TABLE} WHERE " +
                    $"{DbMetaData.USER_ID_COLUMN} = @userId AND {DbMetaData.TAG_ID_COLUMN} = @tagId)";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@tagId", tagId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Tag> GetUserPreferredTag(int userId)
        {
            List<Tag> tags = new List<Tag>();
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"SELECT t.{DbMetaData.TAG_ID_COLUMN},t.{DbMetaData.TAG_NAME_COLUMN} " +
                    $"FROM {DbMetaData.USER_TAG_TABLE} ut JOIN {DbMetaData.TAG_TABLE} t " +
                    $"ON ut.{DbMetaData.TAG_ID_COLUMN} = t.{DbMetaData.TAG_ID_COLUMN} " +
                    $"AND ut.{DbMetaData.USER_ID_COLUMN} = @userId";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
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
