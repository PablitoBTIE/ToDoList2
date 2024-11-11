using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ToDoListApp
{
    public partial class MainWindow : Window
    {
        private readonly List<ToDoTask> _tasks;

        public MainWindow()
        {
            InitializeComponent();
            _tasks = new List<ToDoTask>();
            FilterComboBox.SelectedIndex = 0;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string description = TaskDescriptionTextBox.Text;

            if (string.IsNullOrWhiteSpace(description)) return;

            _tasks.Add(new ToDoTask(description));
            TaskDescriptionTextBox.Clear();
            UpdateTasksList();
        }

        private void UpdateTasksList()
        {
            var filteredTasks = FilterTasks();

            TasksListBox.Items.Clear();
            foreach (var task in filteredTasks)
            {
                TasksListBox.Items.Add(CreateTaskItem(task));
            }
        }

        private List<ToDoTask> FilterTasks()
        {
            string filter = (FilterComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            return filter switch
            {
                "Do Zrobienia" => _tasks.Where(t => !t.IsCompleted).ToList(),
                "Zrobione" => _tasks.Where(t => t.IsCompleted).ToList(),
                _ => new List<ToDoTask>(_tasks)
            };
        }

        private StackPanel CreateTaskItem(ToDoTask task)
        {
            var taskPanel = new StackPanel { Orientation = Orientation.Horizontal };

            var taskCheckBox = new CheckBox
            {
                Content = task.Description,
                IsChecked = task.IsCompleted
            };
            taskCheckBox.Checked += (s, e) => task.IsCompleted = true;
            taskCheckBox.Unchecked += (s, e) => task.IsCompleted = false;

            var deleteTaskButton = new Button
            {
                Content = "Usuń",
                Tag = task
            };
            deleteTaskButton.Click += RemoveTaskButton_Click;

            taskPanel.Children.Add(taskCheckBox);
            taskPanel.Children.Add(deleteTaskButton);

            return taskPanel;
        }

        private void RemoveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is ToDoTask task)
            {
                _tasks.Remove(task);
                UpdateTasksList();
            }
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTasksList();
        }
    }
}
