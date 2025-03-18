using System.Windows;
using TaskPlannerApp.Views;

namespace TaskPlannerApp
{
    public partial class TaskFormWindow : Window
    {
        private MainWindow _mainWindow;

        public TaskFormWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            // Pobieramy dane z formularza
            string taskName = TaskNameTextBox.Text;
            string author = AuthorTextBox.Text;

            if (string.IsNullOrEmpty(taskName) || string.IsNullOrEmpty(author))
            {
                MessageBox.Show("Proszę uzupełnić wszystkie pola.");
                return;
            }

            // Przekazujemy dane do głównego okna
            _mainWindow.AddTaskToList(taskName, author);

            // Zamykamy formularz
            this.Close();
        }
    }
}