using MyTaskManager.Client.Models;
using MyTaskManager.Client.Services;
using MyTaskManager.Client.Views;
using MyTaskManager.Client.Views.Pages;
using MyTaskManager.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;

namespace MyTaskManager.Client.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private CommonViewService _commonViewService;

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
            _commonViewService = new CommonViewService();

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
            OpenPage(page, _userInfoBtnName, this);
        }

        private void OpenProjectsPage()
        {
            var page = new ProjectsPage();
            OpenPage(page, _userProjectsBtnName, new ProjectsPageViewModel(Token));
        }

        private void OpenDesksPage()
        {
            _commonViewService.ShowMessage(_userDesksBtnName);
        }

        private void OpenTasksPage()
        {
            var page = new UserTasksPage();
            OpenPage(page, _userTasksBtnName, new UserTasksPageViewModel(Token));
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
            _commonViewService.ShowMessage(_manageUsersBtnName);
        }
        #endregion

        private void OpenPage(Page page, string namePage, BindableBase viewModel)
        {
            SelectedPage = page;
            SelectedPageName = namePage;
            SelectedPage.DataContext = viewModel;
        }
    }
}
