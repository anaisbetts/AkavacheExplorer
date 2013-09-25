using System.Windows;
using System.Windows.Controls;
using AkavacheExplorer.ViewModels;
using ReactiveUI;

namespace AkavacheExplorer.Views
{
    /// <summary>
    /// Interaction logic for TextValueView.xaml
    /// </summary>
    public partial class TextValueView : UserControl, IViewFor<TextValueViewModel>
    {
        public TextValueView()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, x => x.TextToDisplay, x => x.TextToDisplay.Text);
        }

        public TextValueViewModel ViewModel {
            get { return (TextValueViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(TextValueViewModel), typeof(TextValueView), new UIPropertyMetadata(null));

        object IViewFor.ViewModel { 
            get { return ViewModel; }
            set { ViewModel = (TextValueViewModel) value; } 
        }
    }
}
