using System.Configuration;
using System.Data;
using System.Windows;
using TaskMn.Data;

namespace TaskMn
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Инициализация базы данных
            TaskContext.InitializeDatabase();

            // Заполнение тестовыми данными
            TaskContext.SeedData();
        }
    }
}
