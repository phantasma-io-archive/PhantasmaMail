using PhantasmaMail.ViewModels.Base;
using System.Threading.Tasks;

namespace PhantasmaMail.ViewModels
{
	public class ExtendedSplashViewModel : ViewModelBase
	{
		public override async Task InitializeAsync(object navigationData)
		{
			IsBusy = true;

			await NavigationService.InitializeAsync();

			IsBusy = false;
		}
	}
}
