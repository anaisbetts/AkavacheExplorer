using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Akavache;
using Newtonsoft.Json;
using ReactiveUI;
using Splat;
using System.IO;

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
            set { this.RaiseAndSetIfChanged(ref _Model, value); }
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

    public class JsonValueViewModel : ReactiveObject, ICacheValueViewModel
    {
        byte[] _Model;
        public byte[] Model {
            get { return _Model; }
            set { this.RaiseAndSetIfChanged(ref _Model, value); }
        }

        ObservableAsPropertyHelper<string> _TextToDisplay;
        public string TextToDisplay {
            get { return _TextToDisplay.Value; }
        }

        public JsonValueViewModel()
        {
            this.WhenAny(x => x.Model, x => x.Value)
                .Where(x => x != null)
                .Select(x => {
                    var ret = Encoding.UTF8.GetString(x);
                    return ret;
                })
                .Select<string, string>(x => {
                    try {
                        dynamic ret = JsonConvert.DeserializeObject(x);
                        return JsonConvert.SerializeObject(ret, Formatting.Indented);
                    } catch (Exception ex) {
                        return ex.ToString();
                    }
                })
                .ToProperty(this, x => x.TextToDisplay);
        }
    }

    public class ImageValueViewModel : ReactiveObject, ICacheValueViewModel
    {
        byte[] _Model;
        public byte[] Model
        {
            get { return _Model; }
            set { this.RaiseAndSetIfChanged(ref _Model, value); }
        }

        ObservableAsPropertyHelper<IBitmap> _Image;
        public IBitmap Image {
            get { return _Image.Value; }
        }

        ObservableAsPropertyHelper<Visibility> _ImageVisibility;
        public Visibility ImageVisibility {
            get { return _ImageVisibility.Value; }
        }

        ObservableAsPropertyHelper<Visibility> _ErrorVisibility;
        public Visibility ErrorVisibility {
            get { return _ErrorVisibility.Value; }
        }

        public ImageValueViewModel()
        {
            this.WhenAny(x => x.Model, x => x.Value)
                .Where(x => x != null)
                .SelectMany(x => BitmapLoader.Current.Load(new MemoryStream(x), null, null))
                .LoggedCatch(this, Observable.Return<IBitmap>(null))
                .ToProperty(this, x => x.Image);

            this.WhenAny(x => x.Image, x => x.Value != null ? Visibility.Visible : Visibility.Hidden)
                .ToProperty(this, x => x.ImageVisibility);
            this.WhenAny(x => x.ImageVisibility, x => x.Value == Visibility.Visible ? Visibility.Hidden : Visibility.Visible)
                .ToProperty(this, x => x.ErrorVisibility);
        }
    }
}