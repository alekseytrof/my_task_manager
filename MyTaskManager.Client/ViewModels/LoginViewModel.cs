using MyTaskManager.Client.Models;
using MyTaskManager.Client.Services;
using MyTaskManager.Client.Views;
using MyTaskManager.Common.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MyTaskManager.Client.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private UsersRequestService _usersRequestService;
        private string _cachePath = Path.GetTempPath() + "usermytaskmamager.txt";
        private Window _currentWindow;

        #region COMMAND
        public DelegateCommand<object> GetUserFromDBCommand { get; private set; }
        public DelegateCommand<object> LoginFromCacheCommand { get; private set; }
        #endregion

        public LoginViewModel()
        {
            _usersRequestService = new UsersRequestService();
            CurrentUserCache = GetUserCache();

            GetUserFromDBCommand = new DelegateCommand<object>(GetUserFromDB);
            LoginFromCacheCommand = new DelegateCommand<object>(LoginFromCache);
        }

        #region PROPERTIES
        public string UserLogin { get; set; }
        public string UserPassword { get; private set; }

        private UserDto _currentUser;
        public UserDto CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                RaisePropertyChanged(nameof(CurrentUser));
            }
        }

        private UserCache _currentUserCache;
        public UserCache CurrentUserCache
        {
            get => _currentUserCache;
            set
            {
                _currentUserCache = value;
                RaisePropertyChanged(nameof(CurrentUserCache));
            }
        }


        private AuthToken _authToken;
        public AuthToken AuthToken
        {
            get => _authToken;
            set
            {
                _authToken = value;
                RaisePropertyChanged(nameof(AuthToken));
            }
        }
        #endregion

        #region METHODS
        private void GetUserFromDB(object parameter)
        {
            var passBox = parameter as PasswordBox;

            bool isNewUser = false;

            _currentWindow = Window.GetWindow(passBox);

            if (UserLogin != CurrentUserCache?.Login ||
                UserPassword != CurrentUserCache?.Password)
            {
                isNewUser = true;
            }

            UserPassword = passBox.Password;

            AuthToken = _usersRequestService.GetToken(UserLogin, UserPassword);
            if (AuthToken == null)
                return;

            CurrentUser = _usersRequestService.GetCurrentUser(AuthToken);
            if (CurrentUser != null)
            {
                if (isNewUser)
                {
                    var saveUserCacheMessage = MessageBox.Show("Хотите сохранить логин и пароль?", "Сохранение данных", MessageBoxButton.YesNo);

                    if (saveUserCacheMessage == MessageBoxResult.Yes)
                    {
                        UserCache newUserCache = new UserCache { Login = UserLogin, Password = UserPassword };
                        CreateUserCache(newUserCache);
                    }
                }
                OpenMainWindow();
            }
        }

        private void CreateUserCache(UserCache userCache)
        {
            string jsonUserCache = JsonConvert.SerializeObject(userCache);

            using (StreamWriter sw = new StreamWriter(_cachePath, false, Encoding.Default))
            {
                sw.Write(jsonUserCache);
                MessageBox.Show("Успех!");
            }
        }

        private UserCache GetUserCache()
        {
            bool isCaheExistFile = File.Exists(_cachePath);

            if (isCaheExistFile && File.ReadAllText(_cachePath).Length > 0)
            {
                return JsonConvert.DeserializeObject<UserCache>(File.ReadAllText(_cachePath));
            }
            return null;
        }

        private void LoginFromCache(object window)
        {
            _currentWindow = window as Window;

            UserLogin = CurrentUserCache.Login;
            UserPassword = CurrentUserCache.Password;
            AuthToken = _usersRequestService.GetToken(UserLogin, UserPassword);

            CurrentUser = _usersRequestService.GetCurrentUser(AuthToken);
            if (CurrentUser != null)
            {
                OpenMainWindow();
            }
        }

        private void OpenMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = new MainWindowViewModel(AuthToken, CurrentUser, mainWindow);
            mainWindow.Show();

            _currentWindow.Close();
        }
        #endregion

    }
}
