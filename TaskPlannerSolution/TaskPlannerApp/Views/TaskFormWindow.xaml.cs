using System.Windows;

namespace TaskPlannerApp.Views
{
    public partial class TaskFormWindow : Window
    {
        public string TaskName { get; set; } //problemy przy required więc nullable
        public string TaskAuthor { get; set; }

        public TaskFormWindow(string taskName, string taskAuthor)
        {
            InitializeComponent();
            TaskName = taskName;
            TaskAuthor = taskAuthor;
        }

        private void SaveTask(object sender, RoutedEventArgs e)
        {
            TaskName = TaskNameTextBox.Text;
            TaskAuthor = TaskAuthorTextBox.Text;

            if (string.IsNullOrWhiteSpace(TaskName) || string.IsNullOrWhiteSpace(TaskAuthor))
            {
                MessageBox.Show("All texts boxes must be filled out", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }
    }
}
