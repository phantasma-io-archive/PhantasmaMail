using PhantasmaMail.ViewModels.Base;
using System;
using System.Threading.Tasks;

namespace PhantasmaMail.Services.Navigation
{
	public interface INavigationService
	{
		Task InitializeAsync();

		Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;

		Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;

		Task NavigateToAsync(Type viewModelType);

		Task NavigateToAsync(Type viewModelType, object parameter);

		Task NavigateBackAsync();

		Task RemoveLastFromBackStackAsync();

		Task NavigateToPopupAsync<TViewModel>(bool animate) where TViewModel : ViewModelBase;

		Task NavigateToPopupAsync<TViewModel>(object parameter, bool animate) where TViewModel : ViewModelBase;

	    ViewModelBase GetCurrentViewModel();
	}
}
