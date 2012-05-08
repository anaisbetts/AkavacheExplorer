using Akavache;
using AkavacheExplorer.Views;
using Ninject;
using ReactiveUI;
using ReactiveUI.Routing;

namespace AkavacheExplorer.ViewModels
{
    public interface IAppState : IReactiveNotifyPropertyChanged
    {
        IBlobCache CurrentCache { get; set; }
        string CachePath { get; set; }
    }

    public class AppBootstrapper : ReactiveObject, IScreen, IAppState
    {
        public IRoutingState Router { get; protected set; }

        IBlobCache _CurrentCache;
        public IBlobCache CurrentCache {
            get { return _CurrentCache; }
            set { this.RaiseAndSetIfChanged(x => x.CurrentCache, value); }
        }

        string _CachePath;
        public string CachePath {
            get { return _CachePath; }
            set { this.RaiseAndSetIfChanged(x => x.CachePath, value); }
        }

        public AppBootstrapper(IKernel kernel = null, IRoutingState router = null)
        {
            kernel = kernel ?? createStandardKernel();
            Router = router ?? new RoutingState();

            RxApp.ConfigureServiceLocator(
                (t, s) => kernel.Get(t, s), (t, s) => kernel.GetAll(t, s));

            // Our first screen is "Open cache"
            Router.Navigate.Execute(RxApp.GetService<IOpenCacheViewModel>());
        }

        IKernel createStandardKernel()
        {
            var ret = new StandardKernel();

            ret.Bind<IScreen>().ToConstant(this);
            ret.Bind<IAppState>().ToConstant(this);
            ret.Bind<IOpenCacheViewModel>().To<OpenCacheViewModel>();
            ret.Bind<IViewForViewModel<OpenCacheViewModel>>().To<OpenCacheView>();
            ret.Bind<ICacheViewModel>().To<CacheViewModel>();
            ret.Bind<IViewForViewModel<CacheViewModel>>().To<CacheView>();

            ret.Bind<ICacheValueViewModel>().To<TextValueViewModel>().Named("Text");
            ret.Bind<ICacheValueViewModel>().To<JsonValueViewModel>().Named("Json");
            ret.Bind<IViewForViewModel<TextValueViewModel>>().To<TextValueView>();
            ret.Bind<IViewForViewModel<JsonValueViewModel>>().To<JsonValueView>();

            return ret;
        }
    }
}