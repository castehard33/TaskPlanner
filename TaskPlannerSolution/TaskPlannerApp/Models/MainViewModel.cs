using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskPlannerApp.Models;

namespace TaskPlannerApp.Models
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<TaskModel> Tasks { get; set; } = new();

        private string _newTaskName;
        public string NewTaskName
        {
            get => _newTaskName;
            set
            {
                _newTaskName = value;
                OnPropertyChanged(nameof(NewTaskName));
            }
        }

        private string _newTaskAuthor;
        public string NewTaskAuthor
        {
            get => _newTaskAuthor;
            set
            {
                _newTaskAuthor = value;
                OnPropertyChanged(nameof(NewTaskAuthor));
            }
        }

        public ICommand AddTaskCommand { get; }

        public MainViewModel()
        {
            AddTaskCommand = new RelayCommand(AddTask);
        }

        private void AddTask(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(NewTaskName) && !string.IsNullOrWhiteSpace(NewTaskAuthor))
            {
                Tasks.Add(new TaskModel { Name = NewTaskName, Author = NewTaskAuthor });
                NewTaskName = string.Empty;
                NewTaskAuthor = string.Empty;
            }
        }
    }
}
