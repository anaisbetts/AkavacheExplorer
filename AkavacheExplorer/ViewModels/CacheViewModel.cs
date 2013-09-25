using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;

namespace AkavacheExplorer.ViewModels
{
    public interface ICacheViewModel : IRoutableViewModel
    {
        ReactiveList<string> Keys { get; }
        string SelectedKey { get; set; }
        ICacheValueViewModel SelectedValue { get; }
        string SelectedViewer { get; set; }
    }

    public class CacheViewModel : ReactiveObject, ICacheViewModel
    {
        public ReactiveList<string> Keys { get; protected set; }

        string _SelectedKey;
        public string SelectedKey {
            get { return _SelectedKey; }
            set { this.RaiseAndSetIfChanged(ref _SelectedKey, value); }
        }

        ObservableAsPropertyHelper<ICacheValueViewModel> _SelectedValue;
        public ICacheValueViewModel SelectedValue {
            get { return _SelectedValue.Value; }
        }

        string _SelectedViewer;
        public string SelectedViewer {
            get { return _SelectedViewer; }
            set { this.RaiseAndSetIfChanged(ref _SelectedViewer, value); }
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

            Keys = new ReactiveList<string>();
            appState.WhenAny(x => x.CurrentCache, x => x.Value).Subscribe(cache => {
                Keys.Clear();
                cache.GetAllKeys().ForEach(x => Keys.Add(x));
            });

            SelectedViewer = "Text";

            this.WhenAny(x => x.SelectedKey, x => x.SelectedViewer, (k,v) => k.Value)
                .Where(x => x != null && SelectedViewer != null)
                .SelectMany(x => appState.CurrentCache.GetAsync(x))
                .Select(x => createValueViewModel(x, SelectedViewer))
                .LoggedCatch(this, Observable.Return<ICacheValueViewModel>(null))
                .ToProperty(this, x => x.SelectedValue);
        }

        static ICacheValueViewModel createValueViewModel(byte[] x, string viewerType)
        {
            var ret = RxApp.DependencyResolver.GetService<ICacheValueViewModel>(viewerType);
            ret.Model = x;
            return ret;
        }
    }
}