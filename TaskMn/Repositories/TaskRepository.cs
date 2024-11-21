using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMn.Data;

namespace TaskMn.Repositories
{
    class TaskRepository
    {
        public static void UpdateTaskName(int taskId, string newName)
        {
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();

            string query = "UPDATE Tasks SET Name = @Name WHERE Id = @TaskId";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@Name", newName);
            command.Parameters.AddWithValue("@TaskId", taskId);

            command.ExecuteNonQuery();

            // Записываем в историю
            ChangeHistoryRepository.AddChangeHistory(taskId, "Изменено название задачи");
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

    }
}
