using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TaskMn
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        // Обработчик кнопки для добавления информации
        private void AddInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string newInfo = InfoTextBox.Text; // Получение текста из TextBox

                // Проверка, что поле не пустое
                if (string.IsNullOrWhiteSpace(newInfo))
                {
                    MessageBox.Show("Введите текст!", "Ошибка");
                    return;
                }

                // Подключение к БД
                using (SqlConnection connection = new SqlConnection("YourConnectionStringHere"))
                {
                    connection.Open();
                    // Запрос для добавления данных
                    string query = "INSERT INTO YourTable (ColumnName) VALUES (@NewInfo)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NewInfo", newInfo);
                        int rowsAdded = command.ExecuteNonQuery();
                        MessageBox.Show($"Добавлено строк: {rowsAdded}", "Успех");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }
    }
}
