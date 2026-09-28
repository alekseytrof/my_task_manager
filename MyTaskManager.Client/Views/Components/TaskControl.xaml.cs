using MyTaskManager.Client.Models;
using System.Windows.Controls;

namespace MyTaskManager.Client.Views.Components
{
    public partial class TaskControl : UserControl
    {
        public TaskControl(TaskClient task)
        {
            InitializeComponent();
            DataContext = task;
        }
    }
}
