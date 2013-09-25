using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Akavache;
using Akavache.Models;
using ReactiveUI;
using ReactiveUI.Xaml;
using Akavache.Sqlite3;

namespace AkavacheExplorer.ViewModels
{
    public interface IOpenCacheViewModel : IRoutableViewModel
    {
        string CachePath { get; set; }
        bool OpenAsEncryptedCache { get; set; }
        bool OpenAsSqlite3Cache { get; set; }
        ReactiveCommand OpenCache { get; }
    }

    public class OpenCacheViewModel : ReactiveObject, IOpenCacheViewModel
    {
        string _CachePath;
        public string CachePath {
            get { return _CachePath; }
            set { this.RaiseAndSetIfChanged(ref _CachePath, value);  }
        }

        bool _OpenAsEncryptedCache;
        public bool OpenAsEncryptedCache {
            get { return _OpenAsEncryptedCache; }
            set { this.RaiseAndSetIfChanged(ref _OpenAsEncryptedCache, value);  }
        }

        bool _OpenAsSqlite3Cache;
        public bool OpenAsSqlite3Cache {
            get { return _OpenAsSqlite3Cache; }
            set { this.RaiseAndSetIfChanged(ref _OpenAsSqlite3Cache, value);  }
        }

        public ReactiveCommand OpenCache { get; protected set; }

        public string UrlPathSegment {
            get { return "open"; }
        }

        public IScreen HostScreen { get; protected set; }

        public OpenCacheViewModel(IScreen hostScreen, IAppState appState)
        {
            HostScreen = hostScreen;

            var isCachePathValid = this.WhenAny(
                    x => x.CachePath, x => x.OpenAsEncryptedCache, x => x.OpenAsSqlite3Cache,
                    (cp, _, sql) => new { Path = cp.Value, Sqlite3 = sql.Value })
                .Throttle(TimeSpan.FromMilliseconds(250), RxApp.MainThreadScheduler)
                .Select(x => x.Sqlite3 ? File.Exists(x.Path) : Directory.Exists(x.Path));

            OpenCache = new ReactiveCommand(isCachePathValid);

            OpenCache.SelectMany(_ => openAkavacheCache(CachePath, OpenAsEncryptedCache, OpenAsSqlite3Cache))
                .LoggedCatch(this, Observable.Return<IBlobCache>(null))
                .Subscribe(x => {
                    if (x == null) {
                        UserError.Throw("Couldn't open this cache");
                        return;
                    }

                    appState.CurrentCache = x;
                    hostScreen.Router.Navigate.Execute(RxApp.DependencyResolver.GetService<ICacheViewModel>());
                });
        }

        IObservable<IBlobCache> openAkavacheCache(string path, bool openAsEncrypted, bool openAsSqlite3)
        {
            var ret = default(IObservable<IBlobCache>);

            if (openAsSqlite3) {
                ret = Observable.Start(() => openAsEncrypted ?
                    (IBlobCache)new Akavache.Sqlite3.EncryptedBlobCache(path) : (IBlobCache)new SqlitePersistentBlobCache(path));
            } else {
                ret = Observable.Start(() => openAsEncrypted ?
                    (IBlobCache)new ReadonlyEncryptedBlobCache(path) : (IBlobCache)new ReadonlyBlobCache(path));
            }
                
            return ret.SelectMany(x => x.GetAllKeys().Any() ? 
                Observable.Return(x) : 
                Observable.Throw<IBlobCache>(new Exception("Cache has no items")));
        }
    }
}