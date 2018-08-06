using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using PhantasmaMail.Models.UI;
using PhantasmaMail.Resources;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;
using MenuItem = PhantasmaMail.Models.UI.MenuItem;

namespace PhantasmaMail.ViewModels
{
    public class MenuViewModel : ViewModelBase, IHandleViewAppearing, IHandleViewDisappearing
    {
        private string _boxName;

        private ObservableCollection<MenuItem> _menuItems;

        public string BoxName
        {
            get => _boxName;
            set
            {
                _boxName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get => _menuItems;
            set
            {
                _menuItems = value;
                OnPropertyChanged();
            }
        }

        public ICommand MenuItemSelectedCommand => new Xamarin.Forms.Command<MenuItem>(OnSelectMenuItem);
        public ICommand GoToSettingsCommand => new Command(async () => await GoToSettingsExecute());
        public ICommand LogoutCommand => new Command(async () => await LogoutExecute());

        public Task OnViewAppearingAsync(VisualElement view)
        {
            return Task.FromResult(true);
        }

        public Task OnViewDisappearingAsync(VisualElement view)
        {
            return Task.FromResult(true);
        }

        public override Task InitializeAsync(object navigationData)
        {
            MenuItems = new ObservableCollection<MenuItem>();
            InitMenuItems();
            BoxName = AuthenticationService.AuthenticatedUser.UserBox;
            return base.InitializeAsync(navigationData);
        }

        private void InitMenuItems()
        {
            MenuItems.Add(new MenuItem
            {
                Title = AppResource.MenuItem_Inbox,
                MenuItemType = MenuItemType.Inbox,
                ViewModelType = typeof(InboxViewModel),
                IsEnabled = true
            });
            MenuItems.Add(new MenuItem
            {
                Title = AppResource.MenuItem_Sent,
                MenuItemType = MenuItemType.Sent,
                ViewModelType = typeof(SentViewModel),
                IsEnabled = true
            });
            MenuItems.Add(new MenuItem
            {
                Title = AppResource.MenuItem_Drafts,
                MenuItemType = MenuItemType.Drafts,
                ViewModelType = typeof(DraftsViewModel),
                IsEnabled = true
            });
            MenuItems.Add(new MenuItem
            {
                Title = AppResource.MenuItem_Important,
                MenuItemType = MenuItemType.Important,
                ViewModelType = typeof(LoginViewModel),
                IsEnabled = false
            });
            MenuItems.Add(new MenuItem
            {
                Title = AppResource.MenuItem_Trash,
                MenuItemType = MenuItemType.Trash,
                ViewModelType = typeof(LoginViewModel),
                IsEnabled = false
            });
            MenuItems.Add(new MenuItem
            {
                Title = AppResource.MenuItem_Wallet,
                MenuItemType = MenuItemType.Wallet,
                ViewModelType = typeof(WalletTabViewModel),
                IsEnabled = true
            });
            MenuItems.Add(new MenuItem
            {
                Title = AppResource.MenuItem_Settings,
                MenuItemType = MenuItemType.Settings,
                ViewModelType = typeof(SettingsViewModel),
                IsEnabled = true
            });
        }

        private async void OnSelectMenuItem(MenuItem item)
        {
            if (item.IsEnabled && item.ViewModelType != null)
            {
                item.AfterNavigationAction?.Invoke();
                await NavigationService.NavigateToAsync(item.ViewModelType, item);
            }
        }

        private async Task GoToSettingsExecute()
        {
            await NavigationService.NavigateToAsync<SettingsViewModel>();
        }

        private async Task LogoutExecute()
        {
            await RemoveUserCredentials();
            await NavigationService.NavigateToAsync<ExtendedSplashViewModel>();
        }

        private async Task RemoveUserCredentials()
        {
            await AuthenticationService.LogoutAsync();
        }
    }
}