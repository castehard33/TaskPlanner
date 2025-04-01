using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using TaskPlannerApp.Data;
using TaskPlannerApp.Models;
using TaskPlannerApp.Views;

namespace TaskPlannerApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly TaskPlannerContext _context;
        private ObservableCollection<TaskModel> _tasksToDo;
        private ObservableCollection<TaskModel> _tasksInProgress;
        private ObservableCollection<TaskModel> _tasksDone;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<TaskModel> TasksToDo
        {
            get => _tasksToDo;
            set
            {
                _tasksToDo = value;
                OnPropertyChanged(nameof(TasksToDo));
            }
        }

        public ObservableCollection<TaskModel> TasksInProgress
        {
            get => _tasksInProgress;
            set
            {
                _tasksInProgress = value;
                OnPropertyChanged(nameof(TasksInProgress));
            }
        }

        public ObservableCollection<TaskModel> TasksDone
        {
            get => _tasksDone;
            set
            {
                _tasksDone = value;
                OnPropertyChanged(nameof(TasksDone));
            }
        }

        public ICommand AddTaskCommand { get; }
        public ICommand MoveToInProgressCommand { get; }

        public MainViewModel()
        {
            _context = new TaskPlannerContext();
            _tasksToDo = new ObservableCollection<TaskModel>();
            _tasksInProgress = new ObservableCollection<TaskModel>();
            _tasksDone = new ObservableCollection<TaskModel>();

            LoadAllTasks();

            AddTaskCommand = new RelayCommand(OpenTaskForm);
            MoveToInProgressCommand = new RelayCommand(MoveTaskToInProgressCommand);
        }

        private async void LoadAllTasks()
        {
            try
            {
                // Get tasks from database
                var todoTasks = await _context.Tasks
                    .Where(t => t.Status == AppTaskStatus.ToDo)
                    .ToListAsync();

                var inProgressTasks = await _context.Tasks
                    .Where(t => t.Status == AppTaskStatus.InProgress)
                    .ToListAsync();

                var doneTasks = await _context.Tasks
                    .Where(t => t.Status == AppTaskStatus.Done)
                    .ToListAsync();

                // Update collections on UI thread
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Replace collections entirely instead of modifying them
                    TasksToDo = new ObservableCollection<TaskModel>(todoTasks);
                    TasksInProgress = new ObservableCollection<TaskModel>(inProgressTasks);
                    TasksDone = new ObservableCollection<TaskModel>(doneTasks);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

                    // Reload all tasks instead of just adding to the collection
                    LoadAllTasks();

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
                    // Find and update task in database
                    var existingTask = await _context.Tasks
                        .FirstOrDefaultAsync(t => t.Id == task.Id);

                    if (existingTask == null)
                    {
                        MessageBox.Show("Task not found.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Update status
                    existingTask.Status = AppTaskStatus.InProgress;

                    // Save changes
                    await _context.SaveChangesAsync();

                    // Reload ALL tasks from database - this ensures UI is in sync with DB
                    LoadAllTasks();

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