using Akavache;
using AkavacheExplorer.Views;
using Ninject;
using ReactiveUI;

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
            set { this.RaiseAndSetIfChanged(ref _CurrentCache, value); }
        }

        string _CachePath;
        public string CachePath {
            get { return _CachePath; }
            set { this.RaiseAndSetIfChanged(ref _CachePath, value); }
        }

        public AppBootstrapper(IKernel kernel = null, IRoutingState router = null)
        {
            kernel = kernel ?? createStandardKernel();
            Router = router ?? new RoutingState();

            RxApp.DependencyResolver.ConfigureServiceLocator(
                (t, s) => kernel.Get(t, s),
                (t, s) => kernel.GetAll(t, s),
                (c, t, s) => kernel.Bind(t).To(c));

            // Our first screen is "Open cache"
            Router.Navigate.Execute(RxApp.DependencyResolver.GetService<IOpenCacheViewModel>());
        }

        IKernel createStandardKernel()
        {
            var ret = new StandardKernel();

            ret.Bind<IScreen>().ToConstant(this);
            ret.Bind<IAppState>().ToConstant(this);
            ret.Bind<IOpenCacheViewModel>().To<OpenCacheViewModel>();
            ret.Bind<IViewFor<OpenCacheViewModel>>().To<OpenCacheView>();
            ret.Bind<ICacheViewModel>().To<CacheViewModel>();
            ret.Bind<IViewFor<CacheViewModel>>().To<CacheView>();

            ret.Bind<ICacheValueViewModel>().To<TextValueViewModel>().Named("Text");
            ret.Bind<ICacheValueViewModel>().To<JsonValueViewModel>().Named("Json");
            ret.Bind<ICacheValueViewModel>().To<ImageValueViewModel>().Named("Image");
            ret.Bind<IViewFor<TextValueViewModel>>().To<TextValueView>();
            ret.Bind<IViewFor<JsonValueViewModel>>().To<JsonValueView>();
            ret.Bind<IViewFor<ImageValueViewModel>>().To<ImageValueView>();

            return ret;
        }
    }
}