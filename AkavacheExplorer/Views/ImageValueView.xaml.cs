using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AkavacheExplorer.ViewModels;
using ReactiveUI.Routing;
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
