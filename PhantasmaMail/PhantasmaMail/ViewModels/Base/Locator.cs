using Autofac;
using PhantasmaMail.Services.Dialog;
using PhantasmaMail.Services.Navigation;
using System;

namespace PhantasmaMail.ViewModels.Base
{
    public class Locator
    {
        private IContainer _container;
        private readonly ContainerBuilder _containerBuilder;

        public static Locator Instance { get; } = new Locator();

        public Locator()
        {
            _containerBuilder = new ContainerBuilder();

            _containerBuilder.RegisterType<DialogService>().As<IDialogService>();
            _containerBuilder.RegisterType<NavigationService>().As<INavigationService>();

            _containerBuilder.RegisterType<ExtendedSplashViewModel>();
            _containerBuilder.RegisterType<LoginViewModel>();
            _containerBuilder.RegisterType<MainViewModel>();
            _containerBuilder.RegisterType<MenuViewModel>();
            _containerBuilder.RegisterType<InboxViewModel>();
            _containerBuilder.RegisterType<SentViewModel>();
            _containerBuilder.RegisterType<DraftViewModel>();
            _containerBuilder.RegisterType<DashboardViewModel>();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _containerBuilder.RegisterType<TImplementation>().As<TInterface>();
        }

        public void Register<T>() where T : class
        {
            _containerBuilder.RegisterType<T>();
        }

        public void Build()
        {
            if (_container != null) return;
            _container = _containerBuilder.Build();
        }
    }
}