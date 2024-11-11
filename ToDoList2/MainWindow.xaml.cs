using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ToDoListApp
{
    public partial class MainWindow : Window
    {
        private List<ToDoTask> tasks = new List<ToDoTask>();

        public MainWindow()
        {
            InitializeComponent();
            FilterComboBox.SelectedIndex = 0;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TaskDescriptionTextBox.Text))
            {
                tasks.Add(new ToDoTask(TaskDescriptionTextBox.Text));
                TaskDescriptionTextBox.Clear();
                RefreshTasksList();
            }
        }

        private void RefreshTasksList()
        {
            var filteredTasks = tasks;
            switch ((FilterComboBox.SelectedItem as ComboBoxItem)?.Content.ToString())
            {
                case "Do Zrobienia":
                    filteredTasks = tasks.Where(t => !t.IsCompleted).ToList();
                    break;
                case "Zrobione":
                    filteredTasks = tasks.Where(t => t.IsCompleted).ToList();
                    break;
            }

            TasksListBox.Items.Clear();
            foreach (var task in filteredTasks)
            {
                var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
                var checkBox = new CheckBox { Content = task.Description, IsChecked = task.IsCompleted };
                checkBox.Checked += (s, e) => task.IsCompleted = true;
                checkBox.Unchecked += (s, e) => task.IsCompleted = false;
                stackPanel.Children.Add(checkBox);

                var deleteButton = new Button { Content = "Usuń", Tag = task };
                deleteButton.Click += DeleteButton_Click;
                stackPanel.Children.Add(deleteButton);

                TasksListBox.Items.Add(stackPanel);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var task = (sender as Button)?.Tag as ToDoTask;
            if (task != null)
            {
                tasks.Remove(task);
                RefreshTasksList();
            }
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshTasksList();
        }
    }
}
