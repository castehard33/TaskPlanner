using System.Configuration;
using System.Data;
using System.Windows;
using TaskPlannerApp.ViewModels;

namespace TaskPlannerApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }
    public static class ViewModelLocator
    {
        public static MainViewModel MainViewModel => new MainViewModel();
    }
}
