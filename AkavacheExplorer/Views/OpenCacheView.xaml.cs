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
