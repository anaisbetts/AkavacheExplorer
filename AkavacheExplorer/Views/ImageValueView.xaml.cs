using System.Windows;
using System.Windows.Controls;
using AkavacheExplorer.ViewModels;
using ReactiveUI;

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
