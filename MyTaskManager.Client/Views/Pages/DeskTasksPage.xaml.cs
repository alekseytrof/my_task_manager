using System.Windows.Controls;

namespace MyTaskManager.Client.Views.Pages
{
    public partial class DeskTasksPage : Page
    {
        public Grid TasksGrid { get; private set; }
        public DeskTasksPage()
        {
            InitializeComponent();
            TasksGrid = tasksGrid;
        }
    }
}
