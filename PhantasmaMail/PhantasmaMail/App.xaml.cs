using PhantasmaMail.Services.Navigation;
using PhantasmaMail.ViewModels;
using PhantasmaMail.ViewModels.Base;
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

        public bool IsLoggedIn { get; set; }

        public void BuildDependencies()
        {
            Locator.Instance.Build();
        }

        private void InitNavigation()
        {
            var navigationService = Locator.Instance.Resolve<INavigationService>();
            navigationService.NavigateToAsync<ExtendedSplashViewModel>();
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