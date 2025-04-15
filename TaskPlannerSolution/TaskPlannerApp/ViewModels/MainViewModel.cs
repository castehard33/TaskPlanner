using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using TaskPlannerApp.Data;
using TaskPlannerApp.Models;
using TaskPlannerApp.Views; // Make sure this is correct

namespace TaskPlannerApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly TaskPlannerContext _context;
        private ObservableCollection<TaskModel> _tasksToDo;
        private ObservableCollection<TaskModel> _tasksInProgress;
        private ObservableCollection<TaskModel> _tasksDone;
        private ObservableCollection<TaskModel> _tasksArchived; // Added for Archive

        private string _selectedSortOption = "None";
        private ObservableCollection<string> _sortOptions;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<TaskModel> TasksToDo
        {
            get => _tasksToDo;
            set { _tasksToDo = value; OnPropertyChanged(nameof(TasksToDo)); }
        }

        public ObservableCollection<TaskModel> TasksInProgress
        {
            get => _tasksInProgress;
            set { _tasksInProgress = value; OnPropertyChanged(nameof(TasksInProgress)); }
        }

        public ObservableCollection<TaskModel> TasksDone
        {
            get => _tasksDone;
            set { _tasksDone = value; OnPropertyChanged(nameof(TasksDone)); }
        }


        public ObservableCollection<TaskModel> TasksArchived
        {
            get => _tasksArchived;
            set { _tasksArchived = value; OnPropertyChanged(nameof(TasksArchived)); }
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
            set { _sortOptions = value; OnPropertyChanged(nameof(SortOptions)); }
        }

        public ICommand AddTaskCommand { get; }
        public ICommand MoveToInProgressCommand { get; }
        public ICommand MoveToDoneCommand { get; }
        public ICommand ArchiveTaskCommand { get; } 
        public ICommand OpenArchiveCommand { get; } 
        public ICommand SortCommand { get; }

        public MainViewModel()
        {
            _context = new TaskPlannerContext();
            _tasksToDo = new ObservableCollection<TaskModel>();
            _tasksInProgress = new ObservableCollection<TaskModel>();
            _tasksDone = new ObservableCollection<TaskModel>();
            _tasksArchived = new ObservableCollection<TaskModel>(); 

            _sortOptions = new ObservableCollection<string> { "None", "Name", "Author" };

            LoadAllTasks();

            AddTaskCommand = new RelayCommand(OpenTaskForm);
            MoveToInProgressCommand = new RelayCommand(MoveTaskToInProgress); 
            MoveToDoneCommand = new RelayCommand(MoveTaskToDone);      
            ArchiveTaskCommand = new RelayCommand(MoveTaskToArchive);    
            OpenArchiveCommand = new RelayCommand(ShowArchiveWindow);    
            SortCommand = new RelayCommand(SortTasksExecute);
        }

        private void SortTasksExecute(object? parameter)
        {
            SortTasks();
        }

        private void SortTasks()
        {
            var sortedToDo = TasksToDo.AsEnumerable();
            var sortedInProgress = TasksInProgress.AsEnumerable();
            var sortedDone = TasksDone.AsEnumerable();
            var sortedArchived = TasksArchived.AsEnumerable(); 

            switch (SelectedSortOption)
            {
                case "Author":
                    sortedToDo = sortedToDo.OrderBy(t => t.TaskAuthor);
                    sortedInProgress = sortedInProgress.OrderBy(t => t.TaskAuthor);
                    sortedDone = sortedDone.OrderBy(t => t.TaskAuthor);
                    sortedArchived = sortedArchived.OrderBy(t => t.TaskAuthor); 
                    break;
                case "Name":
                    sortedToDo = sortedToDo.OrderBy(t => t.TaskName);
                    sortedInProgress = sortedInProgress.OrderBy(t => t.TaskName);
                    sortedDone = sortedDone.OrderBy(t => t.TaskName);
                    sortedArchived = sortedArchived.OrderBy(t => t.TaskName);
                    break;
                case "None":
                default:
                    sortedToDo = sortedToDo.OrderBy(t => t.Id);
                    sortedInProgress = sortedInProgress.OrderBy(t => t.Id);
                    sortedDone = sortedDone.OrderBy(t => t.Id);
                    sortedArchived = sortedArchived.OrderBy(t => t.Id); 
                    break;
            }


            TasksToDo = new ObservableCollection<TaskModel>(sortedToDo);
            TasksInProgress = new ObservableCollection<TaskModel>(sortedInProgress);
            TasksDone = new ObservableCollection<TaskModel>(sortedDone);
            TasksArchived = new ObservableCollection<TaskModel>(sortedArchived); 
        }


        private async void LoadAllTasks()
        {
            try
            {
                var allTasks = await _context.Tasks.ToListAsync(); 

                var todoTasks = allTasks.Where(t => t.Status == AppTaskStatus.ToDo);
                var inProgressTasks = allTasks.Where(t => t.Status == AppTaskStatus.InProgress);
                var doneTasks = allTasks.Where(t => t.Status == AppTaskStatus.Done);
                var archivedTasks = allTasks.Where(t => t.Status == AppTaskStatus.Archived);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    TasksToDo.Clear();
                    TasksInProgress.Clear();
                    TasksDone.Clear();
                    TasksArchived.Clear();

                    foreach (var task in todoTasks) TasksToDo.Add(task);
                    foreach (var task in inProgressTasks) TasksInProgress.Add(task);
                    foreach (var task in doneTasks) TasksDone.Add(task);
                    foreach (var task in archivedTasks) TasksArchived.Add(task);

                    SortTasks();
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
                        Status = AppTaskStatus.ToDo 
                    };

                    _context.Tasks.Add(newTask);
                    await _context.SaveChangesAsync();

                    LoadAllTasks(); 

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

        private async void MoveTaskToInProgress(object? parameter) 
        {
            if (parameter is TaskModel task)
            {
                await UpdateTaskStatus(task.Id, AppTaskStatus.InProgress, "Task moved to In Progress");
            }
        }

        private async void MoveTaskToDone(object? parameter) 
        {
            if (parameter is TaskModel task)
            {
                await UpdateTaskStatus(task.Id, AppTaskStatus.Done, "Task moved to Done");
            }
        }

        private async void MoveTaskToArchive(object? parameter)
        {
            if (parameter is TaskModel task)
            {
                await UpdateTaskStatus(task.Id, AppTaskStatus.Archived, "Task moved to Archive");
            }
        }

        private async Task UpdateTaskStatus(int taskId, AppTaskStatus newStatus, string successMessage)
        {
            try
            {
                var existingTask = await _context.Tasks.FindAsync(taskId); 

                if (existingTask == null)
                {
                    MessageBox.Show("Task not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (existingTask.Status == newStatus) return; 

                existingTask.Status = newStatus;

                await _context.SaveChangesAsync();

                LoadAllTasks(); 

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error moving task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowArchiveWindow(object? parameter)
        {
            var archiveWindow = new ArchiveWindow(TasksArchived)
            {
                Owner = Application.Current.MainWindow 
            };
            archiveWindow.Show();
        }
    }
}