using AkavacheExplorer.Views;
using Ninject;
using ReactiveUI;
using ReactiveUI.Routing;

namespace AkavacheExplorer.ViewModels
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public IRoutingState Router { get; protected set; }

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
            ret.Bind<IOpenCacheViewModel>().To<OpenCacheViewModel>();
            ret.Bind<IViewForViewModel<OpenCacheViewModel>>().To<OpenCacheView>();

            return ret;
        }
    }
}