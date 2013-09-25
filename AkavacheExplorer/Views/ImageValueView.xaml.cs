using System.Windows;
using System.Windows.Controls;
using System.Reactive.Linq;
using AkavacheExplorer.ViewModels;
using ReactiveUI;
using Splat;

namespace AkavacheExplorer.Views
{
    /// <summary>
    /// Interaction logic for ImageValueView.xaml
    /// </summary>
    public partial class ImageValueView : UserControl, IViewFor<ImageValueViewModel>
    {
        public ImageValueView()
        {
            InitializeComponent();

            this.WhenAny(x => x.ViewModel.Image, x => x.Value)
                .Where(x => x != null)
                .Select(x => x.ToNative())
                .BindTo(this, x => x.Image.Source);

            this.OneWayBind(ViewModel, x => x.ImageVisibility, x => x.Image.Visibility);
            this.OneWayBind(ViewModel, x => x.ErrorVisibility, x => x.ErrorText.Visibility);
        }

        public ImageValueViewModel ViewModel {
            get { return (ImageValueViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ImageValueViewModel), typeof(ImageValueView), new UIPropertyMetadata(null));

        object IViewFor.ViewModel { 
            get { return ViewModel; }
            set { ViewModel = (ImageValueViewModel) value; } 
        }
    }
}
