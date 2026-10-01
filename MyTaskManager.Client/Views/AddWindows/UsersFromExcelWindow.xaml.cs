using MyTaskManager.Client.ViewModels;
using MyTaskManager.Common.Models;
using System.Windows;
using System.Windows.Controls;

namespace MyTaskManager.Client.Views.AddWindows
{
    public partial class UsersFromExcelWindow : Window
    {
        public UsersFromExcelWindow()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var context = DataContext as UsersPageViewModel;


            foreach (var item in e.RemovedItems)
            {
                if (item.GetType() == typeof(UserDto))
                {
                    var user = (UserDto)item;
                    if (context.SelectedUsersFromExcel.Contains(user))
                        context.SelectedUsersFromExcel.Remove(user);
                }
            }
            foreach (var item in e.AddedItems)
            {
                if (item.GetType() == typeof(UserDto))
                {
                    var user = (UserDto)item;
                    context.SelectedUsersFromExcel.Add(user);
                }
            }

        }
    }
}
