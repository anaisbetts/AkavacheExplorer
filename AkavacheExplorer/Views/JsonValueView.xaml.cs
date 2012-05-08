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
    /// Interaction logic for JsonValueView.xaml
    /// </summary>
    public partial class JsonValueView : UserControl, IViewForViewModel<JsonValueViewModel>
    {
        public JsonValueView()
        {
            InitializeComponent();
        }

        public JsonValueViewModel ViewModel {
            get { return (JsonValueViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(JsonValueViewModel), typeof(JsonValueView), new UIPropertyMetadata(null));

        object IViewForViewModel.ViewModel { 
            get { return ViewModel; }
            set { ViewModel = (JsonValueViewModel) value; } 
        }
    }
}
