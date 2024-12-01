using System.Data.SQLite;
using TaskMn.Data;
using TaskMn.Models;

namespace TaskMn.Repositories
{
    public class TagRepository
    {
        public static void AddTag(string tagName)
        {
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();

            var checkTagCommand = connection.CreateCommand();
            checkTagCommand.CommandText = "SELECT Id FROM Tags WHERE Name = @Name";
            checkTagCommand.Parameters.AddWithValue("@Name", tagName);

            int? tagId = null;
            using (var reader = checkTagCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    tagId = reader.GetInt32(0);
                }
            }

            if (!tagId.HasValue)
            {
                var insertTagCommand = connection.CreateCommand();
                insertTagCommand.CommandText = "INSERT INTO Tags (Name) VALUES (@Name)";
                insertTagCommand.Parameters.AddWithValue("@Name", tagName);
                insertTagCommand.ExecuteNonQuery();

                tagId = (int)connection.LastInsertRowId;
            }
        }

        public static Tag GetTagByName(string tagName)
        {
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();

            string query = "SELECT * FROM Tags WHERE Name = @Name";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Name", tagName);
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
                return new Tag
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString();
                };
            }
            return null;
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
            var getTagIdCommand = connection.CreateCommand();
            getTagIdCommand.CommandText = "SELECT Id FROM Tags WHERE Name = @Name";
            getTagIdCommand.Parameters.AddWithValue("@Name", tagName);

            int tagId = -1;
            using (var reader = getTagIdCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    tagId = reader.GetInt32(0);
                }
                else
                {
                    AddTag(tagName);
                    tagId = (int)connection.LastInsertRowId;
                }
            }

            var insertTaskTagCommand = connection.CreateCommand();
            insertTaskTagCommand.CommandText = "INSERT INTO TaskTags (TaskId, TagId) VALUES (@TaskId, @TagId)";
            insertTaskTagCommand.Parameters.AddWithValue("@TaskId", taskId);
            insertTaskTagCommand.Parameters.AddWithValue("@TagId", tagId);
            insertTaskTagCommand.ExecuteNonQuery();
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

        public static List<string> GetTagsForTask(int taskId)
        {
            var tags = new List<string>();
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
        SELECT t.Name FROM Tags t
        JOIN TaskTags tt ON t.Id = tt.TagId
        WHERE tt.TaskId = @TaskId";
            command.Parameters.AddWithValue("@TaskId", taskId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tags.Add(reader["Name"].ToString());
            }
            return tags;
        }

        public static List<string> GetAllTags()
        {
            var tags = new List<string>();
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Name FROM Tags";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tags.Add(reader["Name"].ToString());
            }
            return tags;
        }

        public static bool IsTagAssignedToTask(int taskId, int tagId)
        {
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();
            string query = "SELECT COUNT(*) FROM TaskTags WHERE TaskId = @TaskId AND TagId = @TagId";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@TaskId", taskId);
            command.Parameters.AddWithValue("@TagId", tagId);

            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
            var count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
    }
}
