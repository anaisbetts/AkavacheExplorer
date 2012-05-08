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

namespace AkavacheExplorer.Views
{
    /// <summary>
    /// Interaction logic for TextValueView.xaml
    /// </summary>
    public partial class TextValueView : UserControl, IViewForViewModel<TextValueViewModel>
    {
        public TextValueView()
        {
            InitializeComponent();
        }

        public TextValueViewModel ViewModel {
            get { return (TextValueViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(TextValueViewModel), typeof(TextValueView), new UIPropertyMetadata(null));

        object IViewForViewModel.ViewModel { 
            get { return ViewModel; }
            set { ViewModel = (TextValueViewModel) value; } 
        }
    }
}
