using Prism.Mvvm;
using Prism.Commands;
using System.Windows;
using MyTaskManager.Client.Models;
using MyTaskManager.Common.Models;
using MyTaskManager.Client.Views.Pages;
using System.Windows.Controls;
using MyTaskManager.Client.Views;

namespace MyTaskManager.Client.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        #region COMMANDS
        public DelegateCommand OpenMyInfoPageCommand;
        public DelegateCommand OpenProjectsPageCommand;
        public DelegateCommand OpenDesksPageCommand;
        public DelegateCommand OpenTasksPageCommand;
        public DelegateCommand LogoutCommand;

        public DelegateCommand OpenUsersManagementCommand;
        #endregion

        public MainWindowViewModel(AuthToken token, UserDto user, Window currentWindow = null)
        {
            _currentWindow = currentWindow;
            Token = token;
            CurrentUser = user;

            OpenMyInfoPageCommand = new DelegateCommand(OpenMyInfoPage);
            NavButtons.Add(_userInfoBtnName, OpenMyInfoPageCommand);

            OpenProjectsPageCommand = new DelegateCommand(OpenProjectsPage);
            NavButtons.Add(_userProjectsBtnName, OpenProjectsPageCommand);

            OpenDesksPageCommand = new DelegateCommand(OpenDesksPage);
            NavButtons.Add(_userDesksBtnName, OpenDesksPageCommand);

            OpenTasksPageCommand = new DelegateCommand(OpenTasksPage);
            NavButtons.Add(_userTasksBtnName, OpenTasksPageCommand);

            if (CurrentUser.Status == UserStatus.Admin)
            {
                OpenUsersManagementCommand = new DelegateCommand(OpenUsersManagement);
                NavButtons.Add(_manageUsersBtnName, OpenUsersManagementCommand);
            }

            LogoutCommand = new DelegateCommand(Logout);
            NavButtons.Add(_logoutBtnName, LogoutCommand);
            _currentWindow = currentWindow;

            OpenMyInfoPage();
        }

        #region PROPERTIES
        private Window _currentWindow;
        private readonly string _userProjectsBtnName = "My projects";
        private readonly string _userDesksBtnName = "My desks";
        private readonly string _userTasksBtnName = "My tasks";
        private readonly string _userInfoBtnName = "My info";
        private readonly string _logoutBtnName = "Logout";

        private readonly string _manageUsersBtnName = "Users";

        private Dictionary<string, DelegateCommand> _navButtons = new Dictionary<string, DelegateCommand>();
        public Dictionary<string, DelegateCommand> NavButtons
        {
            get => _navButtons;
            set
            {
                _navButtons = value;
                RaisePropertyChanged(nameof(NavButtons));
            }
        }

        private AuthToken _token;
        public AuthToken Token
        {
            get => _token;
            private set
            {
                _token = value;
                RaisePropertyChanged(nameof(Token));
            }
        }

        private UserDto _currentUser;
        public UserDto CurrentUser
        {
            get => _currentUser;
            private set
            {
                _currentUser = value;
                RaisePropertyChanged(nameof(CurrentUser));
            }
        }

        private string _selectedPageName;
        public string SelectedPageName
        {
            get => _selectedPageName;
            set
            {
                _selectedPageName = value;
                RaisePropertyChanged(nameof(SelectedPageName));
            }
        }

        private Page _selectedPage;
        public Page SelectedPage
        {
            get => _selectedPage;
            set
            {
                _selectedPage = value;
                RaisePropertyChanged(nameof(SelectedPage));
            }
        }

        #endregion

        #region METHODS
        private void OpenMyInfoPage()
        {
            var page = new UserInfoPage();
            page.DataContext = this;
            OpenPage(page, _userInfoBtnName);
        }

        private void OpenProjectsPage()
        {
            SelectedPageName = _userProjectsBtnName;
            ShowMessage(_userProjectsBtnName);
        }

        private void OpenDesksPage()
        {
            SelectedPageName = _userDesksBtnName;
            ShowMessage(_userDesksBtnName);
        }

        private void OpenTasksPage()
        {
            SelectedPageName = _userTasksBtnName;
            ShowMessage(_userTasksBtnName);
        }

        private void Logout()
        {
            var question = MessageBox.Show("Are you sure?", "logout", MessageBoxButton.YesNo);

            if (question == MessageBoxResult.Yes && _currentWindow != null)
            {
                Login login = new Login();
                login.Show();
                _currentWindow.Close();
            }
        }

        private void OpenUsersManagement()
        {
            SelectedPageName = _manageUsersBtnName;
            ShowMessage(_manageUsersBtnName);
        }
        #endregion

        private void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void OpenPage(Page page, string namePage)
        {
            SelectedPage = page;
            SelectedPageName = namePage;
        }
    }
}
