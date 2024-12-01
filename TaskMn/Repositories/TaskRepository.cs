using System.Data.SQLite;
using TaskMn.Data;
using TaskMn.Models;
using TaskMn.Repositories;

namespace TaskMn.Repositories
{
    public static class TaskRepository
    {
        public static void AddTask(TaskModel task)
        {
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();
            string query = "INSERT INTO Tasks (UserId, Title, Description, Priority, Status, CreatedAt, CompletedAt) " +
                           "VALUES (@UserId, @Title, @Description, @Priority, @Status, @CreatedAt, @CompletedAt)";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", task.UserId);
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@Priority", task.Priority);
            command.Parameters.AddWithValue("@Status", task.Status);
            command.Parameters.AddWithValue("@CreatedAt", task.CreatedAt);
            command.Parameters.AddWithValue("@CompletedAt", task.CompletedAt ?? (object)DBNull.Value);
            command.ExecuteNonQuery();
            int taskId = (int)connection.LastInsertRowId;

            ChangeHistoryRepository.AddChangeHistory(taskId, "Добавление");
        }

        public static void DeleteTask(int taskId)
        {
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();
            string query = "DELETE FROM Tasks WHERE Id = @Id";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", taskId);
            command.ExecuteNonQuery();

            ChangeHistoryRepository.AddChangeHistory(taskId, "Удаление");
        }

        public static List<TaskModel> GetTasks()
        {
            var tasks = new List<TaskModel>();
            using (var connection = new SQLiteConnection(TaskContext.ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Tasks";
                using var command = new SQLiteCommand(query, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tasks.Add(new TaskModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Title = reader["Title"].ToString(),
                        Description = reader["Description"].ToString(),
                        Priority = reader["Priority"].ToString(),
                        Status = Convert.ToBoolean(reader["Status"]),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        CompletedAt = reader["CompletedAt"] as DateTime?
                    });
                }
            }
            return tasks;
        }

        public static void UpdateTask(TaskModel task)
        {
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();

            string query = @"
        UPDATE Tasks
        SET Title = @Title,
            Description = @Description,
            Priority = @Priority,
            Status = @Status,
            CreatedAt = @CreatedAt,
            CompletedAt = @CompletedAt
        WHERE Id = @Id";

            string query = "UPDATE Tasks SET Title = @Title, Description = @Description, Priority = @Priority, Status = @Status, " +
                           "CreatedAt = @CreatedAt, CompletedAt = @CompletedAt WHERE Id = @Id";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", task.Id);
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@Priority", task.Priority);
            command.Parameters.AddWithValue("@Status", task.Status);
            command.Parameters.AddWithValue("@CreatedAt", task.CreatedAt);
            command.Parameters.AddWithValue("@CompletedAt", task.CompletedAt ?? (object)DBNull.Value);

            command.ExecuteNonQuery();
        }

            command.Parameters.AddWithValue("@CompletedAt", task.CompletedAt);
            command.ExecuteNonQuery();

            ChangeHistoryRepository.AddChangeHistory(task.Id, "Редактирование");
        }

        public static List<TaskModel> GetTasksByUserId(int userId)
        {
            var tasks = new List<TaskModel>();
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();

            string query = "SELECT * FROM Tasks WHERE UserId = @UserId";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tasks.Add(new TaskModel
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    UserId = Convert.ToInt32(reader["UserId"]),
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    Priority = reader["Priority"].ToString(),
                    Status = Convert.ToBoolean(reader["Status"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                    CompletedAt = reader["CompletedAt"] as DateTime?
                });
            }

            return tasks;
        }

        public static List<TaskModel> GetTasksByTag(string tagName)
        {
            var tasks = new List<TaskModel>();
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();

            string query = @"
        SELECT t.* 
        FROM Tasks t
        JOIN TaskTags tt ON t.Id = tt.TaskId
        JOIN Tags tg ON tg.Id = tt.TagId
        WHERE tg.Name = @TagName";

            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@TagName", tagName);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tasks.Add(new TaskModel
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    UserId = Convert.ToInt32(reader["UserId"]),
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    Priority = reader["Priority"].ToString(),
                    Status = Convert.ToBoolean(reader["Status"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                    CompletedAt = reader["CompletedAt"] as DateTime?
                });
            }

            return tasks;
        }
    }
}
