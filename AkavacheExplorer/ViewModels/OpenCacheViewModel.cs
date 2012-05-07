using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Akavache;
using Akavache.Models;
using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;

namespace AkavacheExplorer.ViewModels
{
    public interface IOpenCacheViewModel : IRoutableViewModel
    {
        string CachePath { get; set; }
        bool OpenAsEncryptedCache { get; set; }
        ReactiveCommand OpenCache { get; }
    }

    public class OpenCacheViewModel : ReactiveObject, IOpenCacheViewModel
    {
        string _CachePath;
        public string CachePath {
            get { return _CachePath; }
            set { this.RaiseAndSetIfChanged(x => x.CachePath, value);  }
        }

        bool _OpenAsEncryptedCache;
        public bool OpenAsEncryptedCache {
            get { return _OpenAsEncryptedCache; }
            set { this.RaiseAndSetIfChanged(x => x.OpenAsEncryptedCache, value);  }
        }

        public ReactiveCommand OpenCache { get; protected set; }

        public string UrlPathSegment {
            get { return "open"; }
        }

        public IScreen HostScreen { get; protected set; }

        public OpenCacheViewModel(IScreen hostScreen, IAppState appState)
        {
            HostScreen = hostScreen;

            var isCachePathValid = this.WhenAny(x => x.CachePath, x => x.OpenAsEncryptedCache, (cp, _) => cp.Value)
                .Throttle(TimeSpan.FromMilliseconds(250), RxApp.DeferredScheduler)
                .Select(Directory.Exists);

            OpenCache = new ReactiveCommand(isCachePathValid);

            OpenCache.SelectMany(_ => openAkavacheCache(OpenAsEncryptedCache))
                .LoggedCatch(this, Observable.Return<IBlobCache>(null))
                .Subscribe(x => {
                    if (x == null) {
                        UserError.Throw("Couldn't open this cache");
                        return;
                    }

                    appState.CurrentCache = x;
                    hostScreen.Router.Navigate.Execute(RxApp.GetService<ICacheViewModel>());
                });        }

        IObservable<IBlobCache> openAkavacheCache(bool openAsEncrypted)
        {
            var ret = Observable.Start(() => openAsEncrypted ?
                (IBlobCache)new ReadonlyEncryptedBlobCache(CachePath) : (IBlobCache)new ReadonlyBlobCache(CachePath));
                
            return ret.SelectMany(x => x.GetAllKeys().Any() ? 
                Observable.Return(x) : 
                Observable.Throw<IBlobCache>(new Exception("Cache has no items")));
        }
    }
}