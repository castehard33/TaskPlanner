using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TaskPlannerApp.Models;
using TaskPlannerApp.ViewModels;

namespace TaskPlannerApp.Views
{
    public partial class MainWindow : Window
    {

        private object? _draggedItem;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.MainViewModel();
        }


    }
}
