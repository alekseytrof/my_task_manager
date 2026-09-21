using MyTaskManager.Client.Models.Extensions;
using MyTaskManager.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MyTaskManager.Client.Models
{
    public class ModelClient<T> where T : CommonDto
    {
        public T Model { get; set; }

        public ModelClient(T model)
        {
            Model = model;
        }

        public BitmapImage Image
        {
            get => Model?.LoadImage();
        }
    }
}
