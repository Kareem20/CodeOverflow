﻿using System.Data.SqlClient;

namespace CodeOverFlow.Data
{
    public class TagRepository
    {
        private readonly string _connectionString = DbConfig.ConnectionString;
        public int AddTag(string tagName)
        {
            using(var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"INSERT INTO {DbMetaData.TAG_TABLE} ({DbMetaData.TAG_NAME_COLUMN})" +
                    $" OUTPUT INSERTED.{DbMetaData.TAG_ID_COLUMN} VALUES ('@tagName')";
                using(var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@tagName", tagName);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }
        public int? GetIdByName(string tagName)
        {
            using(var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = $"SELECT {DbMetaData.TAG_ID_COLUMN} FROM {DbMetaData.TAG_TABLE} " +
                    $"WHERE LOWER({DbMetaData.TAG_NAME_COLUMN}) = @TagName";
                using(var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TagName", tagName.ToLower());
                    var result = cmd.ExecuteScalar();
                    return result != null ? (int?)result : null;
                }
            }
        }
    }
}
