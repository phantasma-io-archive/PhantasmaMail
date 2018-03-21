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

		private void InitMenuItems()
		{
			MenuItems.Add(new Models.UI.MenuItem
			{
				Title = "Dashboard",
				MenuItemType = Models.UI.MenuItemType.Inbox,
				ViewModelType = typeof(DashboardViewModel),
				IsEnabled = true
			});

			MenuItems.Add(new Models.UI.MenuItem
			{
				Title = "Login",
				MenuItemType = Models.UI.MenuItemType.About,
				ViewModelType = typeof(LoginViewModel),
				IsEnabled = true
			});
		}

		private async void OnSelectMenuItem(Models.UI.MenuItem item)
		{
			if (item.IsEnabled && item.ViewModelType != null)
			{
				item.AfterNavigationAction?.Invoke();
				await NavigationService.NavigateToAsync(item.ViewModelType, item);
			}
		}

		private Task RemoveUserCredentials()
		{
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
