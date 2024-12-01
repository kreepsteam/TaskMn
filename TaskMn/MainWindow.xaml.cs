using System.Collections.Generic;
using System.Windows;
﻿using Newtonsoft.Json;
using System.IO;
using System.Windows;
using TaskMn.Data;
using TaskMn.Models;
using TaskMn.Repositories;

namespace TaskMn
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TaskContext.InitializeDatabase();
            LoadTasks();
            LoadChangeHistory();
        }

        public static void ExportTasksToJson(List<TaskModel> tasks)
        {
            try
            {
                string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);

                string filePath = "tasks.json";

                File.WriteAllText(filePath, json);

                MessageBox.Show("Задачи успешно экспортированы в файл tasks.json", "Экспорт завершен", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при экспорте задач: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportTasksToJson_Click(object sender, RoutedEventArgs e)
        {
            var tasks = TaskRepository.GetTasks();
            ExportTasksToJson(tasks);
        }

        private void LoadTasks()
        {
            var allTasks = TaskRepository.GetTasks();
            AllTasksGrid.ItemsSource = allTasks;

            var highPriorityTasks = allTasks.Where(t => t.Priority == "High").ToList();
            HighPriorityTasksGrid.ItemsSource = highPriorityTasks;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new AddTaskWindow();
            addTaskWindow.ShowDialog();
            LoadChangeHistory();
            LoadTasks();
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = GetSelectedTask();
            if (selectedTask != null)
            {
                var editTaskWindow = new EditTaskWindow(selectedTask);
                editTaskWindow.ShowDialog();
                LoadChangeHistory();

                LoadTasks();
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = GetSelectedTask();
            if (selectedTask != null)
            {
                TaskRepository.DeleteTask(selectedTask.Id);
                LoadTasks();
                LoadChangeHistory();
            }
        }

        private void LoadChangeHistory()
        {
            var history = ChangeHistoryRepository.GetChangeHistory();
            ChangeHistoryGrid.ItemsSource = history;
            ChangeHistoryGrid.Items.Refresh();
        }

        private TaskModel GetSelectedTask()
        {
            if (AllTasksGrid.IsVisible)
            {
                return (TaskModel)AllTasksGrid.SelectedItem;
            }
            else if (HighPriorityTasksGrid.IsVisible)
            {
                return (TaskModel)HighPriorityTasksGrid.SelectedItem;
            }

            return null;
        }

        private void FilterTasksByTag_Click(object sender, RoutedEventArgs e)
        {
            string tagName = TagFilterTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(tagName))
            {
                var filteredTasks = TaskRepository.GetTasksByTag(tagName);
                AllTasksGrid.ItemsSource = filteredTasks;
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите имя тега.");
            }
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
