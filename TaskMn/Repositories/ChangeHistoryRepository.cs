using System.Data.SQLite;
using TaskMn.Data;
using TaskMn.Models;

namespace TaskMn.Repositories
{
    public static class ChangeHistoryRepository
    {
        public static List<ChangeHistory> GetChangeHistory()
        {
            var changeHistory = new List<ChangeHistory>();

            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();
            string query = "SELECT * FROM ChangeHistory ORDER BY ChangeTime DESC";
            using var command = new SQLiteCommand(query, connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                changeHistory.Add(new ChangeHistory
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    TaskId = Convert.ToInt32(reader["TaskId"]),
                    ChangeType = reader["ChangeType"].ToString(),
                    ChangeTime = Convert.ToDateTime(reader["ChangeTime"])
                });
            }

            return changeHistory;
        }

        public static void AddChangeHistory(int taskId, string changeType)
        {
            using var connection = new SQLiteConnection(TaskContext.ConnectionString);
            connection.Open();
            string query = "INSERT INTO ChangeHistory (TaskId, ChangeType) VALUES (@TaskId, @ChangeType)";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@TaskId", taskId);
            command.Parameters.AddWithValue("@ChangeType", changeType);
            command.ExecuteNonQuery();
        }
    }
}
