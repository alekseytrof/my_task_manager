using MyTaskManager.Client.ViewModels;
using MyTaskManager.Common.Models;
using System.Windows;
using System.Windows.Controls;

namespace MyTaskManager.Client.Views.AddWindows
{
    public partial class AddUsersToProjectWindow : Window
    {
        public AddUsersToProjectWindow()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = (ProjectsPageViewModel)DataContext;

            foreach (UserDto user in e.RemovedItems)
            {
                viewModel.SelectedUsersForProject.Add(user);
            }

            foreach (UserDto user in e.AddedItems)
            {
                viewModel.SelectedUsersForProject.Add(user);
            }
        }
    }
}
