using MyTaskManager.Client.Models;
using MyTaskManager.Client.Services;
using MyTaskManager.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace MyTaskManager.Client.ViewModels
{
    public class UserDesksPageViewModel : BindableBase
    {
        private CommonViewService _viewService;
        private DesksRequestService _desksRequestService;
        private UsersRequestService _usersRequestService;
        private DeskViewService _deskViewService;

        #region COMMANDS

        public DelegateCommand OpenEditeDeskCommand { get; set; }
        public DelegateCommand CreateOrUpdateDeskCommand { get; private set; }
        public DelegateCommand DeleteDeskCommand { get; private set; }
        public DelegateCommand SelectPhotoForDeskCommand { get; private set; }
        public DelegateCommand AddNewColumnItemCommand { get; private set; }
        public DelegateCommand<object> RemoveColumnItemCommand { get; private set; }

        #endregion

        public UserDesksPageViewModel(AuthToken token)
        {
            Token = token;
            _viewService = new CommonViewService();
            _desksRequestService = new DesksRequestService();
            _usersRequestService = new UsersRequestService();
            _deskViewService = new DeskViewService(Token, _desksRequestService, _viewService);

            OpenEditeDeskCommand = new DelegateCommand(OpenUpdateDesk);
            CreateOrUpdateDeskCommand = new DelegateCommand(UpdateDesk);
            DeleteDeskCommand = new DelegateCommand(DeleteDesk);
            SelectPhotoForDeskCommand = new DelegateCommand(SelectPhotoForDesk);
            AddNewColumnItemCommand = new DelegateCommand(AddNewColumnItem);
            RemoveColumnItemCommand = new DelegateCommand<object>(RemoveColumnItem);

            ContextMenuCommand.Add("Edite", OpenEditeDeskCommand);
            ContextMenuCommand.Add("Delete", DeleteDeskCommand);

            UpdatePage();

        }

        #region PROPERTIES
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

        private List<ModelClient<DeskDto>> _allDesks = new List<ModelClient<DeskDto>>();
        public List<ModelClient<DeskDto>> AllDesks
        {
            get => _allDesks;
            set
            {
                _allDesks = value;
                RaisePropertyChanged(nameof(AllDesks));
            }
        }

        private ModelClient<DeskDto> _selectedDesk;
        public ModelClient<DeskDto> SelectedDesk
        {
            get => _selectedDesk;
            set
            {
                _selectedDesk = value;
                RaisePropertyChanged(nameof(SelectedDesk));
            }
        }

        private Dictionary<string, DelegateCommand> _contextMenuCommand = new Dictionary<string, DelegateCommand>();

        public Dictionary<string, DelegateCommand> ContextMenuCommand
        {
            get => _contextMenuCommand;
            set
            {
                _contextMenuCommand = value;
                RaisePropertyChanged(nameof(ContextMenuCommand));
            }
        }

        private ObservableCollection<ColumnBindingHelp> _columnsForNewDesk = new ObservableCollection<ColumnBindingHelp>()
        {
            new ColumnBindingHelp("New"),
            new ColumnBindingHelp("In Progress"),
            new ColumnBindingHelp("In Review"),
            new ColumnBindingHelp("Done"),
        };
        public ObservableCollection<ColumnBindingHelp> ColumnsForNewDesk
        {
            get => _columnsForNewDesk;
            set
            {
                _columnsForNewDesk = value;
                RaisePropertyChanged(nameof(ColumnsForNewDesk));
            }
        }
        #endregion

        #region METHODS
        private void OpenUpdateDesk()
        {
            SelectedDesk = _deskViewService.GetDeskClientById(SelectedDesk.Model.Id);

            ColumnsForNewDesk = new ObservableCollection<ColumnBindingHelp>(SelectedDesk.Model.Columns.Select(c => new ColumnBindingHelp(c)));
            _deskViewService.OpenViewDeskInfo(SelectedDesk.Model.Id, this);
        }

        private void SelectPhotoForDesk()
        {
            _deskViewService.SelectPhotoForDesk(SelectedDesk);
        }

        private void UpdateDesk()
        {
            SelectedDesk.Model.Columns = ColumnsForNewDesk.Select(c => c.Value).ToArray();
            _deskViewService.UpdateDesk(SelectedDesk.Model);
            UpdatePage();
        }

        private void DeleteDesk()
        {
            _deskViewService.DeleteDesk(SelectedDesk.Model.Id);
            UpdatePage();
        }

        private void UpdatePage()
        {
            SelectedDesk = null;
            AllDesks = _deskViewService.GetAllDesks();
            _viewService.CurrentOpenedWindow?.Close();
        }

        private void AddNewColumnItem()
        {
            ColumnsForNewDesk.Add(new ColumnBindingHelp("Column"));
        }

        private void RemoveColumnItem(object item)
        {
            if (item is ColumnBindingHelp column)
            {
                ColumnsForNewDesk.Remove(column);
            }
        }
        #endregion
    }
}
