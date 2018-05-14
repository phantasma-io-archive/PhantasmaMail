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
        private ObservableCollection<MenuItem> _menuItems;

        public MenuViewModel()
        {
            MenuItems = new ObservableCollection<MenuItem>();
            InitMenuItems();
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

        public ICommand MenuItemSelectedCommand => new Command<MenuItem>(OnSelectMenuItem);
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
                Title = AppResource.MenuItem_Compose,
                MenuItemType = MenuItemType.Compose,
                ViewModelType = typeof(ComposeViewModel),
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
                ViewModelType = typeof(WalletViewModel),
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