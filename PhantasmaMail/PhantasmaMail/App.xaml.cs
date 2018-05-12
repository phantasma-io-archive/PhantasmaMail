using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using PCLStorage;
using PhantasmaMail.Services.Db;
using PhantasmaMail.Services.Navigation;
using PhantasmaMail.ViewModels;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms.Xaml;
using Application = Xamarin.Forms.Application;
using FileAccess = PCLStorage.FileAccess;
using Message = PhantasmaMail.Models.Message;

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