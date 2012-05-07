using System;
using System.IO;
using System.Linq;
using ReactiveUI;
using ReactiveUI.Routing;

namespace AkavacheExplorer.ViewModels
{
    public interface ICacheViewModel : IRoutableViewModel
    {
        ReactiveCollection<string> Keys { get; }
        string SelectedKey { get; set; }
    }

    public class CacheViewModel : ReactiveObject, ICacheViewModel
    {
        public ReactiveCollection<string> Keys { get; protected set; }

        string _SelectedKey;
        public string SelectedKey {
            get { return _SelectedKey; }
            set { this.RaiseAndSetIfChanged(x => x.SelectedKey, value); }
        }

        ObservableAsPropertyHelper<string> _UrlPathSegment;
        public string UrlPathSegment {
            get { return _UrlPathSegment.Value; }
        }

        public IScreen HostScreen { get; protected set; }

        public CacheViewModel(IScreen hostScreen, IAppState appState)
        {
            HostScreen = hostScreen;

            appState.WhenAny(x => x.CachePath, x => "cache_" + (new DirectoryInfo(x.Value)).Name)
                .ToProperty(this, x => x.UrlPathSegment);

            Keys = new ReactiveCollection<string>();
            appState.WhenAny(x => x.CurrentCache, x => x.Value).Subscribe(cache => {
                Keys.Clear();
                cache.GetAllKeys().ForEach(x => Keys.Add(x));
            });
        }
    }
}