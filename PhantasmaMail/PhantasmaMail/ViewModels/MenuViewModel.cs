using PhantasmaMail.Resources;
using PhantasmaMail.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
	public class MenuViewModel : ViewModelBase, IHandleViewAppearing, IHandleViewDisappearing
	{
		private ObservableCollection<Models.UI.MenuItem> _menuItems;

		public ObservableCollection<Models.UI.MenuItem> MenuItems
		{
			get
			{
				return _menuItems;
			}
			set
			{
				_menuItems = value;
				OnPropertyChanged();
			}
		}

		public MenuViewModel()
		{
			MenuItems = new ObservableCollection<Models.UI.MenuItem>();
			InitMenuItems();
		}

		public ICommand MenuItemSelectedCommand => new Command<Models.UI.MenuItem>(OnSelectMenuItem);
		public ICommand GoToSettingsCommand => new Command(async () => await GoToSettingsExecute());
		public ICommand LoggoutCommand => new Command(async () => await LogoutExecute());

		private void InitMenuItems()
		{
			MenuItems.Add(new Models.UI.MenuItem
			{
				Title = AppResource.MenuItem_Inbox,
				MenuItemType = Models.UI.MenuItemType.Inbox,
				ViewModelType = typeof(InboxViewModel),
				IsEnabled = true
			});

			MenuItems.Add(new Models.UI.MenuItem
			{
				Title = AppResource.MenuItem_Sent,
				MenuItemType = Models.UI.MenuItemType.Sent,
				ViewModelType = typeof(SentViewModel),
				IsEnabled = true
			});
			MenuItems.Add(new Models.UI.MenuItem
			{
				Title = AppResource.MenuItem_Draft,
				MenuItemType = Models.UI.MenuItemType.Draft,
				ViewModelType = typeof(DraftViewModel),
				IsEnabled = false
			});
            MenuItems.Add(new Models.UI.MenuItem
            {
                Title = AppResource.MenuItem_Trash,
                MenuItemType = Models.UI.MenuItemType.Trash,
                ViewModelType = typeof(LoginViewModel),
                IsEnabled = false
            });
            MenuItems.Add(new Models.UI.MenuItem
            {
                Title = AppResource.MenuItem_Wallet,
                MenuItemType = Models.UI.MenuItemType.Wallet,
                ViewModelType = typeof(LoginViewModel),
                IsEnabled = false
            });
        }

		private async void OnSelectMenuItem(Models.UI.MenuItem item)
		{
			//TODO
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

		private Task RemoveUserCredentials()
		{
			// todo
			//return _authenticationService.LogoutAsync();
			return Task.Delay(1);
		}

		public Task OnViewAppearingAsync(VisualElement view)
		{
			return Task.FromResult(true);
		}

		public Task OnViewDisappearingAsync(VisualElement view)
		{
			return Task.FromResult(true);
		}
	}
}
