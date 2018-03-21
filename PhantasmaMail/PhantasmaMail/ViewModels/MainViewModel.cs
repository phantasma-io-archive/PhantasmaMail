using PhantasmaMail.ViewModels.Base;
using System.Threading.Tasks;

namespace PhantasmaMail.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private MenuViewModel _menuViewModel;

		public MainViewModel(MenuViewModel menuViewModel)
		{
			_menuViewModel = menuViewModel;
		}

		public MenuViewModel MenuViewModel
		{
			get
			{
				return _menuViewModel;
			}

			set
			{
				_menuViewModel = value;
				OnPropertyChanged();
			}
		}

		public override Task InitializeAsync(object navigationData)
		{
			return Task.WhenAll
				(
					_menuViewModel.InitializeAsync(navigationData),
					NavigationService.NavigateToAsync<DashboardViewModel>()
				);
		}
	}
}
