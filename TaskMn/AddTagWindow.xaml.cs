using System.Windows;
using TaskMn.Repositories;

namespace TaskMn
{
    public partial class AddTagWindow : Window
    {
        public AddTagWindow()
        {
            InitializeComponent();
        }

        private void SaveTagButton_Click(object sender, RoutedEventArgs e)
        {
            var tagName = TagTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(tagName))
            {
                MessageBox.Show("Пожалуйста, введите название тега");
                return;
            }

            TagRepository.AddTag(tagName);

            MessageBox.Show($"Тег '{tagName}' успешно добавлен");
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
