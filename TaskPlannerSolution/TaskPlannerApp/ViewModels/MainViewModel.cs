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

        public ICommand MoveToInProgressCommand { get; }



        public MainViewModel()
        {
            _context = new TaskPlannerContext();
            LoadTasks();

            AddTaskCommand = new RelayCommand(OpenTaskForm);
            MoveToInProgressCommand = new RelayCommand(MoveTaskToInProgressCommand);
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

        private async void OpenTaskForm(object? parameter)
        {

            var taskForm = new TaskFormWindow(
               taskName: "Enter task name",
               taskAuthor: "Enter task author"
           );

            if (taskForm.ShowDialog() == true)
            {
                try
                {
                    var newTask = new TaskModel
                    {
                        TaskName = taskForm.TaskName,
                        TaskAuthor = taskForm.TaskAuthor,
                    };

                    _context.Tasks.Add(newTask);
                    await _context.SaveChangesAsync();

                    TasksToDo.Add(newTask);

                    MessageBox.Show("Task successfully added to the database!",
                                    "Success",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding task: {ex.Message}",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
        }

        private async void MoveTaskToInProgressCommand(object? parameter)
        {
            if (parameter is TaskModel task)
            {
                try
                {
                    // Find the task in the database
                    var existingTask = await _context.Tasks
                        .FirstOrDefaultAsync(t => t.Id == task.Id);

                    if (existingTask == null)
                    {
                        MessageBox.Show("Task not found.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Update status using the enum
                    existingTask.Status = AppTaskStatus.InProgress;

                    // Save changes
                    await _context.SaveChangesAsync();

                    // Refresh entire collection
                    var updatedTasks = await _context.Tasks
                        .Where(t => t.Status == AppTaskStatus.ToDo)
                        .ToListAsync();

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TasksToDo.Clear();
                        foreach (var updatedTask in updatedTasks)
                        {
                            TasksToDo.Add(updatedTask);
                        }
                    });

                    MessageBox.Show("Task moved to In Progress", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error moving task: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
