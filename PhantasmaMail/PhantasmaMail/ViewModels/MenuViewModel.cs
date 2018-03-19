using PhantasmaMail.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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

		public ICommand MenuItemSelectedCommand => new Command<Models.UI.MenuItem>(OnSelectMenuItem);

		private void InitMenuItems()
		{
			MenuItems.Add(new Models.UI.MenuItem
			{
				Title = "Home",
				MenuItemType = Models.UI.MenuItemType.Inbox,
				//ViewModelType = typeof(InboxViewModel),
				IsEnabled = true
			});

			MenuItems.Add(new Models.UI.MenuItem
			{
				Title = "Book a room",
				MenuItemType = Models.UI.MenuItemType.About,
				//ViewModelType = typeof(AboutViewModel),
				IsEnabled = true
			});

			MenuItems.Add(new Models.UI.MenuItem
			{
				Title = "Logout",
				MenuItemType = Models.UI.MenuItemType.Logout,
				//ViewModelType = typeof(LoginViewModel),
				IsEnabled = true,
				AfterNavigationAction = RemoveUserCredentials
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
