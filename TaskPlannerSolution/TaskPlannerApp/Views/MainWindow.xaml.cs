using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TaskPlannerApp.Models;

namespace TaskPlannerApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenTaskForm(object sender, RoutedEventArgs e)
        {
            var taskForm = new TaskFormWindow(this);
            taskForm.ShowDialog(); 
        }
        public void AddTaskToList(string taskName, string author)
        {
            var taskCard = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(34, 40, 44)),
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                CornerRadius = new CornerRadius(5),
                BorderBrush = Brushes.Transparent,
                BorderThickness = new Thickness(2)
            };

            taskCard.MouseEnter += (s, e) => taskCard.BorderBrush = Brushes.White;
            taskCard.MouseLeave += (s, e) => taskCard.BorderBrush = Brushes.Transparent;

            var taskStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
            };

            var taskNameText = new TextBlock
            {
                Text = taskName,
                FontWeight = System.Windows.FontWeights.Bold,
                FontSize = 14,
                Foreground = Brushes.White
            };

            var authorText = new TextBlock
            {
                Text = "Autor: " + author,
                FontSize = 12,
                Foreground = Brushes.White,
                Margin = new Thickness(0, 20, 0, 0),
            };

            taskStackPanel.Children.Add(taskNameText);
            taskStackPanel.Children.Add(authorText);

            taskCard.Child = taskStackPanel;

            TaskStackPanel.Children.Add(taskCard);
        }
    }
}
