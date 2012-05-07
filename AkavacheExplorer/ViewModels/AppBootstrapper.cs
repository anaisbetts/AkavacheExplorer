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
            router = router ?? new RoutingState();

            RxApp.ConfigureServiceLocator(
                (t, s) => kernel.Get(t, s), (t, s) => kernel.GetAll(t, s));
        }

        IKernel createStandardKernel()
        {
            var ret = new StandardKernel();
            return ret;
        }
    }
}