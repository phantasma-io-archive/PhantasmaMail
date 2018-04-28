using PhantasmaMail.Services.Authentication;
using PhantasmaMail.ViewModels;
using PhantasmaMail.ViewModels.Base;
using PhantasmaMail.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PhantasmaMail.Services.Navigation
{
    public partial class NavigationService : INavigationService
    {
        private readonly IAuthenticationService _authenticationService;
        protected readonly Dictionary<Type, Type> Mappings;

        protected Application CurrentApplication => Application.Current;

        public ViewModelBase GetCurrentViewModel()
        {
            var currentMainpage = (MasterDetailPage)CurrentApplication.MainPage;
            var detailPage = currentMainpage.Detail as CustomNavigationPage;
            var vm = detailPage?.CurrentPage.BindingContext as ViewModelBase;
            return vm;
        }

        //public NavigationService(IAuthenticationService authenticationService)

        public NavigationService()
        {
            _authenticationService = null;
            Mappings = new Dictionary<Type, Type>();

            CreatePageViewModelMappings();
        }

        public async Task InitializeAsync()
        {
            //TODO

            //if (await _authenticationService.UserIsAuthenticatedAndValidAsync())
            //if ()
            //      {
            //          //await NavigateToAsync<MainViewModel>();
            //      }
            //      else
            //      {
            //          //await NavigateToAsync<LoginViewModel>();
            //      }
            await NavigateToAsync<LoginViewModel>();
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return InternalNavigateToAsync(viewModelType, null);
        }

        public Task NavigateToAsync(Type viewModelType, object parameter)
        {
            return InternalNavigateToAsync(viewModelType, parameter);
        }

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage is MainView mainPage)
            {
                await mainPage.Detail.Navigation.PopAsync();
            }
            else if (CurrentApplication.MainPage != null)
            {
                await CurrentApplication.MainPage.Navigation.PopAsync();
            }
        }

        public virtual Task RemoveLastFromBackStackAsync()
        {
            if (CurrentApplication.MainPage is MainView mainPage)
            {
                mainPage.Detail.Navigation.RemovePage(
                    mainPage.Detail.Navigation.NavigationStack[mainPage.Detail.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreateAndBindPage(viewModelType, parameter);

            switch (page)
            {
                case MainView _:
                    CurrentApplication.MainPage = page;
                    break;
                case LoginView _:
                    CurrentApplication.MainPage = new CustomNavigationPage(page);
                    break;
                default:
                    if (CurrentApplication.MainPage is MainView mainPage)
                    {
                        if (mainPage.Detail is CustomNavigationPage navigationPage &&
                            (viewModelType != typeof(InboxViewModel) &&
                             viewModelType != typeof(SentViewModel) &&
                             viewModelType != typeof(DraftViewModel))) //menu items
                        {
                            var currentPage = navigationPage.CurrentPage;

                            if (currentPage.GetType() != page.GetType())
                            {
                                await navigationPage.PushAsync(page);
                            }
                        }
                        else
                        {
                            navigationPage = new CustomNavigationPage(page);
                            mainPage.Detail = navigationPage;
                        }

                        mainPage.IsPresented = false;
                    }
                    else
                    {
                        if (CurrentApplication.MainPage is CustomNavigationPage navigationPage)
                        {
                            await navigationPage.PushAsync(page);
                        }
                        else
                        {
                            CurrentApplication.MainPage = new CustomNavigationPage(page);
                        }
                    }

                    break;
            }

            await (page.BindingContext as ViewModelBase)?.InitializeAsync(parameter);
        }

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!Mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            return Mappings[viewModelType];
        }

        protected Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            ViewModelBase viewModel = Locator.Instance.Resolve(viewModelType) as ViewModelBase;
            page.BindingContext = viewModel;

            return page;
        }

        private void CreatePageViewModelMappings()
        {
            Mappings.Add(typeof(ExtendedSplashViewModel), typeof(ExtendedSplashView));
            Mappings.Add(typeof(LoginViewModel), typeof(LoginView));
            Mappings.Add(typeof(MainViewModel), typeof(MainView));
            Mappings.Add(typeof(InboxViewModel), typeof(InboxView));
            Mappings.Add(typeof(DraftViewModel), typeof(DraftView));
            Mappings.Add(typeof(SentViewModel), typeof(SentView));
            Mappings.Add(typeof(SettingsViewModel), typeof(SettingsView));
            Mappings.Add(typeof(MessageDetailViewModel), typeof(MessageDetailView));
        }
    }
}