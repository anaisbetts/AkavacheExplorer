using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
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
using ReactiveUI.Xaml;
using ReactiveUI;

namespace AkavacheExplorer.Views
{
    /// <summary>
    /// Interaction logic for CacheView.xaml
    /// </summary>
    public partial class CacheView : UserControl, IViewFor<CacheViewModel>
    {
        public CacheView()
        {
            InitializeComponent();

            RxApp.DeferredScheduler.Schedule(() => {
                new[] { textRadio, jsonRadio, imageRadio }
                    .Select(y => y.WhenAny(x => x.IsChecked, x => x).Where(x => x.Value == true).Select(x => x.Sender.Tag))
                    .Merge()
                    .Subscribe(x => ViewModel.SelectedViewer = (string)x);
            });
        }

        public CacheViewModel ViewModel {
            get { return (CacheViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(CacheViewModel), typeof(CacheView), new UIPropertyMetadata(null));

        object IViewFor.ViewModel { 
            get { return ViewModel; }
            set { ViewModel = (CacheViewModel) value; } 
        }
    }
}
