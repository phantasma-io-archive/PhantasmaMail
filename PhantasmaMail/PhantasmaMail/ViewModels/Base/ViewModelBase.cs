using System;
using System.Diagnostics;
using PhantasmaMail.Services.Dialog;
using PhantasmaMail.Services.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using PhantasmaMail.Services.Authentication;
using PhantasmaMail.Services.Phantasma;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels.Base
{
    public abstract class ViewModelBase : BindableObject
    {
        private bool _isBusy;

        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;
        protected readonly IAuthenticationService AuthenticationService;
        protected readonly IPhantasmaService PhantasmaService;

        public ICommand SwitchToInboxCommand => new Command(async () => await SwitchToInboxExecute());
        public ICommand SwitchToSentCommand => new Command(async () => await SwitchToSentExecute());

        protected ViewModelBase()
        {
            DialogService = Locator.Instance.Resolve<IDialogService>();
            NavigationService = Locator.Instance.Resolve<INavigationService>();
            AuthenticationService = Locator.Instance.Resolve<IAuthenticationService>();
            PhantasmaService = Locator.Instance.Resolve<IPhantasmaService>();
        }

        //public ViewModelBase(IDialogService dialogService, INavigationService navigationService)
        //{
        //    DialogService = dialogService;
        //    NavigationService = navigationService;
        //}

        private async Task SwitchToInboxExecute()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                if (NavigationService.GetCurrentViewModel() is InboxViewModel) return;

                await NavigationService.NavigateToAsync<InboxViewModel>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }


        private async Task SwitchToSentExecute()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                if (NavigationService.GetCurrentViewModel() is SentViewModel) return;
                await NavigationService.NavigateToAsync<SentViewModel>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public bool IsBusy
        {
            get => _isBusy;

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