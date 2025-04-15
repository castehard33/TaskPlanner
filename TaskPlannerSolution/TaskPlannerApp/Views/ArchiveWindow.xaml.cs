using System.Collections.ObjectModel;
using System.Windows;
using TaskPlannerApp.Models; // Make sure this namespace is correct

namespace TaskPlannerApp.Views // Make sure this namespace is correct
{

    public partial class ArchiveWindow : Window
    {
        
        public ObservableCollection<TaskModel> ArchivedTasks { get; set; }


        public ArchiveWindow(ObservableCollection<TaskModel> archivedTasks)
        {
            InitializeComponent();
            ArchivedTasks = archivedTasks;
            DataContext = this; 
        }
    }
}