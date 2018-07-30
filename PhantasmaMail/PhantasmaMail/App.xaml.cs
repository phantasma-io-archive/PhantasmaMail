using System;
using System.IO;
using System.Reflection;
using NeoModules.Rest.DTOs.NeoNotifications;
using Newtonsoft.Json;
using PhantasmaMail.Services.Navigation;
using PhantasmaMail.ViewModels;
using PhantasmaMail.ViewModels.Base;
using Syncfusion.Licensing;
using Xamarin.Forms.Xaml;
using Application = Xamarin.Forms.Application;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace PhantasmaMail
{
    public partial class App : Application
    {
        public App()
        {
            //Register Syncfusion license
            SyncfusionLicenseProvider.RegisterLicense("MTEyMThAMzEzNjJlMzIyZTMwS2FEWitRbksreDlYUFlrcnNtT0pUSnQrY0pBcE1oUDRMUjBuWXZ1R1dJaz0=");

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
            //TODO
            var assembly = typeof(App).GetTypeInfo().Assembly;

            Stream stream = assembly.GetManifestResourceStream("PhantasmaMail.tokens.json");
            using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
            {
                var json = reader.ReadToEnd();
                var rootobject = JsonConvert.DeserializeObject<TokenResult>(json);

                AppSettings.TokenList = rootobject;
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