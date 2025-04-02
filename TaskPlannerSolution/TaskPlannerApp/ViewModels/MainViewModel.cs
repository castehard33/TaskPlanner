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

        private string _selectedSortOption = "None";
        private ObservableCollection<string> _sortOptions;

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

        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                if (_selectedSortOption != value)
                {
                    _selectedSortOption = value;
                    OnPropertyChanged(nameof(SelectedSortOption));
                    SortTasks();
                }
            }
        }

        public ObservableCollection<string> SortOptions
        {
            get => _sortOptions;
            set
            {
                _sortOptions = value;
                OnPropertyChanged(nameof(SortOptions));
            }
        }

        public ICommand AddTaskCommand { get; }
        public ICommand MoveToInProgressCommand { get; }
        public ICommand MoveToDoneCommand { get;  }
        public ICommand SortCommand { get;  }

        public MainViewModel()
        {
            _context = new TaskPlannerContext();
            _tasksToDo = new ObservableCollection<TaskModel>();
            _tasksInProgress = new ObservableCollection<TaskModel>();
            _tasksDone = new ObservableCollection<TaskModel>();

            _sortOptions = new ObservableCollection<string> { "None", "Name", "Author" };

            LoadAllTasks();

            AddTaskCommand = new RelayCommand(OpenTaskForm);
            MoveToInProgressCommand = new RelayCommand(MoveTaskToInProgressCommand);
            MoveToDoneCommand = new RelayCommand(MoveTaskToDoneCommand);
            SortCommand = new RelayCommand(SortTasksExecute);
        }

        private void SortTasksExecute(object? parameter)
        {
            SortTasks();
        }

        private void SortTasks()
        {
            switch (SelectedSortOption)
            {
                case "Author":
                    TasksToDo = [.. TasksToDo.OrderBy(t => t.TaskAuthor)];
                    TasksInProgress = [.. TasksInProgress.OrderBy(t => t.TaskAuthor)];
                    TasksDone = [.. TasksDone.OrderBy(t => t.TaskAuthor)];
                    break;
                case "Name":
                    TasksToDo = [.. TasksToDo.OrderBy(t => t.TaskName)];
                    TasksInProgress = [.. TasksInProgress.OrderBy(t => t.TaskName)];
                    TasksDone = [.. TasksDone.OrderBy(t => t.TaskName)];
                    break;
                case "None":
                default:
                    TasksToDo = [.. TasksToDo.OrderBy(t => t.Id)];
                    TasksInProgress = [.. TasksInProgress.OrderBy(t => t.Id)];
                    TasksDone = [.. TasksDone.OrderBy(t => t.Id)];
                    break;
            }
        }

        private async void LoadAllTasks()
        {
            try
            {
                var todoTasks = await _context.Tasks
                    .Where(t => t.Status == AppTaskStatus.ToDo)
                    .ToListAsync();

                var inProgressTasks = await _context.Tasks
                    .Where(t => t.Status == AppTaskStatus.InProgress)
                    .ToListAsync();

                var doneTasks = await _context.Tasks
                    .Where(t => t.Status == AppTaskStatus.Done)
                    .ToListAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    TasksToDo = new ObservableCollection<TaskModel>(todoTasks);
                    TasksInProgress = new ObservableCollection<TaskModel>(inProgressTasks);
                    TasksDone = new ObservableCollection<TaskModel>(doneTasks);

                    if (SelectedSortOption != "None")
                    {
                        SortTasks();
                    }
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
                    var existingTask = await _context.Tasks
                        .FirstOrDefaultAsync(t => t.Id == task.Id);

                    if (existingTask == null)
                    {
                        MessageBox.Show("Task not found.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    existingTask.Status = AppTaskStatus.InProgress;

                    await _context.SaveChangesAsync();

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

        private async void MoveTaskToDoneCommand(object? parameter)
        {
            if (parameter is TaskModel task)
            {
                try
                {
                    var existingTask = await _context.Tasks
                        .FirstOrDefaultAsync(t => t.Id == task.Id);

                    if (existingTask == null)
                    {
                        MessageBox.Show("Task not found.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    existingTask.Status = AppTaskStatus.Done;

                    await _context.SaveChangesAsync();

                    LoadAllTasks();

                    MessageBox.Show("Task moved to Done section", "Success",
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