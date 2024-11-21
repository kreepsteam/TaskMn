using System.Collections.Generic;
using System.Windows;
using TaskMn.Model;
using TaskMn.Repositories;

namespace TaskMn
{
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        // Показать историю изменений
        private void ShowChangeHistory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ChangeHistory> history = ChangeHistoryRepository.GetChangeHistory();
                ChangeHistoryListView.ItemsSource = history;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }
    }
}
