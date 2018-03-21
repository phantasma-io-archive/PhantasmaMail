using PhantasmaMail.Services.Navigation;
using PhantasmaMail.ViewModels;
using PhantasmaMail.ViewModels.Base;
using PhantasmaMail.Views;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PhantasmaMail
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();
			BuildDependencies();
			InitNavigation();
		}

		public void BuildDependencies()
		{
			Locator.Instance.Build();
		}

		private Task InitNavigation()
		{
			var navigationService = Locator.Instance.Resolve<INavigationService>();
			return navigationService.NavigateToAsync<ExtendedSplashViewModel>();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
