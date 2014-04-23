using System.Windows;
using System.Windows.Controls;
using AkavacheExplorer.ViewModels;
using ReactiveUI;

namespace AkavacheExplorer.Views
{
    /// <summary>
    /// Interaction logic for OpenCacheView.xaml
    /// </summary>
    public partial class OpenCacheView : UserControl, IViewFor<OpenCacheViewModel>
    {
        public OpenCacheView()
        {
            InitializeComponent();

            this.Bind(ViewModel, x => x.CachePath, x => x.CachePath.Text);
            this.Bind(ViewModel, x => x.OpenAsEncryptedCache, x => x.OpenAsEncryptedCache.IsChecked);
            this.Bind(ViewModel, x => x.OpenAsSqlite3Cache, x => x.OpenAsSqlite3Cache.IsChecked);

            this.BindCommand(ViewModel, x => x.OpenCache, x => x.OpenCache);
            this.BindCommand(ViewModel, x => x.BrowseForCache, x => x.browseButton);
        }

        public OpenCacheViewModel ViewModel {
            get { return (OpenCacheViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(OpenCacheViewModel), typeof(OpenCacheView), new UIPropertyMetadata(null));

        object IViewFor.ViewModel { 
            get { return ViewModel; }
            set { ViewModel = (OpenCacheViewModel) value; } 
        }
    }
}
