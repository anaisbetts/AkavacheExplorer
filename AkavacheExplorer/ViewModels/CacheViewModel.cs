using System;
using ReactiveUI;
using ReactiveUI.Routing;

namespace AkavacheExplorer.ViewModels
{
    public interface ICacheViewModel : IRoutableViewModel
    {
    }

    public class CacheViewModel : ReactiveObject, ICacheViewModel
    {
        public string UrlPathSegment { get; protected set; }
        public IScreen HostScreen { get; protected set; }

        public CacheViewModel(IScreen hostScreen, IAppState appState)
        {
            HostScreen = hostScreen;
        }
    }
}