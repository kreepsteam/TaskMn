using System.Collections.Generic;
using System.Windows;
using TaskMn.Repositories;

namespace TaskMn
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Добавить тег
        private void AddTag_Click(object sender, RoutedEventArgs e)
        {
            string tagName = TagNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(tagName))
            {
                MessageBox.Show("Введите название тега.", "Ошибка");
                return;
            }

            try
            {
                TagRepository.AddTag(tagName);
                MessageBox.Show($"Тег '{tagName}' успешно добавлен.", "Успех");
                TagNameTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        // Показать теги для задачи
        private void ShowTaskTags_Click(object sender, RoutedEventArgs e)
        {
            int taskId = 1; // ID задачи для примера
            try
            {
                List<string> tags = TagRepository.GetTagsForTask(taskId);
                TagsListBox.ItemsSource = tags;
                if (tags.Count == 0)
                {
                    MessageBox.Show("У задачи нет привязанных тегов.", "Информация");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }
    }
}
