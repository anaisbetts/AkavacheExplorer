using System.Windows;
using System.Windows.Controls;
using AkavacheExplorer.ViewModels;
using ReactiveUI;

namespace AkavacheExplorer.Views
{
    /// <summary>
    /// Interaction logic for JsonValueView.xaml
    /// </summary>
    public partial class JsonValueView : UserControl, IViewFor<JsonValueViewModel>
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

        object IViewFor.ViewModel { 
            get { return ViewModel; }
            set { ViewModel = (JsonValueViewModel) value; } 
        }
    }
}
