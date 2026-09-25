using MyTaskManager.Common.Models;
using Prism.Mvvm;
using System.IO;
using System.Windows;

namespace MyTaskManager.Client.Services
{
    public class CommonViewService
    {
        private string _imageDialogFilterPattern = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

        public CommonViewService() { }

        public Window CurrentOpenedWindow { get; private set; }
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void ShowActionResult(System.Net.HttpStatusCode code, string message)
        {
            if (code == System.Net.HttpStatusCode.OK)
            {
                ShowMessage(code.ToString() + $"\n{message}");
            }
            else
            {
                ShowMessage(code.ToString() + "\nError!!!");
            }
        }

        public void OpenWindow(Window window, BindableBase bindable)
        {
            CurrentOpenedWindow = window;
            window.DataContext = bindable;
            window.ShowDialog();
        }

        public string GetFileFromDialog(string filter)
        {
            string filePath = string.Empty;

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Filter = filter;

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                filePath = dlg.FileName;
            }
            return filePath;
        }

        public void SetPhotoForObject(CommonDto model)
        {
            string photoPath = GetFileFromDialog(_imageDialogFilterPattern);
            if (string.IsNullOrEmpty(photoPath) == false)
            {
                var photoBytes = File.ReadAllBytes(photoPath);
                model.Photo = photoBytes;
            }
        }
    }
}
