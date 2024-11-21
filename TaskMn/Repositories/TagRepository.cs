using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMn.Data;

namespace TaskMn.Repositories
{
    class TagRepository
    {
        public static TagModel? GetTagByName(string name)
        {
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();

            string query = "SELECT * FROM Tags WHERE Name = @Name";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@Name", name);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new TagModel
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()!
                };
            }

            return null;
        }

        public static List<string> GetAllTags()
        {
            var tags = new List<string>();

            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();

            string query = "SELECT Name FROM Tags";
            using var command = new SQLiteCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tags.Add(reader["Name"].ToString()!);
            }

            return tags;
        }

        public static List<string> GetTagsForTask(int taskId)
        {
            var tags = new List<string>();

            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();

            string query = @"
        SELECT t.Name
        FROM Tags t
        INNER JOIN TaskTags tt ON t.Id = tt.TagId
        WHERE tt.TaskId = @TaskId";

            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@TaskId", taskId);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tags.Add(reader["Name"].ToString()!);
            }

            return tags;
        }

        public static void AddTagToTask(int taskId, string tagName)
        {
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();

            string query = @"
        INSERT INTO TaskTags (TaskId, TagId)
        SELECT @TaskId, t.Id
        FROM Tags t
        WHERE t.Name = @TagName";

            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@TaskId", taskId);
            command.Parameters.AddWithValue("@TagName", tagName);

            command.ExecuteNonQuery();
        }

        public static void RemoveTagFromTask(int taskId, int tagId)
        {
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();

            string query = "DELETE FROM TaskTags WHERE TaskId = @TaskId AND TagId = @TagId";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@TaskId", taskId);
            command.Parameters.AddWithValue("@TagId", tagId);

            command.ExecuteNonQuery();
        }

        public static bool IsTagAssignedToTask(int taskId, int tagId)
        {
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();

            string query = "SELECT COUNT(1) FROM TaskTags WHERE TaskId = @TaskId AND TagId = @TagId";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@TaskId", taskId);
            command.Parameters.AddWithValue("@TagId", tagId);

            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }

    }
}
