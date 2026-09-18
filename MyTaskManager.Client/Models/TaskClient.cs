using MyTaskManager.Client.Models.Extensions;
using MyTaskManager.Common.Models;
using System.Windows.Media.Imaging;

namespace MyTaskManager.Client.Models
{
    public class TaskClient
    {
        public TaskClient(TaskDto model)
        {
            Model = model;
        }

        public TaskDto Model { get; private set; }
        public UserDto Creator { get; set; }
        public UserDto Executor { get; set; }

        public BitmapImage Image
        {
            get
            {
                return Model.LoadImage();
            }
        }

        public bool IsHaveFile
        {
            get => Model?.File != null;
        }
    }
}
