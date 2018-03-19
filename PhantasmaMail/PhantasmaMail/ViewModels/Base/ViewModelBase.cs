using PhantasmaMail.Services.Dialog;
using PhantasmaMail.Services.Navigation;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels.Base
{
	public abstract class ViewModelBase : BindableObject
	{
		private bool _isBusy;

		protected readonly IDialogService DialogService;
		protected readonly INavigationService NavigationService;

		public ViewModelBase()
		{
			DialogService = Locator.Instance.Resolve<IDialogService>();
			NavigationService = Locator.Instance.Resolve<INavigationService>();
		}

		public bool IsBusy
		{
			get
			{
				return _isBusy;
			}

			set
			{
				_isBusy = value;
				OnPropertyChanged();
			}
		}

		public virtual Task InitializeAsync(object navigationData)
		{
			return Task.FromResult(false);
		}
	}
}
