using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using TaskPlannerApp.Models;
using TaskPlannerApp.Views;

namespace TaskPlannerApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<TaskModel> TasksToDo { get; set; } = [];
        public ObservableCollection<TaskModel> TasksInProgress { get; set; } = [];
        public ObservableCollection<TaskModel> TasksDone { get; set; } = [];

        public ICommand AddTaskCommand { get; }


        public MainViewModel()
        {
            AddTaskCommand = new RelayCommand(OpenTaskForm);
        }

        private void OpenTaskForm(object? parameter)
        {
            string defaultTaskName = "Wprowadź nazwę zadania";
            string defaultTaskAuthor = "Wprowadź autora";

            var taskForm = new TaskFormWindow(defaultTaskName, defaultTaskAuthor);

            if (taskForm.ShowDialog() == true)
            {
                TasksToDo.Add(new TaskModel { TaskName = taskForm.TaskName, TaskAuthor = taskForm.TaskAuthor });

                MessageBox.Show("Zadanie zostało dodane!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
