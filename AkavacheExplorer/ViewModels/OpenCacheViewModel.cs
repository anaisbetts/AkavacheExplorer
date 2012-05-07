using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.Routing;

namespace AkavacheExplorer.ViewModels
{
    public interface IOpenCacheViewModel : IRoutableViewModel
    {
    }

    public class OpenCacheViewModel : ReactiveObject, IOpenCacheViewModel
    {
        public string UrlPathSegment {
            get { return "open"; }
        }

        public IScreen HostScreen { get; protected set; }

        public OpenCacheViewModel(IScreen hostScreen)
        {
            HostScreen = hostScreen;
        }
    }
}
