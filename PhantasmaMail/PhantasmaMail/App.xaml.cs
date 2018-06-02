using NeoModules.Rest.Services;
using PhantasmaMail.Services.Navigation;
using PhantasmaMail.ViewModels;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using Application = Xamarin.Forms.Application;

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

        protected override async void OnStart()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                // Connection to internet is available
                
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
        //    var navigationService = Locator.Instance.Resolve<INavigationService>();
        //    var auth  = Locator.Instance.Resolve<IAuthenticationService>();
        //    if (auth.IsAuthenticated)
        //    {
        //        navigationService.NavigateToAsync<MainViewModel>();
        //    }
        //    else
        //    {
        //        navigationService.NavigateToAsync<ExtendedSplashViewModel>();
        //    }
        }

        
    }
}