using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using ReactiveUI;

namespace AkavacheExplorer.ViewModels
{
    public interface ICacheValueViewModel : IReactiveNotifyPropertyChanged
    {
        byte[] Model { get; set; }
    }

    public class TextValueViewModel : ReactiveObject, ICacheValueViewModel
    {
        byte[] _Model;
        public byte[] Model {
            get { return _Model; }
            set { this.RaiseAndSetIfChanged(x => x.Model, value); }
        }

        ObservableAsPropertyHelper<string> _TextToDisplay;
        public string TextToDisplay {
            get { return _TextToDisplay.Value; }
        }

        public TextValueViewModel()
        {
            this.WhenAny(x => x.Model, x => x.Value)
                .Where(x => x != null)
                .Select(x => {
                    var ret = Encoding.UTF8.GetString(x);
                    return ret;
                })
                .ToProperty(this, x => x.TextToDisplay);
        }
    }
}
