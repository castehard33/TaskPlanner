using System.Collections.ObjectModel;
using System.Windows;
using TaskPlannerApp.Models; 

namespace TaskPlannerApp.Views 
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