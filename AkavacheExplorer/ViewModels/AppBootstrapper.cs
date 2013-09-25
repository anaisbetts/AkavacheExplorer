using Akavache;
using AkavacheExplorer.Views;
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

        public AppBootstrapper(IDependencyResolver kernel = null, IRoutingState router = null)
        {
            RxApp.DependencyResolver = kernel ?? createStandardKernel();
            Router = router ?? new RoutingState();

            // Our first screen is "Open cache"
            Router.Navigate.Execute(new OpenCacheViewModel(this, this));
        }

        IDependencyResolver createStandardKernel()
        {
            var r = RxApp.MutableResolver;

            r.RegisterConstant(this, typeof(IScreen));

            r.Register(() => new OpenCacheView(), typeof(IViewFor<OpenCacheViewModel>));
            r.Register(() => new CacheView(), typeof(IViewFor<CacheViewModel>));
            r.Register(() => new TextValueView(), typeof(IViewFor<TextValueViewModel>));
            r.Register(() => new JsonValueView(), typeof(IViewFor<JsonValueViewModel>));
            r.Register(() => new ImageValueView(), typeof(IViewFor<ImageValueViewModel>));

            return r;
        }
    }
}