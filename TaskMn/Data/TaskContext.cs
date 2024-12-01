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
        }
        /*public const string ConnectionString = "Data Source=tasks.db;Version=3;";
        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        public static void InitializeDatabase()
        {
            using var connection = GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
        CREATE TABLE IF NOT EXISTS Users (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Username TEXT NOT NULL UNIQUE,
            Password TEXT NOT NULL
        );
        CREATE TABLE IF NOT EXISTS Tasks (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId INTEGER NOT NULL,
            Title TEXT NOT NULL,
            Description TEXT,
            Priority TEXT CHECK(Priority IN ('High', 'Medium', 'Low')),
            Status BOOLEAN NOT NULL DEFAULT 0,
            CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
            CompletedAt DATETIME,
            FOREIGN KEY(UserId) REFERENCES Users(Id)
        );
        CREATE TABLE IF NOT EXISTS Tags (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL UNIQUE
        );
        CREATE TABLE IF NOT EXISTS TaskTags (
            TaskId INTEGER NOT NULL,
            TagId INTEGER NOT NULL,
            FOREIGN KEY(TaskId) REFERENCES Tasks(Id),
            FOREIGN KEY(TagId) REFERENCES Tags(Id),
            PRIMARY KEY(TaskId, TagId)
        );
        CREATE TABLE IF NOT EXISTS ChangeHistory (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            TaskId INTEGER NOT NULL,
            ChangeType TEXT NOT NULL,
            ChangeTime DATETIME DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY(TaskId) REFERENCES Tasks(Id)
        );";
            command.ExecuteNonQuery();
        }*/
    }
}
