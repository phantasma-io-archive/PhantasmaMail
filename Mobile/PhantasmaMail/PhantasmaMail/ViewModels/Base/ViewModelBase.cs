using PhantasmaMail.Services.Dialog;
using PhantasmaMail.Services.Navigation;
using System.Threading.Tasks;
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


        protected ViewModelBase()
        {
            DialogService = Locator.Instance.Resolve<IDialogService>();
            NavigationService = Locator.Instance.Resolve<INavigationService>();
            AuthenticationService = Locator.Instance.Resolve<IAuthenticationService>();
            PhantasmaService = Locator.Instance.Resolve<IPhantasmaService>();
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