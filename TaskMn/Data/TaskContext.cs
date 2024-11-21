using System.Data.SQLite;
using System.IO;

namespace TaskMn.Data
{
    public static class TaskContext
    {
        public static readonly string ConnectionString = "Data Source=TaskManager.db;Version=3;";

        // Инициализация базы данных
        public static void InitializeDatabase()
        {
            if (!File.Exists("TaskManager.db"))
            {
                SQLiteConnection.CreateFile("TaskManager.db");
            }

            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            string createTagsTable = @"
                CREATE TABLE IF NOT EXISTS Tags (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE
                );";

            string createTaskTagsTable = @"
                CREATE TABLE IF NOT EXISTS TaskTags (
                    TaskId INTEGER NOT NULL,
                    TagId INTEGER NOT NULL,
                    PRIMARY KEY (TaskId, TagId),
                    FOREIGN KEY (TagId) REFERENCES Tags(Id)
                );";

            using var createTagsCommand = new SQLiteCommand(createTagsTable, connection);
            createTagsCommand.ExecuteNonQuery();

            using var createTaskTagsCommand = new SQLiteCommand(createTaskTagsTable, connection);
            createTaskTagsCommand.ExecuteNonQuery();
        }

        public static void SeedData()
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            string insertSampleTags = @"
        INSERT OR IGNORE INTO Tags (Name) VALUES
        ('Important'),
        ('Urgent'),
        ('Personal'),
        ('Work');";

            using var command = new SQLiteCommand(insertSampleTags, connection);
            command.ExecuteNonQuery();
        }
    }
}
