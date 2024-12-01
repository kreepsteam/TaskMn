using System.Windows;
using TaskMn.Models;
using TaskMn.Repositories;

namespace TaskMn
{
    public partial class AddTaskWindow : Window
    {
        public AddTaskWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) || string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните как название, так и описание");
                return;
            }

            string priority = string.Empty;

            if (HighPriorityRadioButton.IsChecked == true)
            {
                priority = "High";
            }
            else if (MediumPriorityRadioButton.IsChecked == true)
            {
                priority = "Medium";
            }
            else if (LowPriorityRadioButton.IsChecked == true)
            {
                priority = "Low";
            }

            if (string.IsNullOrEmpty(priority))
            {
                MessageBox.Show("Пожалуйста, выберите приоритет");
                return;
            }

            var newTask = new TaskModel
            {
                UserId = 1,
                Title = TitleTextBox.Text,
                Description = DescriptionTextBox.Text,
                Priority = priority,
                Status = false,
            };

            TaskRepository.AddTask(newTask);

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
