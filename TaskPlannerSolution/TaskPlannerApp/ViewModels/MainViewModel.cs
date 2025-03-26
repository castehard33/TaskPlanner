using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TaskPlannerApp.Data;
using System.Windows;
using TaskPlannerApp.Models;
using TaskPlannerApp.Views;
using Microsoft.EntityFrameworkCore;


namespace TaskPlannerApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private readonly TaskPlannerContext _context;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<TaskModel> TasksToDo { get; set; } = [];
        public ObservableCollection<TaskModel> TasksInProgress { get; set; } = [];
        public ObservableCollection<TaskModel> TasksDone { get; set; } = [];

        public ICommand AddTaskCommand { get; }


        public MainViewModel()
        {
            _context = new TaskPlannerContext();
            LoadTasks();

            AddTaskCommand = new RelayCommand(OpenTaskForm);
        }

        private void LoadTasks()
        {
            TasksToDo = new ObservableCollection<TaskModel>(
                _context.Tasks.Where(t => t.Status == AppTaskStatus.ToDo).ToList()
            );

            TasksInProgress = new ObservableCollection<TaskModel>(
                _context.Tasks.Where(t => t.Status == AppTaskStatus.InProgress).ToList()
            );

            TasksDone = new ObservableCollection<TaskModel>(
                _context.Tasks.Where(t => t.Status == AppTaskStatus.Done).ToList()
            );
        }

        private void OpenTaskForm(object? parameter)
        {
            string defaultTaskName = "Wprowadź nazwę zadania";
            string defaultTaskAuthor = "Wprowadź autora";

            var taskForm = new TaskFormWindow(defaultTaskName, defaultTaskAuthor);

            if (taskForm.ShowDialog() == true)
            {
                TasksToDo.Add(new TaskModel { TaskName = taskForm.TaskName, TaskAuthor = taskForm.TaskAuthor });

                //MessageBox.Show("Zadanie zostało dodane!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


    }
}
