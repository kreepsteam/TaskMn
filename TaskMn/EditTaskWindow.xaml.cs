using System.Windows;
using TaskMn.Models;
using TaskMn.Repositories;

namespace TaskMn
{
    public partial class EditTaskWindow : Window
    {
        private TaskModel _task;

        public EditTaskWindow(TaskModel task)
        {
            InitializeComponent();
            _task = task;
            TitleTextBox.Text = _task.Title;
            DescriptionTextBox.Text = _task.Description;
            SetPriority(_task.Priority);
            StatusCheckBox.IsChecked = _task.Status;

            LoadTags();
            LoadTaskTags(_task.Id);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) || string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните как название, так и описание");
                return;
            }

            _task.Title = TitleTextBox.Text;
            _task.Description = DescriptionTextBox.Text;
            _task.Priority = GetSelectedPriority();
            _task.Status = StatusCheckBox.IsChecked ?? _task.Status;
            _task.CreatedAt = DateTime.Now;

            if (_task.Status == true)
            {
                _task.CompletedAt = DateTime.Now;
            }

            TaskRepository.UpdateTask(_task);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddTagButton_Click(object sender, RoutedEventArgs e)
        {
            var addTagWindow = new AddTagWindow();
            addTagWindow.ShowDialog();
        }

        private void AddTagToTaskButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrRemoveTags(TagsListBox.SelectedItems.Cast<string>().ToList(), addTags: true);
        }

        private void RemoveTagFromTaskButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrRemoveTags(TaskTagsListBox.SelectedItems.Cast<string>().ToList(), addTags: false);
        }

        private void AddOrRemoveTags(List<string> selectedTags, bool addTags)
        {
            if (selectedTags.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите хотя бы один тег.");
                return;
            }

            foreach (var tagName in selectedTags)
            {
                var tag = TagRepository.GetTagByName(tagName);
                if (tag != null)
                {
                    if (addTags && !TagRepository.IsTagAssignedToTask(_task.Id, tag.Id))
                    {
                        TagRepository.AddTagToTask(_task.Id, tag.Name);
                    }
                    else if (!addTags && TagRepository.IsTagAssignedToTask(_task.Id, tag.Id))
                    {
                        TagRepository.RemoveTagFromTask(_task.Id, tag.Id);
                    }
                }
                else
                {
                    MessageBox.Show($"Тег '{tagName}' не существует в базе.");
                    return;
                }
            }

            LoadTaskTags(_task.Id);
        }

        private void LoadTags()
        {
            var tags = TagRepository.GetAllTags();
            TagsListBox.Items.Clear();

            foreach (var tag in tags)
            {
                TagsListBox.Items.Add(tag);
            }
        }

        private void LoadTaskTags(int taskId)
        {
            var tags = TagRepository.GetTagsForTask(taskId);
            TaskTagsListBox.Items.Clear();

            foreach (var tag in tags)
            {
                TaskTagsListBox.Items.Add(tag);
            }
        }

        private void SetPriority(string priority)
        {
            if (priority == "High")
            {
                HighPriorityRadioButton.IsChecked = true;
            }
            else if (priority == "Medium")
            {
                MediumPriorityRadioButton.IsChecked = true;
            }
            else if (priority == "Low")
            {
                LowPriorityRadioButton.IsChecked = true;
            }
        }

        private string GetSelectedPriority()
        {
            if (HighPriorityRadioButton.IsChecked == true)
            {
                return "High";
            }
            else if (MediumPriorityRadioButton.IsChecked == true)
            {
                return "Medium";
            }
            else if (LowPriorityRadioButton.IsChecked == true)
            {
                return "Low";
            }
            return string.Empty;
        }
    }
}
