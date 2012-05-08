using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Routing;

namespace AkavacheExplorer.ViewModels
{
    public interface ICacheViewModel : IRoutableViewModel
    {
        ReactiveCollection<string> Keys { get; }
        string SelectedKey { get; set; }
        ICacheValueViewModel SelectedValue { get; }
    }

    public class CacheViewModel : ReactiveObject, ICacheViewModel
    {
        public ReactiveCollection<string> Keys { get; protected set; }

        string _SelectedKey;
        public string SelectedKey {
            get { return _SelectedKey; }
            set { this.RaiseAndSetIfChanged(x => x.SelectedKey, value); }
        }

        ObservableAsPropertyHelper<ICacheValueViewModel> _SelectedValue;
        public ICacheValueViewModel SelectedValue {
            get { return _SelectedValue.Value; }
        }

        ObservableAsPropertyHelper<string> _UrlPathSegment;
        public string UrlPathSegment {
            get { return _UrlPathSegment.Value; }
        }

        public IScreen HostScreen { get; protected set; }

        public CacheViewModel(IScreen hostScreen, IAppState appState)
        {
            HostScreen = hostScreen;

            appState.WhenAny(x => x.CachePath, x => x.Value)
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .Select(x => (new DirectoryInfo(x)).Name)
                .ToProperty(this, x => x.UrlPathSegment);

            Keys = new ReactiveCollection<string>();
            appState.WhenAny(x => x.CurrentCache, x => x.Value).Subscribe(cache => {
                Keys.Clear();
                cache.GetAllKeys().ForEach(x => Keys.Add(x));
            });

            this.WhenAny(x => x.SelectedKey, x => x.Value)
                .Where(x => x != null)
                .SelectMany(x => appState.CurrentCache.GetAsync(x))
                .Select(createValueViewModel)
                .LoggedCatch(this, Observable.Return<ICacheValueViewModel>(null))
                .ToProperty(this, x => x.SelectedValue);
        }

        static ICacheValueViewModel createValueViewModel(byte[] x)
        {
            var ret = RxApp.GetService<ICacheValueViewModel>("Text");
            ret.Model = x;
            return ret;
        }
    }
}